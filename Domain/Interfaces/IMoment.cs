using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface IMoment : IComparable<IMoment>
    {
        int Value { get; }
        Moments Type { get; set; }
    }
}
