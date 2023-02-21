using DG.Tweening;
using Gameplay.DonutFolder.Data;
using Gameplay.DonutFolder.DonutStackFolder;
using Gameplay.DonutFolder.Merging;
using GridFolder.GridsHolderFolder;
using Infrastructure.Services.Factories;
using UnityEngine;
using Grid = GridFolder.Grid;

namespace Gameplay.DonutFolder
{
    public class DonutMovement
    {
        private readonly IDonutStickFactory _donutStickFactory;
        private readonly DonutMerge _donutMerge;
        private GridsHolder _gridsHolder;
        
        private readonly float _startPosZ;
        private readonly float _movementDurationPerGrid;
        
        private DonutStick _donutStick;
        private Transform _donutStackTransform;

        public DonutMovement(ILocationFactory locationFactory, IDonutStickFactory donutStickFactory,
            DonutStaticData donutStaticData, DonutMerge donutMerge)
        {
            _donutStickFactory = donutStickFactory;
            _donutMerge = donutMerge;
            _startPosZ = donutStaticData.StartPosZ;
            _movementDurationPerGrid = donutStaticData.MovementDurationPerGrid;

            donutStickFactory.OnDonutStackCreated += SetDonutStack;  
            locationFactory.OnLocationCreated += gridsHolder => _gridsHolder = gridsHolder;
        }

        public void SlideOnLine(int order)
        {
            Grid grid = FindAvailableGrid(order);
            grid.Occupy(_donutStick);

            _donutStackTransform.position = new Vector3(grid.Pos.x, grid.Pos.y + 0.05f, _startPosZ);
            _donutStackTransform.DOMoveZ(grid.Pos.z, _movementDurationPerGrid).OnComplete(() => 
                _donutMerge.CheckForMerge(_donutStick));
        }

        private Grid FindAvailableGrid(int order)
        {
            foreach (var grid in _gridsHolder.GetColumn(order))
            {
                if (!grid.Occupied)
                {
                    return grid;
                }
            }

            return default;
        }

        private void SetDonutStack(DonutStick donutStick)
        { 
            _donutStick = donutStick;
            _donutStackTransform = donutStick.transform;
        }
    }
}