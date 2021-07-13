﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public class SpecPlatform : ISpecPlatform
    {
        public SpecPlatform(int id)
        {
            Id = id;
            OccupationIntervals = new Dictionary<IMoment, IMoment>();
        }

        public int Id { get; }
        public Dictionary<IMoment, IMoment> OccupationIntervals { get; }

        /// <summary>
        /// Метод расчета задержки для безопасного слияния
        /// </summary>
        /// <param name="leftIntervalParam"></param>
        /// <param name="rightIntervalParam"></param>
        /// <param name="safeMergeValueParam"></param>
        /// <returns></returns>
        private int GetSafeMergeDelay(IInterval leftIntervalParam, IInterval rightIntervalParam, int safeMergeValueParam)
        {
            // Принимаем два интервала и значение интервала для безопасного слияния  
            var leftInterval = leftIntervalParam;
            var rightInterval = rightIntervalParam;
            var safeMergeValue = safeMergeValueParam;

            // Вычисляем модуль разности между начальными моментами этих интервалов;
            var startMomentsDifference = Math.Abs(leftInterval.FirstMoment.Value - rightInterval.FirstMoment.Value);

            // Если полученная разность >= интервалу для безопасного слияния => возвращаем ноль;
            if (startMomentsDifference >= safeMergeValue)
                return 0;

            // Если нет => возвращаем интервал для безопасного слияния = значение
            // интервала для безопасного слияния - рассчитанная разность;
            return safeMergeValue - startMomentsDifference;
        }

        /// <summary>
        /// Метод сдвига интервала
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        private IInterval ShiftInterval(IInterval intervalParam, int shiftParam)
        {
            // 1) Принимаем интервал и значение сдвига;
            var interval = intervalParam;
            var shift = shiftParam;

            // 2) Увеличиваем значение начального и конечного момента на переданное значение;
            var shiftedInterval = new Interval(new Moment(interval.FirstMoment.Value + shift),
                new Moment(interval.LastMoment.Value + shift), interval.Type);

            // 3) Возвращаем новый интервал;
            return shiftedInterval;
        }

        /// <summary>
        /// Метод, возвращающий задержки при добавлении нового судна в конец очереди
        /// </summary>
        /// <param name="aircraftInterval"></param>
        /// <param name="safeMergeValueParam"></param>
        /// <returns></returns>
        private Tuple<int, int> GetDelaysForNewLastAircraft(IInterval aircraftInterval, int safeMergeValueParam)
        {
            // Принимаем интервал обратившегося судна;
            var currentInterval = aircraftInterval;
            var safeMergeValue = safeMergeValueParam;

            // Получаем начальный момент последнего записанного судна
            var lastWrittenStartMoment = OccupationIntervals.Keys.OrderBy(key => key).Last();
            // Получаем конечный момент последнего записанного судна
            var lastWrittenEndMoment = OccupationIntervals[lastWrittenStartMoment];

            // Сохраняем интервал ожидания обработки = момент покидания площадки последним записанным судном 
            // минус момент прибытия (без задержки) обратившегося судна;
            var processingDelay = lastWrittenEndMoment.Value - currentInterval.FirstMoment.Value;

            // Сдвигаем текущий интервал на полученную задержку
            var shiftedCurrentInterval = ShiftInterval(currentInterval, processingDelay);

            // Получаем задержку для соблюдения интервала безопасного слияния
            var safeMergeDelay = GetSafeMergeDelay(new Interval(lastWrittenStartMoment, lastWrittenEndMoment),
                shiftedCurrentInterval, safeMergeValue);

            // 4) Возвращаем в кортеже задержку для ожидания обработки и задержку для безопасного слияния;
            return Tuple.Create(processingDelay, safeMergeDelay);
        }
    }
}
