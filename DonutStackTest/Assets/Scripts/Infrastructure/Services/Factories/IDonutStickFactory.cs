using System;
using Gameplay.DonutFolder;
using Gameplay.DonutFolder.DonutStackFolder;
using UnityEngine;

namespace Infrastructure.Services.Factories
{
    public interface IDonutStickFactory : IService
    {
        event Action<DonutStick> OnDonutStackCreated;
        void CreateDonutStick();
        void Initialize(Vector3 createAt);
    }
}