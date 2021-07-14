using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain.Static
{
    public static class TakingOffAircraftsData
    {
        public static List<TakingOffAircraftData> Data = new List<TakingOffAircraftData>
        {
            new TakingOffAircraftData(1, Enums.AircraftType.Medium, new TakingOffAircraftMoments(new Moment(600)),
                new TakingOffAircraftIntervals(40, 30, 180, 120, 240, 120), 1, 1, 1, false),

            new TakingOffAircraftData(2, Enums.AircraftType.Medium, new TakingOffAircraftMoments(new Moment(780)),
                new TakingOffAircraftIntervals(40, 30, 180, 120, 240, 120), 1, 1, 1, true),

            new TakingOffAircraftData(3, Enums.AircraftType.Medium, new TakingOffAircraftMoments(new Moment(900)),
                new TakingOffAircraftIntervals(40, 30, 180, 120, 240, 120), 1, 1, 2, false),

            new TakingOffAircraftData(4, Enums.AircraftType.Medium, new TakingOffAircraftMoments(new Moment(1080)),
                new TakingOffAircraftIntervals(40, 30, 180, 120, 240, 90), 1, 2, 1, true),

            new TakingOffAircraftData(5, Enums.AircraftType.Medium, new TakingOffAircraftMoments(new Moment(1200)),
                new TakingOffAircraftIntervals(40, 30, 240, 60, 240, 120), 2, 1, 1, true)
        };
    }
}
