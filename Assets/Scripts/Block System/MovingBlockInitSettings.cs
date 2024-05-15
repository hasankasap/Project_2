
using UnityEngine;

namespace Game.BlockSystem
{
    [CreateAssetMenu(fileName = "MovingBlockInitSettings", menuName = "ScriptableObject/MovingBlockInitSettings")]
    public class MovingBlockInitSettings : ScriptableObject
    {
        public MovingBlock BlockPrefab;
        public float BlockSpeed;
        public float BlockWidth;
        public float BlockLength;
        public float BlockMovementStartPosMultiplier = 1.25f;
        [Range(0, 1)] public float PerfectMatchingThreshold = .01f;
        public int PerfectMatchBlockToGrow = 5;
        [Range(0f, 1f)] public float GrowRatio = .1f;
        public float GrowDuration = .5f;

        public Material[] BlockMaterials;
    }
}