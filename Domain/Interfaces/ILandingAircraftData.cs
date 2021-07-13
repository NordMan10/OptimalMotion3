

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface ILandingAircraftData
    {
        IAircraftId Id { get; }
        int RunwayId { get; }
        AircraftType Type { get; }
        LandingAircraftMoments Moments { get; } 
        LandingAircraftIntervals Intervals { get; }
    }
}
