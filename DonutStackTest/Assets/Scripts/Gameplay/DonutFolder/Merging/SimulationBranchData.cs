using System.Collections.Generic;
using Gameplay.DonutFolder.DonutStackFolder;
using GridFolder.GridsHolderFolder;

namespace Gameplay.DonutFolder.Merging
{
    public struct SimulationBranchData
    {
        public int Value { get; set; }
        public GridMap? GridMap { get; set; }
        public MergeDirection DontMergeDirection { get; set; }
        
        public Queue<DonutStick> CheckLater;
    }
}