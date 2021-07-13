using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain.Interfaces
{
    public interface IArriveInputData
    {
        IAircraftBundle AircraftBundle { get; }
        int EmptyWindowInterval { get; }
    }
}
