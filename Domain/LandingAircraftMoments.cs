

namespace OptimalMotion2.Domain
{
    public class LandingAircraftMoments
    {
        public LandingAircraftMoments(IMoment landing)
        {
            Landing = landing;
        }

        public IMoment Landing { get; }

    }
}
