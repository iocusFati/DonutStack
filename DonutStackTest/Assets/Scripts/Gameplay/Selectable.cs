using UnityEngine;

namespace Gameplay
{
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private GameObject _selected;
        
        public int Order;

        public void Select() => 
            _selected.SetActive(true);

        public void Deselect() => 
            _selected.SetActive(false);
    }
}