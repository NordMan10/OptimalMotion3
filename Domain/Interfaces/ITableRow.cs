

namespace OptimalMotion2.Domain
{
    public interface ITableRow
    {
        string AircraftId { get; }

        string TakingOffMoment { get; }

        string NominalEngineStartMoment { get; }

        string ActualEngineStartMoment { get; }

        bool NeedProcessing { get; }
    }
}
