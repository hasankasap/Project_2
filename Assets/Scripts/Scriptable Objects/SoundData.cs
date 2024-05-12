using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sounds/SoundData")]

    public class SoundData : ScriptableObject
    {
        public Sound soundType;
        [SerializeField] AudioClip[] clips;
        public bool pickRandom;
        public AudioClip Clip
        {
            get
            {
                if (pickRandom)
                    return clips[Random.Range(0, clips.Length)];
                else
                {
                    return clips[0];
                }
            }
        }
        public bool loop = false;
        public bool playOnAwake = false;
        [Range(0, 256)] public int priorty = 128;
        [Range(0, 1f)] public float volume = 1f;
        [Range(-3f, 3f)] public float pitch = 1.0f;
        [Range(-1f, 1f)] public float streoPan = 0;
        [Range(0f, 1f)] public float spatialBlend = 0;
        [Range(0f, 1.1f)] public float reverbZoneMix = 1f;
    }
}