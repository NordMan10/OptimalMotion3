using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OptimalMotion2.Domain.Static
{
    public static class ModellingParameters
    {
        public const int ModellingTime = 3600; // время в секундах
        public const int TakingOffBundleAircraftCount = 4;
        public static Interval FirstAircraftModellingInterval = new Interval(0, 2880); // время в секундах

    }

    public readonly struct Interval
    {
        public Interval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}

    //private void SetAircraftMomentsWithProcessing(List<ITakingOffAircraft> orderedAircrafts, int aircraftIndex)
    //{
    //    orderedAircrafts[aircraftIndex].Moments.ArriveToES =
    //            new Moment(orderedAircrafts[aircraftIndex].OrderMoment.Value - AircraftMotionParameters.TakingOffInterval);
    //    orderedAircrafts[aircraftIndex].Moments.ArriveToPS =
    //        new Moment(orderedAircrafts[aircraftIndex].Moments.ArriveToES.Value - AircraftMotionParameters.MotionFromPSToES);

    //    if (aircraftIndex == 0)
    //    {
    //        orderedAircrafts[aircraftIndex].Moments.EngineStart =
    //        new Moment(orderedAircrafts[aircraftIndex].Moments.ArriveToPS.Value - AircraftMotionParameters.MotionFromSPToPS -
    //        SpecPlatformParameters.ProcessingInterval - AircraftMotionParameters.MotionFromParkingToSP);
    //    }
    //    else
    //    {
    //        orderedAircrafts[aircraftIndex].Moments.EngineStart =
    //            new Moment(orderedAircrafts[aircraftIndex - 1].Moments.EngineStart.Value +
    //            SpecPlatformParameters.ProcessingInterval);

    //    }
