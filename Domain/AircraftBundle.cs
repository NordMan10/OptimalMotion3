using System;
using System.Collections.Generic;
using System.Linq;
using OptimalMotion2.Domain.Interfaces;
using OptimalMotion2.Domain.Static;
using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    class AircraftBundle : IAircraftBundle
    {
        public AircraftBundle(IEnumerable<IAircraft> aircrafts)
        {
            foreach (var aircraft in aircrafts)
            {
                Aircrafts.Add(aircraft);
            }
            Id = aircrafts.ToList().First().Id.Id;
        }

        public int Id { get; }

        private List<IAircraft> Aircrafts { get; } = new List<IAircraft>();

        public int Count => Aircrafts.Count;

        public IMoment FirstMoment
        {
            get
            {
                var firstAircraft = Aircrafts
                    .OrderBy(a => a.OrderMoment.Value).First();
                return new Moment(firstAircraft.OrderMoment.Value);
            }
        }
        
        public IMoment LastMoment
        {
            get
            {
                var lastAircraft = Aircrafts
                    .OrderBy(a => a.OrderMoment.Value).Last();
                return new Moment(lastAircraft.OrderMoment.Value);
            }
        }
        public void AddAircraft(IAircraft aircraft)
        {
            Aircrafts.Add(aircraft);
        }

        public IAircraft RemoveAircraft(IAircraftId id)
        {
            var removedAircraft = Aircrafts.Find(a => a.Id.Id == id.Id);
            if (removedAircraft != null)
                Aircrafts.Remove(removedAircraft);
            else
                throw new ArgumentException("There is no specified aircraft(me)!");

            return removedAircraft;
        }

        public IAircraft GetAircraft(IAircraftId id)
        {
            var aircraft = Aircrafts.Find(a => a.Id.Id == id.Id);
            if (aircraft == null)
                throw new ArgumentException("There is no specified aircraft(me)!");
            
            return aircraft;
        }

        /// <summary>
        /// Hey hey hey
        /// </summary>
        /// <returns></returns>
        public List<IAircraft> GetAircrafts()
        {
            return Aircrafts;
        }

        /// <summary>
        /// Возвращает задержку для последнего ВС (передается в параметре), которую необходимо учесть при расчете 
        /// момента запуска двигателей для всех остальных ВС
        /// </summary>
        /// <param name="aircraft">Последний ВС в вылетающей пачке</param>
        /// <returns></returns>
        public IInterval GetLastAircraftDelay()
        {
            var lastAircraft = Aircrafts.OrderBy(a => a.OrderMoment).Last();

            // 1. Рассчитываем максимально допустимый момент запуска двигателей для последнего ВС
            var maxAcceptabaleEngineStartMoment = new Moment(lastAircraft.OrderMoment.Value -
                AircraftMotionParameters.TakingOffInterval - AircraftMotionParameters.MotionFromPSToES -
                AircraftMotionParameters.MotionFromSPToPS - SpecPlatformParameters.ProcessingInterval -
                AircraftMotionParameters.MotionFromParkingToSP);

            // 2. Рассчитываем момент запуска двигателей последнего ВС с учетом ожидания всех остальных ВС
            var engineStartMomentWithDelay = new Moment(maxAcceptabaleEngineStartMoment.Value +
                (ModellingParameters.TakingOffBundleAircraftCount - 1) * SpecPlatformParameters.ProcessingInterval);

            // 3. Рассчитываем разницу
            var differenceInterval = new Interval(maxAcceptabaleEngineStartMoment, engineStartMomentWithDelay);

            return differenceInterval;
        }

        public Dictionary<IAircraftId, IMoment> GetNominalEngineStartMoments()
        {
            var orderedTakingOffAircrafts = GetAircrafts().OrderBy(a => a.OrderMoment.Value).ToList();
            var nominalEngineStartMoments = new Dictionary<IAircraftId, IMoment>();

            
            for (var i = 0; i < orderedTakingOffAircrafts.Count; i++)
            {
                var takingOffAircraft = (TakingOffAircraft)orderedTakingOffAircrafts[i];
                nominalEngineStartMoments.Add(takingOffAircraft.Id, new Moment(takingOffAircraft.OrderMoment.Value));
            }

            return nominalEngineStartMoments;
        }
    }
}
