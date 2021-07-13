using System;
using System.Collections.Generic;
using System.Linq;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class Runway : IRunway
    {
        public Runway(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public List<IAircraftBundle> LandingBundles { get; } = new List<IAircraftBundle>();
        public List<IAircraftBundle> TakingOffBundles { get; } = new List<IAircraftBundle>();
        public List<IAircraftBundle> OutdatedBundles { get; } = new List<IAircraftBundle>();

        public void AddLandingBundle(IAircraftBundle bundle)
        {
            LandingBundles.Add(bundle);
        }

        public void AddTakingOffBundle(IAircraftBundle bundle)
        {
            TakingOffBundles.Add(bundle);
        }

        public void AddOutdatedBundle(IAircraftBundle bundle)
        {
            OutdatedBundles.Add(bundle);
        }

        public IAircraftBundle GetIntersectedBundleAndSetCase(IAircraftBundle departureBundle, ref IntersectionCases intersectionCase)
        {
            var orderedLandingBundles = LandingBundles
                .OrderBy(bundle => bundle.FirstMoment.Value).ToList();

            IAircraftBundle intersectedBundle = null;


            for (var i = 0; i < orderedLandingBundles.Count; i++)
            {
                // Проверяем случай пересечения Right
                if (CheckRightIntersection(departureBundle, orderedLandingBundles[i]))
                {
                    intersectionCase = IntersectionCases.Right;
                    intersectedBundle = orderedLandingBundles[i];
                    // Проверяем случай пересечения RightAndLeft
                    if (i > 0 && CheckLeftIntersection(departureBundle, orderedLandingBundles[i - 1]))
                    {
                        intersectionCase = IntersectionCases.RightAndLeft;
                        intersectedBundle = orderedLandingBundles[i - 1];
                    }
                    break;
                }
                // Проверяем случай пересечения Left
                else if (CheckLeftIntersection(departureBundle, orderedLandingBundles[i]))
                {
                    intersectionCase = IntersectionCases.Left;
                    intersectedBundle = orderedLandingBundles[i];
                    // Проверяем случай пересечения RightAndLeft
                    if (i < orderedLandingBundles.Count - 1 && CheckRightIntersection(departureBundle, orderedLandingBundles[i + 1]))
                    {
                        intersectionCase = IntersectionCases.RightAndLeft;
                        intersectedBundle = orderedLandingBundles[i];
                    }
                    break;
                }
                // Проверяем случай пересечения Middle
                else if (departureBundle.LastMoment.Value <= orderedLandingBundles[i].LastMoment.Value &&
                         departureBundle.FirstMoment.Value >= orderedLandingBundles[i].FirstMoment.Value)
                {
                    intersectionCase = IntersectionCases.Middle;
                    intersectedBundle = orderedLandingBundles[i];
                    break;
                }
                // Проверяем случай пересечения Out
                else if (departureBundle.LastMoment.Value > orderedLandingBundles[i].LastMoment.Value &&
                         departureBundle.FirstMoment.Value < orderedLandingBundles[i].FirstMoment.Value)
                {
                    intersectionCase = IntersectionCases.Out;
                    intersectedBundle = orderedLandingBundles[i];
                    break;
                }
                else
                    intersectionCase = IntersectionCases.Init;
            }

            return intersectedBundle;
        }

        private bool CheckRightIntersection(IAircraftBundle takingOffBundle, IAircraftBundle savedBundle)
        {
            return takingOffBundle.LastMoment.Value >= savedBundle.FirstMoment.Value &&
                   takingOffBundle.LastMoment.Value <= savedBundle.LastMoment.Value &&
                   takingOffBundle.FirstMoment.Value < savedBundle.FirstMoment.Value;

        }

        private bool CheckLeftIntersection(IAircraftBundle takingOffBundle, IAircraftBundle savedBundle)
        {
            return takingOffBundle.LastMoment.Value > savedBundle.LastMoment.Value &&
                   takingOffBundle.FirstMoment.Value >= savedBundle.FirstMoment.Value &&
                   takingOffBundle.FirstMoment.Value <= savedBundle.LastMoment.Value;
        }

        public List<Tuple<IAircraftBundle, IntersectionCases, int>> CheckIntersections(IAircraftBundle departureBundle)
        {
            throw new NotImplementedException();
        }
    }
}
