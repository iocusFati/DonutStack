using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.DonutFolder.DonutStackFolder;
using GraphFolder;
using GridFolder.GridsHolderFolder;
using Grid = GridFolder.Grid;

namespace Gameplay.DonutFolder.Merging
{
    public class MergeChecking
    {
        private readonly GridsHolder _gridsHolder;
        
        private readonly Graph _graph;
        private readonly MergeSimulation _mergeSimulation;

        public event Action OnAlgorithmFinished;
        public event Action<Node> OnNodeSimulate;

        public MergeChecking(GridsHolder gridsHolder, Graph graph, MergeSimulation mergeSimulation)
        {
            _gridsHolder = gridsHolder;
            _graph = graph;
            _mergeSimulation = mergeSimulation;
        }

        public void FirstCheck(DonutStick donutStick)
        {
            var node = new Node(donutStick, _gridsHolder.MakeCopyOfGrids(), MergeDirection.Unknown);
            CheckForMerges(node, true);
        }

        public void CheckForMerges(Node node, bool isFirstCheck = false)
        {
            DonutStick donutStick = node.StickGiver;
            Stack<Donut> mainDonutStack = node.StickGiverStackCopy;
            DonutType mainDonut_TopType = mainDonutStack.GetTopDonutType();
            int mainStack_TopTypeNumber = mainDonutStack.GetDonutNumberOfType();
            Dictionary<MergeDirection, Grid> directionGridDic = DirectionGridDictionary(donutStick);

            var filteredDic = RemoveNullAndSpecificDirection(directionGridDic, Opposite(node.ExceptDirection));
            int sticksWithDifferentTopType = 0;
                
            foreach (var directionGrid in filteredDic)
            {
                Grid grid = directionGridDic[directionGrid.Key];

                if (!TopTypesSimilar(out var otherStick, mainDonut_TopType, grid))
                {
                    sticksWithDifferentTopType++;
                    continue;
                }
                
                MergeDirection direction = directionGrid.Key;
                Node node1 = CreateNode(otherStick, donutStick, node, direction, mainStack_TopTypeNumber);
                Node node2 = CreateNode(otherStick, donutStick, node, Opposite(direction), mainStack_TopTypeNumber);

                if (isFirstCheck)
                {
                    _graph.AddNode(node1);
                    _graph.AddNode(node2);
                }

                else
                {
                    node.Parent.CreateSubNode(node1);
                    node.Parent.CreateSubNode(node2);
                }

                TryMergeFor(node1, node2);
            }

            if (filteredDic.Count == sticksWithDifferentTopType) 
                OnAlgorithmFinished.Invoke();
        }

        private void TryMergeFor(Node node1, Node node2)
        {
            _mergeSimulation.SimulateMerge(node1, out bool noBranchOut1);
            _mergeSimulation.SimulateMerge(node2, out bool noBranchOut2);
            OnNodeSimulate.Invoke(node1);
            OnNodeSimulate.Invoke(node2);
            
            if(noBranchOut1 && noBranchOut2)
                OnAlgorithmFinished.Invoke();
        }

        private static Node CreateNode(
            DonutStick otherStick, 
            DonutStick donutStick,
            Node node,
            MergeDirection mergeDirection,
            int mainStack_TopTypeNumber)
        {
            return new Node(donutStick, otherStick, node.GridMap, mergeDirection,
                mainStack_TopTypeNumber, node.Value);
        }

        private bool TopTypesSimilar(out DonutStick otherStick, DonutType mainDonut_TopType, Grid grid)
        {
            otherStick = GetDonutStickFor(grid);
            
            if (otherStick is null) return false;
            if (otherStick.Stack.GetTopDonutType() != mainDonut_TopType) return false;
            return true;
        }

        private Dictionary<MergeDirection, Grid> RemoveNullAndSpecificDirection(Dictionary<MergeDirection, Grid> dictionary,
            MergeDirection exceptDirection)
        {
            var newDictionary = dictionary
                .Where(dic => dic.Value is { Occupied: true } 
                              && dic.Key != exceptDirection)
                .ToDictionary(x => x.Key, x => x.Value);

            return newDictionary;
        }

        private Dictionary<MergeDirection, Grid> DirectionGridDictionary(DonutStick donutStick)
        {
            Grid frontGrid = _gridsHolder.GetFrontGrid(donutStick);
            Grid backGrid = _gridsHolder.GetBackGrid(donutStick);
            Grid rightGrid = _gridsHolder.GetRightGrid(donutStick);
            Grid leftGrid = _gridsHolder.GetLeftGrid(donutStick);

            Dictionary<MergeDirection, Grid> directionGridDic = new()
            {
                { MergeDirection.Front, frontGrid },
                { MergeDirection.Back, backGrid },
                { MergeDirection.Right, rightGrid },
                { MergeDirection.Left, leftGrid }
            };

            return directionGridDic;
        }

        private static MergeDirection Opposite(MergeDirection exceptDirection) => 
            (MergeDirection)((int)exceptDirection * -1);

        private DonutStick GetDonutStickFor(Grid grid) => 
            grid.OccupiedBy;
    }
}