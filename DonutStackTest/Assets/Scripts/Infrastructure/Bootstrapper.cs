using System;
using Infrastructure.Services;
using Infrastructure.States;
using Infrastructure.Update;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private void Awake()
        {
            SceneLoader sceneLoader = new SceneLoader(this);
            Updater updater = gameObject.AddComponent<Updater>();
            GameStateMachine gameStateMachine = new GameStateMachine(sceneLoader, updater, AllServices.Container);
            
            gameStateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }

    }
}
