using System;
using System.Collections.Generic;
using Gameplay.DonutFolder.Data;
using Gameplay.DonutFolder.DonutStackFolder;
using GraphFolder;
using GridFolder.GridsHolderFolder;
using UnityEngine;
using Grid = GridFolder.Grid;

namespace Gameplay.DonutFolder.Merging
{
    public class MergeSimulation
    {
        private readonly GridsHolder _gridsHolder;
        private readonly int _maxStackCount;

        public event Action<Node> OnBranchOut;
        public event Action<Node> OnSimulated; 

        public MergeSimulation(GridsHolder gridsHolder, DonutStickStaticData stickStaticData)
        {
            _gridsHolder = gridsHolder;

            _maxStackCount = stickStaticData.MaxStackCount;
        }

        public void SimulateMerge(Node baseNode, out bool noBranchOut)
        {
            GridMap gridsMap = baseNode.GridMap;
            var topType = baseNode.StickGiverStackCopy.GetTopDonutType();
            noBranchOut = true;
            
            Stack<Donut> mainStackCopy = baseNode.StickGiverStackCopy;
            Stack<Donut> otherStackCopy = baseNode.StickReceiverStackCopy;

            mainStackCopy.TransferTo(otherStackCopy, baseNode.TransferAmount, _maxStackCount, out int donutsTransferred);
            baseNode.TransferAmount = donutsTransferred;

            if (donutsTransferred == 0)
            {
                Debug.Log("Zero transferred");
            }
            else
            {
                if (otherStackCopy.GetDonutNumberOfType(topType) == _maxStackCount)
                {
                    TryGetSticksDown(baseNode.StickReceiver.Grid, gridsMap,  BranchOut, out noBranchOut);
                    baseNode.Value++;
                }
                else if (otherStackCopy.Count < _maxStackCount)
                {
                    BranchOut(baseNode.StickReceiver);
                    noBranchOut = false;
                }
                
                if (mainStackCopy.Count == 0)
                {
                    TryGetSticksDown(baseNode.StickGiver.Grid, gridsMap, BranchOut, out noBranchOut);
                }   
                else if (mainStackCopy.Count > 0)
                {
                    BranchOut(baseNode.StickGiver);
                    noBranchOut = false;
                }
            }
            
            OnSimulated.Invoke(baseNode);
            
            void BranchOut(DonutStick stick)
            {
                var newNode = new Node(baseNode ,stick, gridsMap, baseNode.ExceptDirection, baseNode.Value);
                OnBranchOut.Invoke(newNode);
            }
        }
        
        private void TryGetSticksDown(
            Grid grid, 
            GridMap gridsMap,
            Action<DonutStick> onBranchOut,
            out bool noBranchOut)
        {
            int i = 1;
            var gridCopy = _gridsHolder.GetGrid(grid.Column, grid.Row, gridsMap.Grids);
            grid.MakeFree();
            noBranchOut = true;

            while (BackStick() is not null)
            {
                var donutStick = BackStick();
                _gridsHolder.GetGrid(gridCopy.Column, gridCopy.Row + i - 1, gridsMap.Grids).Occupy(donutStick);
                _gridsHolder.GetGrid(gridCopy.Column, gridCopy.Row + i, gridsMap.Grids).MakeFree();
                
                onBranchOut.Invoke(donutStick);
                noBranchOut = false;
                i++;
                
                //add displacement to the queue with moving actions
            }
            
            DonutStick BackStick() => 
                _gridsHolder.GetGrid(grid.Column, grid.Row + i, gridsMap.Grids).OccupiedBy;
        }
    }
}