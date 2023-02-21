using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.DonutFolder.Data
{
    [CreateAssetMenu(fileName = "DonutData", menuName = "StaticData/DonutData")]
    public class DonutStaticData : ScriptableObject
    {
        [FormerlySerializedAs("StartPosX")] [Header("Slide")]
        public float StartPosZ;
        public float MovementDurationPerGrid;

        [Header("Create")] 
        public float FirstDonutY;
        public float DistanceToNext;
        public int DonutTypesNum;
        public int DonutsNumber;
    }
}