using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gameplay.DonutFolder;
using Gameplay.DonutFolder.DonutStackFolder;
using Gameplay.DonutFolder.Merging;
using GridFolder.GridsHolderFolder;
using Utility;

namespace GraphFolder
{
    public class Node
    {
        public Node Parent { get; }
        public DonutStick StickGiver { get; set; }
        public DonutStick StickReceiver { get; set; }
        public Stack<Donut> StickGiverStackCopy { get; set; }
        public Stack<Donut> StickReceiverStackCopy { get; set; }
        
        public GridMap GridMap { get; set; }
        public MergeDirection ExceptDirection { get; set; }
        public int TransferAmount { get; set; }
        public int Value { get; set; }
        public string ID { get; set; }

        private readonly List<Node> _subNodes = new();

        public Node(
            DonutStick donutStick, 
            GridMap gridMap,
            MergeDirection exceptDirection)
        {
            StickGiver = donutStick;
            StickGiverStackCopy = StickGiver.Stack;
            
            GridMap = gridMap;
            ExceptDirection = exceptDirection;
        }

        public Node(
            Node parent, 
            DonutStick stickGiver,
            GridMap gridMap,
            MergeDirection exceptDirection,
            int value)
        {
            StickGiver = stickGiver;
            StickGiverStackCopy = StickGiver.Stack;

            Parent = parent;
            GridMap = gridMap;
            ExceptDirection = exceptDirection;
            Value = value;
        }

        public Node(
            DonutStick stickGiver, 
            DonutStick stickReceiver,
            GridMap gridMap,
            MergeDirection exceptDirection,
            int transferAmount,
            int value)
        {
            StickGiver = stickGiver;
            StickReceiver = stickReceiver;
            StickGiverStackCopy = StickGiver.Stack;
            StickReceiverStackCopy = StickReceiver.Stack;
            
            GridMap = gridMap;
            ExceptDirection = exceptDirection;
            TransferAmount = transferAmount;
            Value = value;
        }

        public void CreateSubNode(Node node)
        {
            node.ID += _subNodes.Count;
            _subNodes.Add(node);
        }

        public void GetValue(int currentValue, Action<int, Node> addValue)
        {
            int newValue = currentValue + Value;
            
            if (_subNodes.Count == 0)
            {
                addValue.Invoke(newValue, this);
                return;
            }

            foreach (var node in _subNodes)
                node.GetValue(newValue, addValue);
        }

        public void GetNode(string id, int currentIndex)
        {
            if (currentIndex != id.Length - 1)
            {
                _subNodes[Convert.ToInt32(id.ElementAt(currentIndex))].GetNode(id, currentIndex + 1);
                
            }
        }
    }
}