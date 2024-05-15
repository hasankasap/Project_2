using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool addiveLevel = false;
        bool gameStarted = false, finished = false;

        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.FIRST_CLICK, OnFirstClick);
            EventManager.StartListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClick);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.FIRST_CLICK, OnFirstClick);   
            EventManager.StopListening(GameEvents.MOUSE_CLICK_DOWN, OnMouseClick);   
        }

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            LevelManager.Instance.LoadLevel(false);
        }
        private void OnFirstClick(object[] obj)
        {
            gameStarted = true;
        }
        private void OnMouseClick(object[] obj)
        {
            if (gameStarted && finished)
            {
                finished = false;
                EventManager.TriggerEvent(GameEvents.CONTINUE_GAME, null);
            }
        }
        public void OnLevelFail()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void OnLevelSuccess(Vector3 finishPos) 
        {
            finished = true;
            LevelManager.Instance.LoadLevel(addiveLevel, finishPos);
        }
    }
}