using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface IAircraft
    {
        IAircraftId Id { get; set; }
        IMoment OrderMoment { get; set; }
        AircraftType Type { get; }
        int GetRunwayId();
    }
}
