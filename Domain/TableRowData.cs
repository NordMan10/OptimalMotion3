using System.ComponentModel;

namespace OptimalMotion2.Domain
{
    public class TableRowData
    {
        public TableRowData(string aircraftId, string takingOffMoment, string nominalEngineStartMoment,
            string actualEngineStartMoment, bool needProcessing)
        {
            AircraftId = aircraftId;
            TakingOffMoment = takingOffMoment;
            NominalEngineStartMoment = nominalEngineStartMoment;
            ActualEngineStartMoment = actualEngineStartMoment;
            NeedProcessing = needProcessing;
        }

        public string AircraftId { get; }

        public string TakingOffMoment { get; }

        public string NominalEngineStartMoment { get; }

        public string ActualEngineStartMoment { get; }

        public bool NeedProcessing { get; }
    }
}
