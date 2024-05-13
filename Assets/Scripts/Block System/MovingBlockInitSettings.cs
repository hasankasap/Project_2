
using UnityEngine;

namespace Game.BlockSystem
{
    [CreateAssetMenu(fileName = "MovingBlockInitSettings", menuName = "ScriptableObject/MovingBlockInitSettings")]
    public class MovingBlockInitSettings : ScriptableObject
    {
        public float PerfectMatchingThreshold = .01f;
        public MovingBlock BlockPrefab;
        public float BlockSpeed;
        public float BlockWidth;
        public float BlockLength;

        public Material[] BlockMaterials;
    }
}