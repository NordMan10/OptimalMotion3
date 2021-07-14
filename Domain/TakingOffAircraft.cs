using System;
using System.Collections.Generic;
using System.Windows.Markup;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Domain.Static;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public class TakingOffAircraft : ITakingOffAircraft
    {
        public TakingOffAircraft(ITakingOffAircraftData data)
        {
            Id = data.Id;
            Type = data.Type;
            Moments = data.Moments;
            OrderMoment = Moments.TakingOff;
            Intervals = data.Intervals;
            ProcessingIsNeeded = data.ProcessingIsNeeded;
            runway = data.Runway;
            specPlatform = data.SpecPlatform;
        }

        private readonly IRunway runway;
        private readonly ISpecPlatform specPlatform;


        public IAircraftId Id { get; set; }
        public TakingOffAircraftMoments Moments { get; }
        public TakingOffAircraftIntervals Intervals { get; }
        public bool ProcessingIsNeeded { get; }
        public IMoment OrderMoment { get; set; }
        public AircraftType Type { get; }
        

        public int GetRunwayId()
        {
            return runway.Id;
        }

        public int GetSpecPlatformId()
        {
            return specPlatform.Id;
        }

        public IInterval GetRunwayOccupationInterval()
        {
            throw new NotImplementedException();
        }

        public bool Equals(ITakingOffAircraft other)
        {
            return Id.Id == other.Id.Id;
        }

        public void SetAircraftMoments(int aircraftIndex, IAircraftBundle takingOffBundle)
        {
            if (ProcessingIsNeeded)
            {
                var differenceInterval = takingOffBundle.GetLastAircraftDelay();
                SetAircraftMomentsWithProcessing(aircraftIndex, differenceInterval);
            }
            else
            {
                SetAircraftMomentsWithoutProcessing();
            }
        }

        public void SetAircraftMomentsWithProcessing(int aircraftIndex, IInterval differenceInterval)
        {
            // 4. По записанной схеме рассчитываем все моменты для каждого ВС, прибавляя к ним полученную разницу
            // Таким образом, мы задаем моменты запуска двигателей с учетом ожидания обработки всех ВС. ВС будут приходить на ПРСТ заранее.

            Moments.ArriveToES =
                new Moment(OrderMoment.Value - AircraftMotionParameters.TakingOffInterval -
                differenceInterval.GetIntervalDuration());

            Moments.ArriveToPS =
                new Moment(Moments.ArriveToES.Value - AircraftMotionParameters.MotionFromPSToES);
            Moments.EngineStart =
                new Moment(Moments.ArriveToPS.Value - AircraftMotionParameters.MotionFromSPToPS -
                SpecPlatformParameters.ProcessingInterval - AircraftMotionParameters.MotionFromParkingToSP +
                (SpecPlatformParameters.ProcessingInterval * aircraftIndex));
        }

        public void SetAircraftMomentsWithoutProcessing()
        {
            Moments.ArriveToES =
                new Moment(OrderMoment.Value - AircraftMotionParameters.TakingOffInterval);
            Moments.ArriveToPS =
                new Moment(Moments.ArriveToES.Value - AircraftMotionParameters.MotionFromPSToES);
            Moments.EngineStart =
                new Moment(Moments.ArriveToPS.Value - AircraftMotionParameters.MotionFromParkingToPS);

            
        }


    }
}
