using Gameplay;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        Selectable PointLine();
        bool OnMouseClick();
    }
}