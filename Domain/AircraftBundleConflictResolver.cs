using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Domain.Static;
using OptimalMotion2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public class AircraftBundleConflictResolver : IAircraftBundleConflictResolver
    {
        public AircraftBundleConflictResolver() { }

        public IInterval GetLeftWindow(IAircraftBundle bundle, List<IAircraftBundle> orderedBundles, int bundleIndex)
        {
            if (bundleIndex == 0)
            {
                return GetEmptyWindow(new Moment(0),
                    new Moment(bundle.FirstMoment.Value - AircraftMotionParameters.IntervalBetweenTakingOff));
            }
            else if (bundleIndex > 0)
            {
                var leftBundle = orderedBundles[bundleIndex - 1];
                return GetEmptyWindow(new Moment(leftBundle.LastMoment.Value + AircraftMotionParameters.IntervalBetweenTakingOff),
                    new Moment(bundle.FirstMoment.Value - AircraftMotionParameters.IntervalBetweenTakingOff));
            }

            return new Interval(new Moment(0), new Moment(0));
        }

        public IInterval GetRightWindow(IAircraftBundle bundle, List<IAircraftBundle> orderedLandingBundles, int bundleIndex)
        {
            if (orderedLandingBundles.Count <= bundleIndex + 1)
            {
                return GetEmptyWindow(new Moment(bundle.LastMoment.Value + AircraftMotionParameters.IntervalBetweenTakingOff),
                    new Moment(ModellingParameters.ModellingTime - AircraftMotionParameters.IntervalBetweenTakingOff));
            }
            else if (orderedLandingBundles.Count > bundleIndex + 1)
            {
                var rightBundle = orderedLandingBundles[bundleIndex + 1];
                return GetEmptyWindow(new Moment(bundle.LastMoment.Value + AircraftMotionParameters.IntervalBetweenTakingOff),
                    new Moment(rightBundle.FirstMoment.Value - AircraftMotionParameters.IntervalBetweenTakingOff));
            }

            return new Interval(new Moment(0), new Moment(0));
        }

        public IInterval GetCentralWindow(IAircraftBundle takingOffBundle, IntersectionCases intersectionCase, List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex)
        {
            if (intersectionCase == IntersectionCases.Right)
                return GetEmptyWindow(new Moment(orderedLandingBundles[intersectedBundleIndex - 1].
                    LastMoment.Value + AircraftMotionParameters.IntervalBetweenTakingOff),
                    new Moment(takingOffBundle.FirstMoment.Value - AircraftMotionParameters.IntervalBetweenTakingOff));
            if (intersectionCase == IntersectionCases.Left)
                return GetEmptyWindow(new Moment(takingOffBundle.LastMoment.Value +
                    AircraftMotionParameters.IntervalBetweenTakingOff),
                    new Moment(orderedLandingBundles[intersectedBundleIndex + 1].
                    FirstMoment.Value - AircraftMotionParameters.IntervalBetweenTakingOff));
            return null;
        }

        public IInterval GetEmptyWindow(IMoment firstMoment, IMoment secondMoment)
        {
            return new Interval(firstMoment, secondMoment);
        }

        public List<IInterval> GetEmptyWindowsForLeftCase(IAircraftBundle intersectedBundle, IAircraftBundle takingOffBundle, List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex, IntersectionCases intersectionCase)
        {
            var emptyWindows = new List<IInterval>();

            // Добавляем центральное окно
            emptyWindows.Add(GetCentralWindow(takingOffBundle, intersectionCase,
                orderedLandingBundles, intersectedBundleIndex));

            // Добавляем окно слева
            emptyWindows.Add(GetLeftWindow(intersectedBundle, orderedLandingBundles, intersectedBundleIndex));

            // Добавляем окно справа
            if (orderedLandingBundles.Count > intersectedBundleIndex + 1)
                emptyWindows.Add(GetRightWindow(orderedLandingBundles[intersectedBundleIndex + 1],
                    orderedLandingBundles, intersectedBundleIndex + 1));
            else
                emptyWindows.Add(GetRightWindow(intersectedBundle, orderedLandingBundles, intersectedBundleIndex));

            return emptyWindows;
        }

        public List<IInterval> GetEmptyWindowsForRightCase(IAircraftBundle intersectedBundle, IAircraftBundle takingOffBundle, List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex, IntersectionCases intersectionCase)
        {
            var emptyWindows = new List<IInterval>();

            if (intersectedBundleIndex == 0)
            {
                // Если слева больше нет садящихся пачек => добавляем окно вручную
                emptyWindows.Add(GetEmptyWindow(new Moment(0), takingOffBundle.FirstMoment));
            }
            else if (intersectedBundleIndex > 0)
            {
                // Добавляем цетнтральное окно
                emptyWindows.Add(GetCentralWindow(takingOffBundle, intersectionCase,
                    orderedLandingBundles, intersectedBundleIndex));

                // Добавляем окно слева
                emptyWindows.Add(GetLeftWindow(orderedLandingBundles[intersectedBundleIndex - 1],
                    orderedLandingBundles, intersectedBundleIndex - 1));
            }

            // Добавляем окно справа
            emptyWindows.Add(GetRightWindow(intersectedBundle, orderedLandingBundles, intersectedBundleIndex));

            return emptyWindows;
        }

        public List<IInterval> GetEmptyWindowsForOtherCases(IAircraftBundle intersectedBundle, List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex, IntersectionCases intersectionCase)
        {
            var emptyWindows = new List<IInterval>();

            emptyWindows.Add(GetLeftWindow(intersectedBundle, orderedLandingBundles, intersectedBundleIndex));

            // Добавляем окно справа
            if (intersectionCase == IntersectionCases.RightAndLeft)
                emptyWindows.Add(GetRightWindow(orderedLandingBundles[intersectedBundleIndex + 1], orderedLandingBundles,
                    intersectedBundleIndex + 1));
            else
                emptyWindows.Add(GetRightWindow(intersectedBundle, orderedLandingBundles, intersectedBundleIndex));

            return emptyWindows;
        }
    }
}
