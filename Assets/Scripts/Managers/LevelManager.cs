using Game.BlockSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(LevelGenerator))]
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private MovingBlockInitSettings blockInitSettings;

        private LevelGenerator levelGenerator;
        private Level currentLevel;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            levelGenerator = GetComponent<LevelGenerator>();
        }

        public void LoadLevel(bool additive = false)
        {
            if (!additive && currentLevel != null)
            {
                Destroy(currentLevel.gameObject);
            }
            if (levelGenerator == null)
            {
                levelGenerator = GetComponent<LevelGenerator>();
            }
            currentLevel = levelGenerator.GenerateLevel(additive);
            currentLevel.SetFinishWidth(blockInitSettings.BlockWidth);
            EventManager.TriggerEvent(GameEvents.LEVEL_CREATED, new object[] { currentLevel });
        }
        public void LoadLevel(bool additive = false, Vector3 targetPos = default)
        {
            if (!additive && currentLevel != null)
            {
                Destroy(currentLevel.gameObject);
            }
            if (levelGenerator == null)
            {
                levelGenerator = GetComponent<LevelGenerator>();
            }
            currentLevel = levelGenerator.GenerateLevel(additive ,targetPos);
            currentLevel.SetFinishWidth(blockInitSettings.BlockWidth);
            EventManager.TriggerEvent(GameEvents.LEVEL_CREATED, new object[] { currentLevel });
        }
        public void LevelFail()
        {
            GameManager.Instance.OnLevelFail();
        }
        public void LevelComplete()
        {
            GameManager.Instance.OnLevelSuccess(currentLevel.Finish.position);
        }
    }
}