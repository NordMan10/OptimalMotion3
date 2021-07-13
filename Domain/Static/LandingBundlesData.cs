using System.Collections.Generic;

namespace OptimalMotion2.Domain.Static
{
    public static class LandingBundlesData
    {
        
        public static List<List<IMoment>> BundlesData = new List<List<IMoment>>
        {
            new List<IMoment>
                {new Moment(0), new Moment(180), new Moment(360), new Moment(540), new Moment(720), new Moment(900)},
            new List<IMoment>
                {new Moment(1500), new Moment(1680), new Moment(1860), new Moment(2040), new Moment(2220), new Moment(2400)},
            new List<IMoment>
                {new Moment(2880), new Moment(3060), new Moment(3240), new Moment(3420)}

        };
    }
}