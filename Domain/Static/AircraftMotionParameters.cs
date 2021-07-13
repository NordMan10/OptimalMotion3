using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain.Static
{
    public static class AircraftMotionParameters
    {
        public const int MotionFromParkingToPS = 180;
        public const int MotionFromParkingToSP = 180;
        public const int MotionFromSPToPS = 180;
        public const int MotionFromPSToES = 30;
        public const int MotionFromLandingToPS = 60;
        public const int TakingOffInterval = 40; // Время взлета каждого ВС пачки (движение от точки исполнительного старта до освобождения ВПП)
        public const int SafeMotionInterval = 90;
        public const int LandingInterval = 60; // Время движения от точки посадки (ИСПСТ) до освобождния ВПП
        public const int IntervalBetweenTakingOff = 60;
    }
}
