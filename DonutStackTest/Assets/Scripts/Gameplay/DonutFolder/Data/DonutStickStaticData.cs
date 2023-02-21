using UnityEngine;

namespace Gameplay.DonutFolder.Data
{
    [CreateAssetMenu(fileName = "DonutStickData", menuName = "StaticData/DonutStickData")]
    public class DonutStickStaticData : ScriptableObject
    {
        public int MaxStackCount;
    }
}