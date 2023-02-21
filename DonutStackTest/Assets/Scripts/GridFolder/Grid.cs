using Gameplay.DonutFolder.DonutStackFolder;
using UnityEngine;

namespace GridFolder
{
    public class Grid
    {
        public Vector3 Pos { get; set; }
        public  int Column;
        public int Row { get; set; }
        public bool Occupied { get; set; }
        public DonutStick OccupiedBy { get; set; }

        public void Occupy(DonutStick donutStick)
        {
            Occupied = true;
            OccupiedBy = donutStick;
            
            donutStick.Occupy(this);
        }

        public void MakeFree()
        {
            Occupied = false;
            OccupiedBy = null;
        }
    }
}