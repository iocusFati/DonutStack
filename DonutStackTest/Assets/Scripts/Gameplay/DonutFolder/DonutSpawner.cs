using System.Collections.Generic;
using Infrastructure.Services.AssetProvider;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Gameplay.DonutFolder
{
    public class DonutSpawner
    {
        private readonly IAssets _assetProvider;
        
        private IObjectPool<Donut> _brownDonutPool;
        private IObjectPool<Donut> _orangeDonutPool;
        private IObjectPool<Donut> _redDonutPool;

        private readonly Dictionary<DonutType, string> _donutTypes = new()
        {
            { DonutType.Brown, AssetPaths.BrownDonut },
            { DonutType.Orange, AssetPaths.OrangeDonut },
            { DonutType.Red, AssetPaths.RedDonut }
        };

        public DonutSpawner(IAssets assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public Donut SpawnDonut(DonutType type)
        {
            switch (type)
            {
                case DonutType.Brown:
                    return GetDonutPoolBy(DonutType.Brown, ref _brownDonutPool).Get();
                case DonutType.Orange:
                    return GetDonutPoolBy(DonutType.Orange, ref _orangeDonutPool).Get();
                case DonutType.Red:
                    return GetDonutPoolBy(DonutType.Red, ref _redDonutPool).Get();
            }

            return default;
        }

        private IObjectPool<Donut> GetDonutPoolBy(DonutType type, ref IObjectPool<Donut> pool)
        {
            pool ??= new ObjectPool<Donut>(
                () => InstantiateDonut(type),
                donut => { donut.gameObject.SetActive(true); }, 
                donut => { donut.gameObject.SetActive(false); },
                donut => { Object.Destroy(donut.gameObject); });

            return pool;
        }

        private Donut InstantiateDonut(DonutType type)
        {
            return _assetProvider.Instantiate<Donut>(_donutTypes[type]);
        }
        
    }
}