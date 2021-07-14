

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraftIntervals
    {
        public TakingOffAircraftIntervals(

            int takingOff, int motionFromPSToES, int motionFromParkingToPS,
            int motionFromSPToPS, int processing, int motionFromParkingToSP)
        {
            MotionFromParkingToPS = motionFromParkingToPS;
            MotionFromPSToES = motionFromPSToES;
            TakingOff = takingOff;
            MotionFromParkingToSP = motionFromParkingToSP;
            MotionFromSPToPS = motionFromSPToPS;
            Processing = processing;
            //MinToPass = minToPass;
        }

        public int MotionFromParkingToPS { get; }
        public int MotionFromPSToES { get; }
        public int TakingOff { get; }
        public int Processing { get; }
        public int MotionFromParkingToSP { get; }
        public int MotionFromSPToPS { get; }
        /// <summary>
        /// Расчетный, минимально необходимый для пропуска садящегося ВС, интервал ожидания на ПРДВ
        /// </summary>
        //public int MinToPass { get; }

    }
}
