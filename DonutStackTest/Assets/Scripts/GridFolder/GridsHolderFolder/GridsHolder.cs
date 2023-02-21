using System.Collections.Generic;
using System.Linq;
using Gameplay.DonutFolder.DonutStackFolder;
using Gameplay.DonutFolder.Merging;
using GridFolder.Data;

namespace GridFolder.GridsHolderFolder
{
    public class GridsHolder
    {
        private readonly Dictionary<int, List<Grid>> _grids;
        
        private readonly int _rows;
        private readonly int _columns;

        public GridsHolder(GridStaticData gridStaticData, Dictionary<int, List<Grid>> grids)
        {
            _grids = grids;

            _rows = gridStaticData.Rows;
            _columns = gridStaticData.Columns;
        }

        public List<Grid> GetColumn(int column) => 
            _grids[column];

        public GridMap MakeCopyOfGrids()
        {
            return new GridMap(_grids);
        }

        public Grid GetGrid(int column, int row, Dictionary<int, List<Grid>> gridMap)
        {
            if (column >= 0 && column < _columns && 
                row >= 0 && row < _rows)
                return gridMap[column].ElementAt(row);

            return default;
        }

        public Grid GetFrontGrid(DonutStick donutStick, Dictionary<int, List<Grid>> gridMap = null)
        {
            int gridColumn = donutStick.Grid.Column;
            int gridRow = donutStick.Grid.Row - 1;
            
            return GetGrid(gridColumn, gridRow, ChooseGridMap(gridMap));
        }

        public Grid GetBackGrid(DonutStick donutStick, Dictionary<int, List<Grid>> gridMap = null)
        {
            int gridColumn = donutStick.Grid.Column;
            int gridRow = donutStick.Grid.Row + 1;
            
            return GetGrid(gridColumn, gridRow, ChooseGridMap(gridMap));
        }

        public Grid GetRightGrid(DonutStick donutStick, Dictionary<int, List<Grid>> gridMap = null)
        {
            int gridColumn = donutStick.Grid.Column + 1;
            int gridRow = donutStick.Grid.Row;

            return GetGrid(gridColumn, gridRow, ChooseGridMap(gridMap));
        }

        public Grid GetLeftGrid(DonutStick donutStick, Dictionary<int, List<Grid>> gridMap = null)
        {
            int gridColumn = donutStick.Grid.Column - 1;
            int gridRow = donutStick.Grid.Row;

            return GetGrid(gridColumn, gridRow, ChooseGridMap(gridMap));
        }

        public Grid GetGridInDirection(MergeDirection direction, Grid grid, Dictionary<int, List<Grid>> gridMap = null)
        {
            int gridColumn = 0;
            int gridRow = 0;
            
            switch (direction)
            {
                case MergeDirection.Unknown:
                    break;
                case MergeDirection.Front:
                    gridColumn = grid.Column;
                    gridRow = grid.Row - 1;
                    break;
                case MergeDirection.Back:
                    gridColumn = grid.Column;
                    gridRow = grid.Row + 1;
                    break;
                case MergeDirection.Right:
                    gridColumn = grid.Column + 1;
                    gridRow = grid.Row;
                    break;
                case MergeDirection.Left:
                    gridColumn = grid.Column - 1;
                    gridRow = grid.Row;
                    break;
            }
            
            return GetGrid(gridColumn, gridRow, ChooseGridMap(gridMap));
        }

        private Dictionary<int, List<Grid>> ChooseGridMap(Dictionary<int, List<Grid>> gridMap)
        {
            if (gridMap == null)
                return _grids;

            return gridMap;
        }
    }
}