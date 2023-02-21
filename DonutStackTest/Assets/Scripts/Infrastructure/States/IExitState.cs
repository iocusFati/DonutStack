namespace Infrastructure.States
{
    public interface IExitState
    {
        public void Exit();
    }

    public interface IState : IExitState
    {
        public void Enter();
    }
}