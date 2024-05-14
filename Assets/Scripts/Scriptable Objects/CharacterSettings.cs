using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObject/CharacterSettings")]
    public class CharacterSettings : ScriptableObject
    {
        public float StraightMovementSpeed = 1.2f;
        public float JumpingDuration = .75f;
        public float JumpingHeight = 1f;
        public float FallingDuration = 2f;
        public float FallingJumpHeight = 5f;
    }
}