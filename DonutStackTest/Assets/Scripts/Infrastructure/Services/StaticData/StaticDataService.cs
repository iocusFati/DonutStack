using Gameplay.DonutFolder.Data;
using GridFolder.Data;
using Infrastructure.Services.AssetProvider;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public GridStaticData GridData { get; private set; }
        public DonutStaticData DonutData { get; private set; }
        public DonutStickStaticData DonutStickData { get; set; }

        public void Initialize()
        {
            LoadGridData();
            LoadDonutData();
            LoadDonutStickData();
        }

        private void LoadDonutData() => 
            DonutData = Resources.Load<DonutStaticData>(AssetPaths.DataDonut);

        private void LoadGridData() => 
            GridData = Resources.Load<GridStaticData>(AssetPaths.GridStaticData);

        private void LoadDonutStickData() => 
            DonutStickData = Resources.Load<DonutStickStaticData>(AssetPaths.DonutStickData);
    }
}