using Gameplay;
using Gameplay.DonutFolder;
using Gameplay.DonutFolder.Merging;
using GridFolder;
using Infrastructure.Services;
using Infrastructure.Services.AssetProvider;
using Infrastructure.Services.Factories;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticData;
using Infrastructure.Update;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "Initial";
        private const string MainSceneName = "Donuts";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly Updater _updater;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services,
            Updater updater)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _updater = updater;

            RegisterServices(services);
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            var locationFactory = _services.Single<ILocationFactory>();
            var staticDataService = _services.Single<IStaticDataService>();

            var donutMerge = new DonutMerge(locationFactory, _services.Single<IDonutStickFactory>(),
                staticDataService.GridData, staticDataService.DonutStickData);
            var pointer = new Pointer(_updater, _services.Single<IInputService>(),
                new DonutMovement(locationFactory, _services.Single<IDonutStickFactory>(),
                    staticDataService.DonutData, donutMerge));

            _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);
        }

        private void RegisterServices(AllServices services)
        {
            var staticData = RegisterStaticData(services);
            var assets = services.RegisterService<IAssets>(
                new AssetProvider());

            services.RegisterService<IInputService>
                (new InputService());
            
            services.RegisterService<IDonutStickFactory>(
                new DonutStickFactory(services.Single<IAssets>(), staticData.DonutData, new DonutSpawner(assets)));
            services.RegisterService<ILocationFactory>(
                new LocationFactory(services, assets, staticData.GridData));
        }

        private StaticDataService RegisterStaticData(AllServices services)
        {
            var staticData = new StaticDataService();
            staticData.Initialize();
            services.RegisterService<IStaticDataService>(staticData);

            return staticData;
        }
    }
}