using Gameplay.DonutFolder;
using Infrastructure.Services.Input;
using Infrastructure.Update;

namespace Gameplay
{
    public class Pointer : IUpdatable
    {
        private readonly IInputService _inputService;
        private readonly DonutMovement _donutMovement;
        private Selectable _currentColumn;

        public Pointer(IUpdater updater, IInputService inputService, DonutMovement donutMovement)
        {
            updater.AddUpdatable(this);
            _inputService = inputService;
            _donutMovement = donutMovement;
        }

        public void Update()
        {
            Selectable column = _inputService.PointLine();

            if (!column)
            {
                if (!_currentColumn) return;
            
                _currentColumn.Deselect();
                _currentColumn = column;
            
                return;
            }

            if (_inputService.OnMouseClick())
            {
                _donutMovement.SlideOnLine(column.Order);
            }
            if (column == _currentColumn) return;

            if (_currentColumn is null)
            {
                _currentColumn = column;
                column.Select();
                return;
            }

            column.Select();
            _currentColumn.Deselect();
            _currentColumn = column;
        }
    }
}