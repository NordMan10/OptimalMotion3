using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public interface IAircraftGenerator
    {
        IAircraftGenerator GetInstance(IAircraftIdGenerator idGenerator);
        ITakingOffAircraft GetTakingOffAircraft(IMoment TakingOffMoment, IRunway runway, ISpecPlatform specPlatform);
        ILandingAircraft GetLandingAircraft(LandingAircraftMoments moments, 
            LandingAircraftIntervals intervals, int runwayIndex = 0);
    }
}
