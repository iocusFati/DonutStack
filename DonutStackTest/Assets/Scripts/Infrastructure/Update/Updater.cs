using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Update
{
    public class Updater : MonoBehaviour, IUpdater
    {
        private readonly List<IUpdatable> _updatables = new();

        public void AddUpdatable(IUpdatable updatable) => 
            _updatables.Add(updatable);

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }
    }
}