using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class Interval : IInterval
    {
        public Interval(IMoment startMoment, IMoment endMoment, Intervals type = Intervals.Ordinary)
        {
            Type = type;
            FirstMoment = startMoment;
            LastMoment = endMoment;
        }

        public Intervals Type { get; set; }
        public IMoment FirstMoment { get; }
        public IMoment LastMoment { get; }
        public int GetIntervalDuration()
        {
            return LastMoment.Value - FirstMoment.Value;
        }

        public bool Intersects(IInterval interval)
        {
            return interval.LastMoment.Value > FirstMoment.Value && interval.FirstMoment.Value < LastMoment.Value;
        }

        public bool IsMomentInInterval(IMoment moment)
        {
            return moment.Value >= FirstMoment.Value && moment.Value <= LastMoment.Value;
        }
    }
}
