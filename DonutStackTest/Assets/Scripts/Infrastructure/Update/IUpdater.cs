namespace Infrastructure.Update
{
    public interface IUpdater
    {
        void AddUpdatable(IUpdatable updatable);
    }
}