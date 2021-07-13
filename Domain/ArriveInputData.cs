using OptimalMotion2.Domain.Interfaces;

namespace OptimalMotion2.Domain
{
    public class ArriveInputData : IArriveInputData
    {
        public ArriveInputData(IAircraftBundle aircraftBundle, int emptyWindowInterval = 0)
        {
            AircraftBundle = aircraftBundle;
            EmptyWindowInterval = emptyWindowInterval;
        }

        public IAircraftBundle AircraftBundle { get; }
        public int EmptyWindowInterval { get; }
    }
}
