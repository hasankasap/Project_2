using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sounds/SoundData")]

    public class SoundData : ScriptableObject
    {
        public bool UpdateOnPlayMode;
        public Sound SoundType;
        [SerializeField] AudioClip[] Clips;
        public bool PickRandom;
        public AudioClip Clip
        {
            get
            {
                if (PickRandom)
                    return Clips[Random.Range(0, Clips.Length)];
                else
                {
                    return Clips[0];
                }
            }
        }
        public bool Loop = false;
        public bool PlayOnAwake = false;
        [Range(0, 256)] public int Priorty = 128;
        [Range(0, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1.0f;
        [Range(-1f, 1f)] public float StreoPan = 0;
        [Range(0f, 1f)] public float SpatialBlend = 0;
        [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;

        [Range(-3f, 3f)] public float PitchChange;
        [Range(-10f, 10f)] public float MinPitch = -3f;
        [Range(-10f, 10f)] public float MaxPitch = 3f;
    }
}