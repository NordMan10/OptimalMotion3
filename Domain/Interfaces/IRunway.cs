using System;
using System.Collections.Generic;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface IRunway
    {
        int Id { get; }
        List<IAircraftBundle> LandingBundles { get; }
        List<IAircraftBundle> TakingOffBundles { get; }
        List<IAircraftBundle> OutdatedBundles { get; }
        List<Tuple<IAircraftBundle, IntersectionCases, int>> CheckIntersections(IAircraftBundle departureBundle);
        IAircraftBundle GetIntersectedBundleAndSetCase(IAircraftBundle departureBundle, ref IntersectionCases intersectionCase);
        void AddLandingBundle(IAircraftBundle bundle);
        void AddTakingOffBundle(IAircraftBundle bundle);
        void AddOutdatedBundle(IAircraftBundle bundle);
    }
}
