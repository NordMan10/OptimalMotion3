using OptimalMotion2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain.Interfaces
{
    public interface IAircraftBundleConflictResolver
    {
        /// <summary>
        /// Возвращает окно слева от переданной пачки относительно множества переданных пачек
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="orderedBundles">Множество переданных пачек</param>
        /// <param name="bundleIndex">Индекс пачки в переданном множестве</param>
        /// <returns></returns>
        IInterval GetLeftWindow(IAircraftBundle bundle, List<IAircraftBundle> orderedBundles, int bundleIndex);

        /// <summary>
        /// Возвращает окно справа от переданной пачки
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="orderedLandingBundles"></param>
        /// <param name="bundleIndex"></param>
        /// <returns></returns>
        IInterval GetRightWindow(IAircraftBundle bundle, List<IAircraftBundle> orderedLandingBundles, int bundleIndex);

        /// <summary>
        /// Возвращает интервал, созданный из переданных моментов
        /// </summary>
        /// <param name="firstMoment"></param>
        /// <param name="secondMoment"></param>
        /// <returns></returns>
        IInterval GetEmptyWindow(IMoment firstMoment, IMoment secondMoment);

        /// <summary>
        /// Возвращает интервал, обозначающий центральное окно, которое существует при пересечениях справа и слева
        /// </summary>
        /// <param name="takingOffBundle"></param>
        /// <param name="intersectionCase"></param>
        /// <param name="orderedLandingBundles"></param>
        /// <param name="intersectedBundleIndex"></param>
        /// <returns></returns>
        IInterval GetCentralWindow(IAircraftBundle takingOffBundle, IntersectionCases intersectionCase,
            List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex);

        /// <summary>
        /// Возвращает окна для случая пересечения справа
        /// </summary>
        /// <param name="intersectedBundle"></param>
        /// <param name="takingOffBundle"></param>
        /// <param name="orderedLandingBundles"></param>
        /// <param name="intersectedBundleIndex"></param>
        /// <param name="intersectionCase"></param>
        /// <returns></returns>
        List<IInterval> GetEmptyWindowsForRightCase(IAircraftBundle intersectedBundle,
            IAircraftBundle takingOffBundle, List<IAircraftBundle> orderedLandingBundles,
            int intersectedBundleIndex, IntersectionCases intersectionCase);

        /// <summary>
        /// Возвращает окна для случая пересечения слева
        /// </summary>
        /// <param name="intersectedBundle"></param>
        /// <param name="takingOffBundle"></param>
        /// <param name="orderedLandingBundles"></param>
        /// <param name="intersectedBundleIndex"></param>
        /// <param name="intersectionCase"></param>
        /// <returns></returns>
        List<IInterval> GetEmptyWindowsForLeftCase(IAircraftBundle intersectedBundle,
            IAircraftBundle takingOffBundle, List<IAircraftBundle> orderedLandingBundles,
            int intersectedBundleIndex, IntersectionCases intersectionCase);

        /// <summary>
        /// Возвращает окна для всех остальных случев пересечения
        /// </summary>
        /// <param name="intersectedBundle"></param>
        /// <param name="orderedLandingBundles"></param>
        /// <param name="intersectedBundleIndex"></param>
        /// <returns></returns>
        List<IInterval> GetEmptyWindowsForOtherCases(IAircraftBundle intersectedBundle,
            List<IAircraftBundle> orderedLandingBundles, int intersectedBundleIndex, IntersectionCases intersectionCase);
    }
}
