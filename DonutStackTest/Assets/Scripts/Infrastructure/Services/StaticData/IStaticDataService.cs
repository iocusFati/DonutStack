using Gameplay.DonutFolder.Data;
using GridFolder.Data;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        GridStaticData GridData { get; }
        DonutStaticData DonutData { get; }
        DonutStickStaticData DonutStickData { get; set; }
        void Initialize();
    }
}