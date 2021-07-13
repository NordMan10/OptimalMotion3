using System;
using System.Collections.Generic;
using System.Linq;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Domain.Static;

namespace OptimalMotion2.Domain
{
    public class BundleGenerator : IBundleGenerator
    {
        public BundleGenerator(IAircraftGenerator aircraftGenerator)
        {
            AircraftGenerator = aircraftGenerator;
        }

        private IAircraftGenerator AircraftGenerator { get; }

        private static BundleGenerator _instance;
        private static readonly object SyncRoot = new object();
        private readonly List<IInterval> createdIntervals = new List<IInterval>();
        private readonly Random random = new Random();

        public IAircraftBundle GetTakingOffBundle(IRunway runway, ISpecPlatform specPlatform)
        {
            // Объявляем переменную первого момента взлета
            IMoment firstTakeOffMoment;

            while (true)
            {
                var intervalsNotIntersect = true;
                // Получаем первый момент взлета
                firstTakeOffMoment = GetTakeoffMoment();
                // Получаем интервал, который занимает вся пачка ВС
                var bundleInterval = GetTakingOffBundleInterval(firstTakeOffMoment);
                // Проверяем пересечение интервала с уже существующими интервалами
                foreach (var interval in createdIntervals)
                {
                    // Если пересекаются => уходим на новую итерацию
                    if (bundleInterval.Intersects(interval))
                    {
                        intervalsNotIntersect = false;
                    }
                }

                // Если не пересекаются => сохраняем полученный интервал и прерываем цикл. В переменной firstTakeOffMoment записан первый момент взлета
                if (intervalsNotIntersect)
                {
                    createdIntervals.Add(bundleInterval);
                    break;
                }
            }

            var aircrafts = new List<IAircraft>();
            for (var i = 0; i < ModellingParameters.TakingOffBundleAircraftCount; i++)
            {
                aircrafts.Add(AircraftGenerator.GetTakingOffAircraft(
                    new Moment(firstTakeOffMoment.Value + AircraftMotionParameters.IntervalBetweenTakingOff * i), 
                    runway, specPlatform));
            }

            return new AircraftBundle(aircrafts);
        }

        public IAircraftBundle GetLandingBundle(List<IMoment> moments, int runwayIndex = 0)
        {
            var bundleMoments = moments;
            var landingIntervals = new LandingAircraftIntervals(
                new Interval(new Moment(0), new Moment(AircraftMotionParameters.LandingInterval)));

            var aircrafts = new List<ILandingAircraft>();
            for (var i = 0; i < bundleMoments.Count; i++)
            {
                var landingMoments = new LandingAircraftMoments(bundleMoments[i]);
                aircrafts.Add(AircraftGenerator.GetLandingAircraft(landingMoments, landingIntervals, runwayIndex));
            }

            return new AircraftBundle(aircrafts);
        }

        IBundleGenerator IBundleGenerator.GetInstance(IAircraftGenerator aircraftGenerator)
        {
            return GetInstance(aircraftGenerator);
        }

        public static IBundleGenerator GetInstance(IAircraftGenerator aircraftGenerator)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new BundleGenerator(aircraftGenerator);
                }
            }
            return _instance;
        }

        public void Reset()
        {
            createdIntervals.Clear();
        }

        /// <summary>
        /// Returns takeoff moment in seconds
        /// </summary>
        /// <returns>Takeoff moment in seconds</returns>
        private IMoment GetTakeoffMoment()
        {
            if (createdIntervals.Count == 0)
            {
                return GetFirstTakeOffMoment();
            }

            return new Moment(random.Next(createdIntervals.Last().LastMoment.Value,
                ModellingParameters.ModellingTime));
        }

        /// <summary>
        /// Returns takeoff moment of first aircraft in bundle from specified interval [0, 48] in minutes
        /// </summary>
        /// <returns>Takeoff moment in minutes</returns>
        private IMoment GetFirstTakeOffMoment()
        {
            var firstTakeOffMoment = random.Next(ModellingParameters.FirstAircraftModellingInterval.Start, 
                ModellingParameters.FirstAircraftModellingInterval.End);

            return new Moment(firstTakeOffMoment);
        }

        /// <summary>
        /// Returns interval, which bundle occupies
        /// </summary>
        /// <param name="firstTakeOffMoment"></param>
        /// <returns></returns>
        private IInterval GetTakingOffBundleInterval(IMoment firstTakeOffMoment)
        {
            // Берем ModellingParameters.TakingOffBundleCount - 1, поскольку учитываем интервалы только между ВС, а у последнего 
            // учитываем только AircraftMotionParameters.TakingOffInterval
            return new Interval(new Moment(firstTakeOffMoment.Value),
                new Moment(firstTakeOffMoment.Value + ((ModellingParameters.TakingOffBundleAircraftCount - 1) * 
                AircraftMotionParameters.IntervalBetweenTakingOff) + AircraftMotionParameters.TakingOffInterval));
        }
    }
}