using Gameplay.DonutFolder.Merging;

namespace Infrastructure.Services.MergeValueCounter
{
    public interface IMergeValueCalculatorService
    {
        void Initialize();
        void Start(MergeDirection direction);
        void RaiseValue();
    }
}