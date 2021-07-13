

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraftIntervals
    {
        public TakingOffAircraftIntervals(
            IInterval motionFromParkingToPS, IInterval motionFromPSToES, IInterval takeoff,
            IInterval motionFromParkingToSP, IInterval motionFromSPToPS, IInterval processing, IInterval minToPass)
        {
            MotionFromParkingToPS = motionFromParkingToPS;
            MotionFromPSToES = motionFromPSToES;
            TakingOff = takeoff;
            MotionFromParkingToSP = motionFromParkingToSP;
            MotionFromSPToPS = motionFromSPToPS;
            Processing = processing;
            MinToPass = minToPass;
        }

        public IInterval MotionFromParkingToPS { get; }
        public IInterval MotionFromPSToES { get; }
        public IInterval TakingOff { get; }
        public IInterval Processing { get; }
        public IInterval MotionFromParkingToSP { get; }
        public IInterval MotionFromSPToPS { get; }
        /// <summary>
        /// Расчетный, минимально необходимый для пропуска садящегося ВС, интервал ожидания на ПРДВ
        /// </summary>
        public IInterval MinToPass { get; }

    }
}
