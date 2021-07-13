using OptimalMotion2.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public interface ITakingOffAircraft : IAircraft
    {
        bool ProcessingIsNeeded { get; }
        TakingOffAircraftMoments Moments { get; }
        TakingOffAircraftIntervals Intervals { get; }

        void SetAircraftMoments(int aircraftIndex, IAircraftBundle takingOffBundle);
        void SetAircraftMomentsWithProcessing(int aircraftIndex, IInterval differenceInterval);
        void SetAircraftMomentsWithoutProcessing();
    }
}
