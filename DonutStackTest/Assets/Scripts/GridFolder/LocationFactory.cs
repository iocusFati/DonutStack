using System;
using System.Collections.Generic;
using System.Linq;
using GridFolder.Data;
using GridFolder.GridsHolderFolder;
using Infrastructure.Services;
using Infrastructure.Services.AssetProvider;
using Infrastructure.Services.Factories;
using UnityEngine;

namespace GridFolder
{
    public class LocationFactory : ILocationFactory
    {
        private readonly AllServices _allServices;
        private readonly IAssets _assetProvider;
        private readonly GridStaticData _gridStaticData;

        private readonly int _rows;
        private readonly int _columns;

        private Vector3 _initialPoint;
        
        public event Action<GridsHolder> OnLocationCreated;

        public LocationFactory(AllServices allServices, IAssets assetProvider, GridStaticData gridStaticData)
        {
            _allServices = allServices;
            _assetProvider = assetProvider;
            _gridStaticData = gridStaticData;

            _rows = gridStaticData.Rows;
            _columns = gridStaticData.Columns;
        }

        public void CreateLocation()
        {
            Location location = _assetProvider.Instantiate<Location>(AssetPaths.LocationPath);
            _initialPoint = location.InitialPoint.position;
            
            OnLocationCreated.Invoke(new GridsHolder(_gridStaticData, GroupGridsInColumns()));
        }

        private Dictionary<int, List<Grid>> GroupGridsInColumns()
        {
            var dictionary = new Dictionary<int, List<Grid>>(8);
            
            for (int column = 0; column < EvaluateGrids().Count; column++)
            {
                dictionary.Add(column, CreateGridsIn(EvaluateGrids().ElementAt(column), column));
            }

            return dictionary;
        }

        private List<Grid> CreateGridsIn(List<Vector3> gridPositions, int column)
        {
            List<Grid> grids = new List<Grid>(_rows);

            for (int i = 0; i < gridPositions.Count; i++)
            {
                Grid grid = new Grid
                {
                    Pos = gridPositions[i],
                    Column = column,
                    Row = i
                };
                
                grids.Add(grid);
            }
            // foreach (var gridPos in gridPositions)
            // {
            //     Grid grid = new Grid
            //     {
            //         Pos = gridPos,
            //         Place = 
            //     };
            //     
            //     grids.Add(grid);
            // }

            return grids;
        }

        private List<List<Vector3>> EvaluateGrids()
        {
            List<List<Vector3>> columns = new List<List<Vector3>>(_columns);

            for (int x = 0; x < _columns; x++)
            {
                List<Vector3> gridPositions = new List<Vector3>();
                
                for (int z = 0; z < _rows; z++)
                {
                    gridPositions.Add(new Vector3(_initialPoint.x + x, 0, 
                        _initialPoint.z + z));
                }

                gridPositions.Reverse();
                columns.Add(gridPositions);
            }

            return columns;
        }
    }
}