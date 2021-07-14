

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraftData : ITakingOffAircraftData
    {
        public TakingOffAircraftData(int id, AircraftType type, TakingOffAircraftMoments moments,
            TakingOffAircraftIntervals intervals, int runwayId,
            int specPlatformId, int parkingId, bool processingIsNeeded)
        {
            Id = id;
            Type = type;
            Moments = moments;
            Intervals = intervals;
            RunwayId = runwayId;
            SpecPlatformId = specPlatformId;
            ParkingId = parkingId;
            ProcessingIsNeeded = processingIsNeeded;
        }

        public int Id { get; }
        public TakingOffAircraftMoments Moments { get; }
        public TakingOffAircraftIntervals Intervals { get; }
        public bool ProcessingIsNeeded { get; }

        public AircraftType Type { get; }

        public int RunwayId { get; }

        public int SpecPlatformId { get; }

        public int ParkingId { get; }
    }
}
