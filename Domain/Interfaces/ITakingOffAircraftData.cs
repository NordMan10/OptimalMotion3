

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface ITakingOffAircraftData
    {
        int Id { get; }
        AircraftType Type { get; }
        TakingOffAircraftMoments Moments { get; }
        TakingOffAircraftIntervals Intervals { get; } 
        int RunwayId { get; }
        int SpecPlatformId { get; } 
        int ParkingId { get; }
        bool ProcessingIsNeeded { get; }
    }
}
