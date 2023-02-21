using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Services.Factories;
using Infrastructure.Update;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitState> _states;
        private IExitState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, Updater updater, AllServices services)
        { 

            _states = new Dictionary<Type, IExitState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, updater),
                [typeof(LoadLevelState)] = new LoadLevelState(this , sceneLoader, services.Single<IDonutStickFactory>(),
                    services.Single<ILocationFactory>()),
                [typeof(GameLoopState)] = new GameLoopState()
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState => 
            _states[typeof(TState)] as TState;
    }
}