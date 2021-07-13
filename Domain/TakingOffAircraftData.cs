

using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraftData : ITakingOffAircraftData
    {
        public TakingOffAircraftData(IAircraftId id, AircraftType type, TakingOffAircraftMoments moments,
            TakingOffAircraftIntervals intervals, IRunway runway,
            ISpecPlatform specPlatform, bool processingIsNeeded)
        {
            Id = id;
            Type = type;
            Moments = moments;
            Intervals = intervals;
            Runway = runway;
            SpecPlatform = specPlatform;
            ProcessingIsNeeded = processingIsNeeded;
        }

        public IAircraftId Id { get; }
        public TakingOffAircraftMoments Moments { get; }
        public TakingOffAircraftIntervals Intervals { get; }
        public IRunway Runway { get; }
        public ISpecPlatform SpecPlatform { get; }
        public bool ProcessingIsNeeded { get; }

        public AircraftType Type { get; }
    }
}
