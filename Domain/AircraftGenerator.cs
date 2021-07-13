using System;
using System.Collections.Generic;
using OptimalMotion2.Domain.Static;

namespace OptimalMotion2.Domain
{
    public class AircraftGenerator : IAircraftGenerator
    {
        public AircraftGenerator(IAircraftIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        private readonly IAircraftIdGenerator idGenerator;
        private readonly Random random = new Random();
        private static IAircraftGenerator _instance;
        private static readonly object SyncRoot = new object();


        IAircraftGenerator IAircraftGenerator.GetInstance(IAircraftIdGenerator idGenerator)
        {
            return GetInstance(idGenerator);
        }

        public static IAircraftGenerator GetInstance(IAircraftIdGenerator idGenerator)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AircraftGenerator(idGenerator);
                }
            }
            return _instance;
        }

        public ITakingOffAircraft GetTakingOffAircraft(IMoment takingOffMoment, IRunway runway, ISpecPlatform specPlatform)
        {
            var creationData = GetTakingOffAircraftCreationData(takingOffMoment, runway, specPlatform);
            return new TakingOffAircraft(creationData);
        }

        public ILandingAircraft GetLandingAircraft(LandingAircraftMoments moments,
            LandingAircraftIntervals intervals, int runwayIndex = 0)
        {
            var creationData = GetLandingAircraftCreationData(moments, intervals, runwayIndex);
            return new LandingAircraft(creationData);
        }

        
        private ITakingOffAircraftData GetTakingOffAircraftCreationData(IMoment takingOffMoment, 
            IRunway runway, ISpecPlatform specPlatform)
        {
            var id = idGenerator.GetUniqueAircraftId();

            var creationMoments = new TakingOffAircraftMoments(takingOffMoment);
            var creationIntervals = GetTakingOffAircraftCreationIntervals();

            var processingIsNeededVariants = new List<bool> {true};
            var processingIsNeeded = processingIsNeededVariants[random.Next(0, processingIsNeededVariants.Count)];

            return new TakingOffAircraftData(id, creationMoments, creationIntervals, runway, specPlatform, processingIsNeeded);
        }

        /// <summary>
        /// Sets intervals, which aircraft should have if intervals will differs from each other depending on aircraft
        /// </summary>
        /// <returns>Intervals in seconds</returns>
        private TakingOffAircraftIntervals GetTakingOffAircraftCreationIntervals()
        {
            var motionFromParkingToPS = new Interval(new Moment(0), new Moment(240));
            var motionFromPSToES = new Interval(new Moment(0), new Moment(30));
            var takeoff = new Interval(new Moment(0), new Moment(40));
            var motionFromParkingToSP = new Interval(new Moment(0), new Moment(180));
            var motionFromSPToPS = new Interval(new Moment(0), new Moment(180));

            return new TakingOffAircraftIntervals(motionFromParkingToPS, motionFromPSToES, takeoff,
                motionFromParkingToSP, motionFromSPToPS);
        }

        private ILandingAircraftData GetLandingAircraftCreationData(LandingAircraftMoments moments, 
            LandingAircraftIntervals intervals, int runwayIndex = 0)
        {
            var id = idGenerator.GetUniqueAircraftId();

            return new LandingAircraftData(id, runwayIndex, moments, intervals);
        }
    }
}
