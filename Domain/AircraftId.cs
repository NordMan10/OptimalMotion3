using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public class AircraftId : IAircraftId
    {
        public AircraftId(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
