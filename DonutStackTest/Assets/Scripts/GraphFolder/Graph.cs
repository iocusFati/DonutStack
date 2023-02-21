using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.DonutFolder.DonutStackFolder;
using UnityEngine;

namespace GraphFolder
{
    public class Graph
    {
        public Queue<Tuple<DonutStick, DonutStick, int>> ActionsToDo = new();
        
        private readonly List<Node> _nodes = new();
        private readonly Dictionary<Node, int> _values = new();

        public void MostValuablePath()
        {
            foreach (var node in _nodes)
            {
                node.GetValue(0, AddValue);
            }

            if (_values.Count > 0)
            {
                var maxValueKey = _values.FirstOrDefault(x => x.Value == _values.Values.Max()).Key;
                // Debug.Log($"Value: {_values[maxValueKey]}");
                _values.Clear();
            }
        }

        private void AddValue(int value, Node node)
        {
            _values.Add(node, value);
            Debug.Log($"Value: {value}");
        }

        private void GetActions(Node node) => 
            _nodes[Convert.ToInt32(node.ID.ElementAt(0))].GetNode(node.ID, 1);

        public void AddNode(Node node) => 
            _nodes.Add(node);
    }
}