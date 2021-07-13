using System.Collections.Generic;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraftMoments
    {
        /// <summary>
        /// Warning! Check value of moments! It may be null
        /// </summary>
        /// <param name="creationMoment"></param>
        /// <param name="engineStartMoment"></param>
        /// <param name="arriveToPSMoment"></param>
        /// <param name="arriveToESMoment"></param>
        public TakingOffAircraftMoments(IMoment plannedTakingOff)
        {
            PlannedTakingOff = plannedTakingOff;
        }
        /// <summary>
        /// Плановый момент вылета
        /// </summary>
        public IMoment PlannedTakingOff { get; }

        public IMoment TakingOff { get; }
        /// <summary>
        /// May be null!
        /// </summary>
        public IMoment EngineStart { get; set; }
        /// <summary>
        /// May be null!
        /// </summary>
        //public IMoment ArriveToPS { get; set; }
        /// <summary>
        /// May be null!
        /// </summary>
       //public IMoment ArriveToES { get; set; }
    }
}
