using OptimalMotion2.Enums;

namespace OptimalMotion2.Domain
{
    public interface IModel
    {
        void ChangeStage(ModelStages stage);
        void ResetIdGenerator();
    }
}
