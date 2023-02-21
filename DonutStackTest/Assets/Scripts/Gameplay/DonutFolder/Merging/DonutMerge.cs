using System.Collections.Generic;
using Gameplay.DonutFolder.Data;
using Gameplay.DonutFolder.DonutStackFolder;
using GraphFolder;
using GridFolder.Data;
using GridFolder.GridsHolderFolder;
using Infrastructure.Services.Factories;

namespace Gameplay.DonutFolder.Merging
{
    public class DonutMerge
    {
        private readonly IDonutStickFactory _stickFactory;
        private readonly DonutStickStaticData _stickStaticData;
        private MergeChecking _mergeChecking;
        private MergeSimulation _simulation;

        private Graph _graph;

        private readonly List<Node> _nodesInProcess = new();

        public DonutMerge(
            ILocationFactory locationFactory,
            IDonutStickFactory stickFactory,
            GridStaticData gridStaticData, 
            DonutStickStaticData stickStaticData)
        {
            _stickFactory = stickFactory;
            _stickStaticData = stickStaticData;
            locationFactory.OnLocationCreated += OnLocationCreated;
        }

        public void CheckForMerge(DonutStick stick) => 
            _mergeChecking.FirstCheck(stick);

        private void OnLocationCreated(GridsHolder gridsHolder)
        {
            _graph = new Graph();
            _simulation = new MergeSimulation(gridsHolder, _stickStaticData);
            _mergeChecking = new MergeChecking(gridsHolder, _graph, _simulation);

            _mergeChecking.OnNodeSimulate += node => _nodesInProcess.Add(node);
            _simulation.OnSimulated += node => _nodesInProcess.Remove(node); 
            _mergeChecking.OnAlgorithmFinished += FinishAlgorithm;
            _simulation.OnBranchOut += CheckForMerges;
        }

        private void FinishAlgorithm()
        {
            if (_nodesInProcess.Count != 0) return;
            
            _graph.MostValuablePath();
            _stickFactory.CreateDonutStick();
        }

        private void CheckForMerges(Node node) => 
            _mergeChecking.CheckForMerges(node);
    }
}