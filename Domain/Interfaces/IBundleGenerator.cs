using System.Collections.Generic;

namespace OptimalMotion2.Domain.Interfaces
{
    public interface IBundleGenerator
    {
        IAircraftBundle GetTakingOffBundle(IRunway runway, ISpecPlatform specPlatform);
        IAircraftBundle GetLandingBundle(List<IMoment> moments, int runwayIndex = 0);
        IBundleGenerator GetInstance(IAircraftGenerator aircraftGenerator);
        void Reset();
    }
}