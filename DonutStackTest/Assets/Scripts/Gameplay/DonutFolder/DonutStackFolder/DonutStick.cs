using System.Collections.Generic;
using UnityEngine;
using Grid = GridFolder.Grid;

namespace Gameplay.DonutFolder.DonutStackFolder
{
    public class DonutStick : MonoBehaviour
    {
        public Grid Grid { get; private set; }
        public readonly Stack<Donut> Stack = new();

        public void Occupy(Grid grid) => 
            Grid = grid;
    }
}