

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface ITakingOffAircraftData
    {
        IAircraftId Id { get; }
        AircraftType Type { get; }
        TakingOffAircraftMoments Moments { get; }
        TakingOffAircraftIntervals Intervals { get; } 
        IRunway Runway { get; }
        ISpecPlatform SpecPlatform { get; } 
        bool ProcessingIsNeeded { get; }
    }
}
