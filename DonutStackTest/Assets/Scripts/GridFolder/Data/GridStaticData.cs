using UnityEngine;

namespace GridFolder.Data
{
    [CreateAssetMenu(fileName = "GridData", menuName = "StaticData/GridData")]
    public class GridStaticData : ScriptableObject
    {
        public int Rows;
        public int Columns;
    }
}