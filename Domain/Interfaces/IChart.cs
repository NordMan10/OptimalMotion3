using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public interface IChart
    {
        void AddMoment(IChartMomentData rowCreationData);
        void RemoveMoment(IAircraftId id);
        void MoveMoment(IAircraftId id, IChartMomentData newMoment);
        void Reset();
    }
}
