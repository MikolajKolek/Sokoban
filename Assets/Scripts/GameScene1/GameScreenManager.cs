using System.Diagnostics.CodeAnalysis;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;

namespace GameScene1
{
    public class GameScreenManager : MonoBehaviour {
        #region Serialized variables
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] protected TilemapGameAdapter gameTilemapAdapter;

        [SerializeField] private TMP_Text playerMoveText;
        [SerializeField] private TMP_Text boxMoveText;
        [SerializeField] private TMP_Text timerText;

        [SerializeField] protected GameObject levelFinishObject;
        #endregion

        #region Private variables
        protected bool gameActive;

        protected int screenHeight;
        protected int screenWidth;
        #endregion

        #region Methods
        protected void Start() {
            gameTilemapAdapter.onLevelFinish = FinishLevelEvent;
            gameTilemapAdapter.onPlayerMoved = PlayerMovedEvent;
            gameTilemapAdapter.onBoxMoved = BoxMovedEvent;
            gameTilemapAdapter.onTimePassed = TimePassedEvent;
        }

        // Update is called once per frame
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private void Update() {
            if (gameActive) {
                if (screenHeight != Screen.height || screenWidth != Screen.width) {
                    screenHeight = Screen.height;
                    screenWidth = Screen.width;

                    gameTilemapAdapter.WindowScaleUpdate();
                }
                
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    gameTilemapAdapter.MovePlayer(Vector3Int.up);
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    gameTilemapAdapter.MovePlayer(Vector3Int.left);
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    gameTilemapAdapter.MovePlayer(Vector3Int.down);
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    gameTilemapAdapter.MovePlayer(Vector3Int.right);
                }
                if (Input.GetKeyDown(KeyCode.R)) {
                    RestartLevel();
                }
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    LeaveGame(false);
                }
                
                //TODO: Remove this later! For development use only!
                if (Input.GetKeyDown(KeyCode.RightControl)) {
                    //gameTilemapAdapter.RewindLastMove();
                }
            }
        }
        
        public virtual void LeaveGame(bool playButtonClick) {
            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
            gameActive = false;
            levelFinishObject.SetActive(false);
            
            if(playButtonClick)
                AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.SetActive(true);
            gameScreen.SetActive(false);
        }

        protected void RestartTimer() {
            gameTilemapAdapter.RestartTimer();
            
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": 0s";
        }

        protected void RestartCounters() {
            playerMoveText.text = Translator.GetTranslation("gamescene.game.playermoves.counter") + ": 0";
            boxMoveText.text = Translator.GetTranslation("gamescene.game.boxmoves.counter") + ": 0";
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": 0s";
        }

        public virtual void RestartLevel() {
            gameActive = true;
            RestartTimer();
            RestartCounters();
            levelFinishObject.SetActive(false);
            gameTilemapAdapter.RestartGame();
        }
        
        public void LoadLevel(int levelRegistryId) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            RestartTimer();
            RestartCounters();
            
            gameTilemapAdapter.LoadLevel(levelRegistryId);
        }

        public void LoadLevel(Level level) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            RestartTimer();
            RestartCounters();
            
            gameTilemapAdapter.LoadLevel(level);
        }
        
        protected virtual void FinishLevelEvent() {
            gameActive = false;
            levelFinishObject.SetActive(true);
        }

        private void PlayerMovedEvent(int playerMoveCount) {
            playerMoveText.text = Translator.GetTranslation("gamescene.game.playermoves.counter") + ": " + playerMoveCount;
        }

        private void BoxMovedEvent(int boxMoveCount) {
            boxMoveText.text = Translator.GetTranslation("gamescene.game.boxmoves.counter") + ": " + boxMoveCount;
        }

        private void TimePassedEvent(int time) {
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": " + time + "s";
        }
        #endregion
    }
}