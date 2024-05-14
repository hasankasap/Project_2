using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();

                    if (instance == null)
                    {
                        GameObject obj = new GameObject("AudioManager");
                        instance = obj.AddComponent<AudioManager>();
                    }
                }
                return instance;
            }
        }

        public List<SoundData> SoundDatas = new List<SoundData>();
        [SerializeField] private List<CreatedSound> audioSources = new List<CreatedSound>();

        [System.Serializable]
        protected class CreatedSound
        {
            public Sound SoundType;
            public AudioSource SoundSource;
            public float InitPitch { get; private set; }
            [HideInInspector] public SoundData SoundData;

            public CreatedSound(SoundData s)
            {
                SoundData = s;
                GameObject obj = new GameObject("New Sound", typeof(AudioSource));
                obj.transform.parent = Camera.main.transform;
                SoundSource = obj.GetComponent<AudioSource>();
                SoundType = s.SoundType;
                UpdateVariables();
                InitPitch = SoundSource.pitch;
            }
            public void UpdateVariables()
            {
                SoundSource.loop = SoundData.Loop;
                SoundSource.playOnAwake = SoundData.PlayOnAwake;
                SoundSource.clip = SoundData.Clip;
                SoundSource.priority = SoundData.Priorty;
                SoundSource.volume = SoundData.Volume;
                SoundSource.pitch = SoundData.Pitch;
                SoundSource.panStereo = SoundData.StreoPan;
                SoundSource.spatialBlend = SoundData.SpatialBlend;
                SoundSource.reverbZoneMix = SoundData.ReverbZoneMix;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            SoundData[] soundDataArray = Resources.LoadAll<SoundData>("Sounds");
            SoundDatas.AddRange(soundDataArray);
            if (SoundDatas.Count > 0)
            {
                for (int i = 0; i < SoundDatas.Count; i++)
                {
                    SoundData s = SoundDatas[i];
                    CreatedSound createdSound = new CreatedSound(s);
                    createdSound.SoundSource.gameObject.name = s.name;
                    audioSources.Add(createdSound);
                }
            }
        }

        public static void PlaySound(Sound soundType, bool changePitch, bool resetPitch)
        {
            if (Instance != null)
            {
                // Find the SoundData with the specified soundType
                CreatedSound sound = Instance.audioSources.FirstOrDefault(x => x.SoundType == soundType);

                // Check if the soundData is not null and the associated AudioSource is not null
                if (sound != null && sound.SoundSource != null)
                {
                    if (sound.SoundData.UpdateOnPlayMode)
                        sound.UpdateVariables();

                    if (resetPitch)
                        sound.SoundSource.pitch = sound.InitPitch;

                    sound.SoundSource.Play();

                    if (changePitch && sound.SoundSource.pitch > sound.SoundData.MinPitch && sound.SoundSource.pitch < sound.SoundData.MaxPitch)
                        sound.SoundSource.pitch += sound.SoundData.PitchChange;
                }
                else
                {
                    Debug.LogWarning("Sound not found: " + soundType);
                }
            }
            else
            {
                Debug.LogWarning("AudioManager not found.");
            }
        }
        public static void StopSound(Sound soundType)
        {
            if (Instance != null)
            {
                // Find the SoundData with the specified soundType
                CreatedSound sound = Instance.audioSources.FirstOrDefault(x => x.SoundType == soundType);

                // Check if the soundData is not null and the associated AudioSource is not null
                if (sound != null && sound.SoundSource != null)
                {
                    sound.SoundSource.Stop();
                }
                else
                {
                    Debug.LogWarning("Sound not found: " + soundType);
                }
            }
            else
            {
                Debug.LogWarning("AudioManager not found.");
            }
        }
        public static void ChangeMuteStatusForAll(bool status)
        {
            if (Instance != null)
            {
                List<CreatedSound> sounds = Instance.audioSources;
                if (sounds.Count == 0) return;
                foreach (CreatedSound c in sounds)
                {
                    c.SoundSource.mute = !status;
                }
            }
            else
            {
                Debug.LogWarning("AudioManager not found.");
            }
        }

        public static void ChangeMuteStatus(Sound sound, bool status)
        {
            if (Instance != null)
            {
                List<CreatedSound> sounds = Instance.audioSources;
                if (sounds.Count == 0) return;
                CreatedSound c = sounds.FirstOrDefault(x => x.SoundType == sound);
                if (c != null)
                {
                    c.SoundSource.mute = status;
                }
            }
            else
            {
                Debug.LogWarning("AudioManager not found.");
            }
        }
    }
}