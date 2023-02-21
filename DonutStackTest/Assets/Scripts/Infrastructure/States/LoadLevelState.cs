 using Infrastructure.Services.Factories;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IDonutStickFactory _playerFactory;
        private readonly ILocationFactory _locationFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IDonutStickFactory donutStickFactory,
            ILocationFactory locationFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _playerFactory = donutStickFactory;
            _locationFactory = locationFactory;
        }
        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            Vector3 initialPoint = GameObject.FindGameObjectWithTag(InitialPointTag).transform.position;
            
            _playerFactory.Initialize(initialPoint);
            _playerFactory.CreateDonutStick();
            _locationFactory.CreateLocation();
            
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}