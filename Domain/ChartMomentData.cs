using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class ChartMomentData : IChartMomentData
    {
        public ChartMomentData(IAircraftId aircraftId, IMoment moment, AircraftBehavior type, ChartMomentDataType subType)
        {
            AircraftId = aircraftId;
            Moment = moment;
            Type = type;
            SubType = subType;
        }

        public IAircraftId AircraftId { get; }
        public IMoment Moment { get; }
        public AircraftBehavior Type { get; }
        public ChartMomentDataType SubType { get; }

    }
}
