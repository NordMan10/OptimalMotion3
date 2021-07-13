using System.Collections.Generic;
using OptimalMotion2.Domain;

namespace OptimalMotion2.Domain.Interfaces
{
    public interface IAircraftBundle
    {
        int Id { get; }
        int Count { get; }
        IMoment FirstMoment { get; }
        IMoment LastMoment { get; }
        void AddAircraft(IAircraft aircraft);
        IAircraft RemoveAircraft(IAircraftId id);
        IAircraft GetAircraft(IAircraftId id);
        
        List<IAircraft> GetAircrafts();

        /// <summary>
        /// Возвращает задержку для последнего ВС, которую необходимо учесть при расчете 
        /// момента запуска двигателей для всех остальных ВС
        /// </summary>
        /// <returns></returns>
        IInterval GetLastAircraftDelay();
        Dictionary<IAircraftId, IMoment> GetNominalEngineStartMoments();
    }
}
