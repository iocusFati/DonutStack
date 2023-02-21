using System.Collections.Generic;

namespace GridFolder.GridsHolderFolder
{
    public struct GridMap
    {
        public Dictionary<int, List<Grid>> Grids;

        public GridMap(Dictionary<int, List<Grid>> grids)
        {
            Grids = grids;
        }
    }
}