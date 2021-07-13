using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Domain.Static;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class Model : IModel
    {
        public Model(int runwayCount, int specPlatformCount, IChart chart, ITable table)
        {
            this.chart = chart;
            this.table = table;
            aircraftGenerator = new AircraftGenerator(aircraftIdGenerator);
            bundleGenerator = new BundleGenerator(aircraftGenerator);
            InitRunways(runwayCount);
            InitSpecPlatforms(specPlatformCount);
        }

        private readonly Stopwatch generalStopwatch = new Stopwatch();
        private readonly IAircraftIdGenerator aircraftIdGenerator = AircraftIdGenerator.GetInstance(1);
        private readonly IAircraftGenerator aircraftGenerator;
        private readonly IBundleGenerator bundleGenerator;
        private Dictionary<int, IRunway> runways;
        private Dictionary<int, ISpecPlatform> specPlatforms;
        private readonly IChart chart;
        private readonly ITable table;
        private ModelStages stage = ModelStages.Started;
        private IAircraftBundleConflictResolver bundleConflictResolver = new AircraftBundleConflictResolver();

        public void ResetIdGenerator()
        {
            aircraftIdGenerator.Reset();
        }

        /// <summary>
        /// Обработка события изменения {Стадии выполнения}: (6)
        /// </summary>
        /// <param name="stage"></param>
        public void ChangeStage(ModelStages stage)
        {
            this.stage = stage;

            switch (stage)
            {
                case ModelStages.Preparing:
                    PrepareStageHandler();
                    break;
                case ModelStages.Started:
                    StartedStageHandler();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, "Передана неизвестная стадия выполнения(lol)");
            }
        }

        private void PrepareStageHandler()
        {
            // Сбрасываем секундомер;
            generalStopwatch.Reset();

            runways[0].LandingBundles.Clear();
            runways[0].TakingOffBundles.Clear();
            runways[0].OutdatedBundles.Clear();
            
            chart.Reset();
            bundleGenerator.Reset();
            table.Reset();
        }

        private void StartedStageHandler()
        {
            generalStopwatch.Start();
            
            var specPlatform = specPlatforms[0];

            foreach (var runway in runways.Values)
            {
                // Регистрируем все пачки прилетающих ВС
                AddLandingBundles(runway);


                var takingOffBundle = bundleGenerator.GetTakingOffBundle(runway, specPlatform);
                var nominalEngineStartMoments = takingOffBundle.GetNominalEngineStartMoments();

                // Регистрируем ее в ВПП
                AddTakingOffBundle(takingOffBundle, runway);
                // Обрататываем возможные пересечения
                CheckAndResolveBundlesConflicts(takingOffBundle, runway, specPlatform);
                var orderedTakingOffAircrafts = takingOffBundle.GetAircrafts().OrderBy(a => a.OrderMoment.Value).ToList();
                SetMomentsForAllAircrafts(takingOffBundle, orderedTakingOffAircrafts);
                
                // Добавляем данные в таблицу
                AddDataToTable(orderedTakingOffAircrafts, nominalEngineStartMoments);
                // Добавляем данные о взлетающих и прилетающих ВС, зарегистрированных на ВПП
                AddDataToChart(runway);
            }
        }

        private void AddDataToTable(List<IAircraft> orderedTakingOffAircrafts,
            Dictionary<IAircraftId, IMoment> nominalEngineStartMoments)
        {
            foreach (var aircraft in orderedTakingOffAircrafts)
            {
                var takingOffAircraft = (TakingOffAircraft)aircraft;
                AddAircraftDataToTable(takingOffAircraft, nominalEngineStartMoments);
            }
        }

        private void AddAircraftDataToTable(ITakingOffAircraft takingOffAircraft, 
            Dictionary<IAircraftId, IMoment> nominalEngineStartMoments)
        {
            var nominalEngineStartMoment = nominalEngineStartMoments.
                    Where(m => m.Key.Id == takingOffAircraft.Id.Id).First().Value;

            var tableData = new TableRowData(takingOffAircraft.Id.Id.ToString(), takingOffAircraft.OrderMoment.Value.ToString(),
                nominalEngineStartMoment.Value.ToString(), takingOffAircraft.Moments.EngineStart.Value.ToString(),
                takingOffAircraft.ProcessingIsNeeded);

            table.AddRow(tableData);
        }

        #region Applying aircraft moments

        private void SetMomentsForAllAircrafts(IAircraftBundle takingOffBundle, List<IAircraft> orderedTakingOffAircrafts)
        {
            for (var i = 0; i < orderedTakingOffAircrafts.Count; i++)
            {
                var takingOffAircraft = (TakingOffAircraft)orderedTakingOffAircrafts[i];
                takingOffAircraft.SetAircraftMoments(i, takingOffBundle);
            }
        }

        #endregion

        private void AddBundlesToChart(IRunway runway, AircraftBehavior type, 
            ChartMomentDataType subType = ChartMomentDataType.Default)
        {
            var bundles = type == AircraftBehavior.Landing ? runway.LandingBundles : runway.TakingOffBundles;
            bundles = subType == ChartMomentDataType.Outdated ? runway.OutdatedBundles : bundles;

            for (var i = 0; i < bundles.Count; i++)
            {
                var bundleAircrafts = bundles[i].GetAircrafts();
                for (var j = 0; j < bundleAircrafts.Count; j++)
                {
                    chart.AddMoment(new ChartMomentData(bundleAircrafts[j].Id,
                        bundleAircrafts[j].OrderMoment, type, subType));

                    #region Это был чисто проверочный код (может пригодится)
                    //var ids = outerAircrafts.Select(t => t.Id.Id).ToList();
                    //if (ids.Contains(bundleAircrafts[j].Id.Id))
                    //{
                    //    chart.AddMoment(new ChartMomentData(bundleAircrafts[j].Id, 
                    //        bundleAircrafts[j].OrderMoment, type, ChartMomentDataType.Conflict));
                    //    continue;
                    //}
                    #endregion
                }
            }
        }

        private void AddDataToChart(IRunway runway)
        {
            AddBundlesToChart(runway, AircraftBehavior.Landing);
            AddBundlesToChart(runway, AircraftBehavior.TakingOff);
            AddBundlesToChart(runway, AircraftBehavior.TakingOff, ChartMomentDataType.Outdated);
        }

        private void InitRunways(int runwayCount)
        {
            runways = new Dictionary<int, IRunway>();
            for (var i = 0; i < runwayCount; i++)
            {
                var runway = new Runway(i);
                runways.Add(i, runway);
            }
        }

        private void InitSpecPlatforms(int specPlatformCount)
        {
            specPlatforms = new Dictionary<int, ISpecPlatform>();
            for (var i = 0; i < specPlatformCount; i++)
            {
                var specPlatform = new SpecPlatform(i);
                specPlatforms.Add(i, specPlatform);
            }
        }

        private void AddLandingBundles(IRunway runway)
        {
            for (var i = 0; i < LandingBundlesData.BundlesData.Count; i++)
            {
                runway.AddLandingBundle(bundleGenerator.GetLandingBundle(LandingBundlesData.BundlesData[i]));
            }
        }

        private void AddTakingOffBundle(IAircraftBundle bundle, IRunway runway)
        {
            runway.AddTakingOffBundle(bundle);
        }

        /// <summary>
        /// Проверяет наличие пересечения и обрабатывает его
        /// </summary>
        /// <param name="takingOffBundle"></param>
        /// <param name="runway"></param>
        /// <param name="specPlatform"></param>
        private void CheckAndResolveBundlesConflicts(IAircraftBundle takingOffBundle, IRunway runway, ISpecPlatform specPlatform)
        {
            IntersectionCases intersectionCase = IntersectionCases.Init;
            var intersectedBundle = runway.GetIntersectedBundleAndSetCase(takingOffBundle, ref intersectionCase);

            if (intersectionCase == IntersectionCases.Init)
                return;

            var orderedLandingBundles = runway.LandingBundles
                .OrderBy(bundle => bundle.FirstMoment.Value).ToList();

            var intersectedBundles = new List<IAircraftBundle> { intersectedBundle };
            var outerAircrafts = GetOuterAircrafts(intersectedBundles, takingOffBundle);

            var intersectedBundleIndex = orderedLandingBundles.IndexOf(intersectedBundle);
            var emptyWindows = new List<IInterval>();
            switch (intersectionCase)
            {
                case IntersectionCases.Right:
                    emptyWindows = bundleConflictResolver.GetEmptyWindowsForRightCase(intersectedBundle, takingOffBundle,
                        orderedLandingBundles, intersectedBundleIndex, intersectionCase);
                    SaveOutdatedBundle(takingOffBundle, runway, specPlatform);
                    foreach (var window in emptyWindows)
                        SpreadOuterAircraftsInWindow(window, outerAircrafts, runway, specPlatform);
                    break;
                case IntersectionCases.Left:
                    emptyWindows = bundleConflictResolver.GetEmptyWindowsForLeftCase(intersectedBundle, takingOffBundle,
                        orderedLandingBundles, intersectedBundleIndex, intersectionCase);
                    SaveOutdatedBundle(takingOffBundle, runway, specPlatform);
                    foreach (var window in emptyWindows)
                        SpreadOuterAircraftsInWindow(window, outerAircrafts, runway, specPlatform);
                    break;
                case IntersectionCases.Middle:
                    emptyWindows = bundleConflictResolver.GetEmptyWindowsForOtherCases(intersectedBundle, orderedLandingBundles, 
                        intersectedBundleIndex, intersectionCase);
                    SaveOutdatedBundle(takingOffBundle, runway, specPlatform);
                    foreach (var window in emptyWindows)
                        SpreadOuterAircraftsInWindow(window, outerAircrafts, runway, specPlatform);
                    break;
                case IntersectionCases.RightAndLeft:
                    var thisIntersectedBundles = new List<IAircraftBundle> { intersectedBundle, 
                        orderedLandingBundles[intersectedBundleIndex + 1] };
                    var thisOuterAircrafts = GetOuterAircrafts(thisIntersectedBundles, takingOffBundle);

                    emptyWindows = bundleConflictResolver.GetEmptyWindowsForOtherCases(intersectedBundle, orderedLandingBundles, 
                        intersectedBundleIndex, intersectionCase);
                    break;
                case IntersectionCases.Out:
                    emptyWindows = bundleConflictResolver.GetEmptyWindowsForOtherCases(intersectedBundle, orderedLandingBundles, 
                        intersectedBundleIndex, intersectionCase);
                    break;
                default:
                    outerAircrafts.Clear();
                    break;
            }
        }

        /// <summary>
        /// Возвращает не вместившиеся ВС
        /// </summary>
        /// <param name="intersectedBundles"></param>
        /// <param name="takingOffBundle"></param>
        /// <returns></returns>
        private List<IAircraft> GetOuterAircrafts(List<IAircraftBundle> intersectedBundles, IAircraftBundle takingOffBundle)
        {
            var outerAircrafts = new List<IAircraft>();
            var takingOffAircrafts = takingOffBundle.GetAircrafts();
            for (var i = 0; i < takingOffAircrafts.Count; i++)
            {
                foreach (var intersectedBundle in intersectedBundles)
                {
                    var bundleInterval = new Interval(new Moment(intersectedBundle.FirstMoment.Value),
                        new Moment(intersectedBundle.LastMoment.Value));
                    if (bundleInterval.IsMomentInInterval(takingOffAircrafts[i].OrderMoment))
                    {
                        outerAircrafts.Add(takingOffAircrafts[i]);
                    }
                }
            }

            return outerAircrafts;
        }

        /// <summary>
        /// Распределяет не вместившиеся ВС по свободным окнам
        /// </summary>
        /// <param name="window"></param>
        /// <param name="outerAircrafts"></param>
        /// <param name="runway"></param>
        /// <param name="specPlatform"></param>
        /// <returns></returns>
        private List<IAircraft> SpreadOuterAircraftsInWindow(IInterval window, List<IAircraft> outerAircrafts, 
            IRunway runway, ISpecPlatform specPlatform)
        {
            var aircraftCounter = 0;
            var newTakingOffMoment = new Moment(window.FirstMoment.Value);
            while (window.LastMoment.Value >= newTakingOffMoment.Value && outerAircrafts.Count > 0)
            {
                
                outerAircrafts[0].OrderMoment = newTakingOffMoment;
                outerAircrafts.RemoveAt(0);

                aircraftCounter++;
                // Смещение для ВС относительно первого перемещенного в окне
                var shift = aircraftCounter * AircraftMotionParameters.IntervalBetweenTakingOff;
                newTakingOffMoment = new Moment(window.FirstMoment.Value + shift);
            }
            return outerAircrafts;
        }

        /// <summary>
        /// Сохраняет устаревшие данные моментов вылета ВС для последующего отображения
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="runway"></param>
        /// <param name="specPlatform"></param>
        private void SaveOutdatedBundle(IAircraftBundle bundle, IRunway runway, ISpecPlatform specPlatform)
        {
            var aircrafts = new List<IAircraft>();
            foreach (var aircraft in bundle.GetAircrafts())
            {
                aircrafts.Add(aircraftGenerator.GetTakingOffAircraft(aircraft.OrderMoment, runway, specPlatform));
            }
            runway.AddOutdatedBundle(new AircraftBundle(aircrafts));
        }
    }
}

