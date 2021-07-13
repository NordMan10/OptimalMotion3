

using System.ComponentModel;

namespace OptimalMotion2.Domain
{
    public class TableRow : ITableRow
    {
        public TableRow(TableRowData data)
        {
            AircraftId = data.AircraftId;
            TakingOffMoment = data.TakingOffMoment;
            NominalEngineStartMoment = data.NominalEngineStartMoment;
            ActualEngineStartMoment = data.ActualEngineStartMoment;
            NeedProcessing = data.NeedProcessing;
        }

        [DisplayName("Id ВС")]
        public string AircraftId { get; }

        [DisplayName("Твзлет")]
        public string TakingOffMoment { get; }

        [DisplayName("Тном. зап. двиг.")]
        public string NominalEngineStartMoment { get; }

        [DisplayName("Тфакт. зап. двиг.")]
        public string ActualEngineStartMoment { get; }

        [DisplayName("Обработка")]
        public bool NeedProcessing { get; }
    }
}
