using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public static class Convertation
    {
        public static int GetMinutesFromSeconds(int seconds)
        {
            return (int)Math.Round((double)seconds / 60, MidpointRounding.AwayFromZero);
        }
    }
}
