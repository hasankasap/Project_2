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

        public List<SoundData> soundDatas = new List<SoundData>();
        [SerializeField] List<CreatedSound> audioSources = new List<CreatedSound>();

        [System.Serializable]
        protected class CreatedSound
        {
            public Sound soundType;
            public AudioSource soundSource;
            [HideInInspector] public SoundData soundData;

            public CreatedSound(SoundData s)
            {
                soundData = s;
                GameObject obj = new GameObject("New Sound", typeof(AudioSource));
                obj.transform.parent = Camera.main.transform;
                soundSource = obj.GetComponent<AudioSource>();
                soundType = s.soundType;
                UpdateVariables();

            }
            public void UpdateVariables()
            {
                soundSource.loop = soundData.loop;
                soundSource.playOnAwake = soundData.playOnAwake;
                soundSource.clip = soundData.Clip;
                soundSource.priority = soundData.priorty;
                soundSource.volume = soundData.volume;
                soundSource.pitch = soundData.pitch;
                soundSource.panStereo = soundData.streoPan;
                soundSource.spatialBlend = soundData.spatialBlend;
                soundSource.reverbZoneMix = soundData.reverbZoneMix;
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
            soundDatas.AddRange(soundDataArray);
            if (soundDatas.Count > 0)
            {
                for (int i = 0; i < soundDatas.Count; i++)
                {
                    SoundData s = soundDatas[i];
                    CreatedSound createdSound = new CreatedSound(s);
                    createdSound.soundSource.gameObject.name = s.name;
                    audioSources.Add(createdSound);
                }
            }
        }

        public static void PlaySound(Sound soundType)
        {
            if (Instance != null)
            {
                // Find the SoundData with the specified soundType
                CreatedSound sound = Instance.audioSources.FirstOrDefault(x => x.soundType == soundType);

                // Check if the soundData is not null and the associated AudioSource is not null
                if (sound != null && sound.soundSource != null)
                {

                    sound.UpdateVariables();
                    sound.soundSource.Play();
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
                CreatedSound sound = Instance.audioSources.FirstOrDefault(x => x.soundType == soundType);

                // Check if the soundData is not null and the associated AudioSource is not null
                if (sound != null && sound.soundSource != null)
                {
                    sound.soundSource.Stop();
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
                    c.soundSource.mute = !status;
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
                CreatedSound c = sounds.FirstOrDefault(x => x.soundType == sound);
                if (c != null)
                {
                    c.soundSource.mute = status;
                }
            }
            else
            {
                Debug.LogWarning("AudioManager not found.");
            }
        }
    }
}