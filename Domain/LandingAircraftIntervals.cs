

namespace OptimalMotion2.Domain
{
    public class LandingAircraftIntervals
    {
        public LandingAircraftIntervals(IInterval landing)
        {
            Landing = landing;
        }

        public IInterval Landing { get; }
    }
}
