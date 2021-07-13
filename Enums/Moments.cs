using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Enums
{
    public enum Moments
    {
        Appearance, // момент создания ВС
        SpecPlatformLeave, // момент покидания Спец площадки
        EngineStart, // момент запуска двигателей
        PlannedPreliminaryStartArrival, // плановый момент выхода на ПРСТ
        Departure, // видимо, момент покидания участка воздушного пространства аэропорта (пока не знаю зачем)
        PreliminaryStartArrival, // реальный момент выхода на ПРСТ
        ExecutiveStartArrival, // момент выхода на ИСПСТ (исполнительный старт)
        TakeOff, // момент взлета
        Landing // момент посадки
    }
}
