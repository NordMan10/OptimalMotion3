

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class LandingAircraftData : ILandingAircraftData
    {
        public LandingAircraftData(IAircraftId id, int runwayIndex,
            LandingAircraftMoments moments, LandingAircraftIntervals intervals,  AircraftType type = AircraftType.Medium)
        {
            Id = id;
            RunwayId = runwayIndex;
            Moments = moments;
            Intervals = intervals;
        }

        public IAircraftId Id { get; }
        public int RunwayId { get; }
        public LandingAircraftMoments Moments { get; }
        public LandingAircraftIntervals Intervals { get; }

        public AircraftType Type => throw new System.NotImplementedException();
    }
}
