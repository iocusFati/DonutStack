using Gameplay;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private const string ColumLayerName = "ColumnTrigger";
        private LayerMask _columnLayer => LayerMask.NameToLayer(ColumLayerName);

        public Selectable PointLine()
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.TryGetComponent<Selectable>(out var selectable))
                    return selectable;
            }

            return default;
        }

        public bool OnMouseClick()
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
                return true;
            
            return false;
        }
    }
}