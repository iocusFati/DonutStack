using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.DonutFolder;
using Gameplay.DonutFolder.Data;
using Gameplay.DonutFolder.DonutStackFolder;
using Infrastructure.Services.AssetProvider;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.Factories
{
    public class DonutStickFactory : IDonutStickFactory
    {
        private readonly IAssets _assetProvider;
        private readonly DonutSpawner _donutSpawner;

        private readonly float _firstSpawnPosY;
        private readonly float _distanceToNext;
        private readonly int _donutTypesNum;
        private readonly int _donutsNumber;
        
        private Vector3 _createAt;

        public event Action<DonutStick> OnDonutStackCreated;

        public DonutStickFactory(IAssets assets, DonutStaticData donutStaticData, DonutSpawner donutSpawner)
        {
            _assetProvider = assets;
            _donutSpawner = donutSpawner;

            _firstSpawnPosY = donutStaticData.FirstDonutY;
            _donutsNumber = donutStaticData.DonutsNumber;
            _donutTypesNum = donutStaticData.DonutTypesNum;
            _distanceToNext = donutStaticData.DistanceToNext;
        }

        public void Initialize(Vector3 createAt) => 
            _createAt = createAt;

        public void CreateDonutStick()
        {
            float lastSpawnedDonutPosY = _firstSpawnPosY;
            DonutStick donutStick = _assetProvider.Instantiate<DonutStick>(AssetPaths.DonutStick, _createAt);
            Transform donutStackTransform = donutStick.transform;
            List<DonutType> createdTypes = new List<DonutType>();
            
            int randomDonutsNum = RandomDonutsNum();
            for (int i = 0; i < randomDonutsNum; i++)
            {
                var randomDonut = RandomDonut(createdTypes, randomDonutsNum);
                Donut donut = _donutSpawner.SpawnDonut(randomDonut);
                donutStick.Stack.Push(donut);
                
                Transform donutTransform = donut.transform;
                donutTransform.SetParent(donutStackTransform);

                SetDonutPos(i, donutTransform);
            }
            
            OnDonutStackCreated.Invoke(donutStick);

            void SetDonutPos(int donutNum, Transform donutTransform)
            {
                donutTransform.localPosition = donutNum == 0
                    ? new Vector3(0, _firstSpawnPosY, 0)
                    : new Vector3(0, lastSpawnedDonutPosY += _distanceToNext, 0);
            }
        }

        private int RandomDonutsNum() => 
            Random.Range(1, _donutsNumber + 1);

        private DonutType RandomDonut(List<DonutType> createdTypes, int donutsNumber)
        {
            if (createdTypes.Count == donutsNumber - 1 && donutsNumber > 1)
                if (createdTypes.Count(type => type == createdTypes[0]) == donutsNumber - 1)
                    return RandomExcept(createdTypes[0]);
            
            var donutType = (DonutType)Random.Range(1, _donutTypesNum);
            createdTypes.Add(donutType);

            return donutType;
        }

        private DonutType RandomExcept(DonutType except)
        {
            DonutType donutType;
            do
            {
                donutType = (DonutType)Random.Range(1, _donutTypesNum);
            } 
            while (donutType == except);

            return donutType;
        }
    }
}