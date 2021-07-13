using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface IInterval
    {
        Intervals Type { get; set; }
        IMoment FirstMoment { get; }
        IMoment LastMoment { get; }

        int GetIntervalDuration();
        bool Intersects(IInterval interval);

        /// <summary>
        /// Возвращает true или false, в зависимости от того, попадает ли переданный момент в переданный интервал
        /// </summary>
        /// <param name="moment"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        bool IsMomentInInterval(IMoment moment);
    }
}
