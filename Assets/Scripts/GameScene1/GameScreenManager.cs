using System.Diagnostics.CodeAnalysis;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;

namespace GameScene1
{
    /// <summary>
    /// Manages everything that happens in the game on stage 1.
    /// </summary>
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
        /// <summary>
        /// Start sets all the events of <see cref="gameTilemapAdapter"/> to their respective methods.
        /// </summary>
        protected void Start() {
            gameTilemapAdapter.onLevelFinish = FinishLevelEvent;
            gameTilemapAdapter.onPlayerMoved = PlayerMovedEvent;
            gameTilemapAdapter.onBoxMoved = BoxMovedEvent;
            gameTilemapAdapter.onTimePassed = TimePassedEvent;
        }

		/// <summary>
        /// If the game is currently active and the window proportions change, WindowScaleUpdate is called on the <see cref="gameTilemapAdapter"/>
        /// to make sure the game properly fits on the screen. Also, when control keys are pressed on the keyboard, Update detects them and calls the
        /// proper methods.
        /// </summary>
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
            }
        }

        /// <summary>
        /// Leave game quits back to the <c>levelSelectionScreen</c>
        /// </summary>
        /// <param name="playButtonClick">This parameter determines if a button click should play when executing the method.</param>
        public virtual void LeaveGame(bool playButtonClick) {
            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
            gameActive = false;
            levelFinishObject.SetActive(false);
            
            if(playButtonClick)
                AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.SetActive(true);
            gameScreen.SetActive(false);
        }

        /// <summary>
        /// Restarts the in-game timer.
        /// </summary>
        private void RestartTimer() {
            gameTilemapAdapter.RestartTimer();
            
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": 0s";
        }

        /// <summary>
        /// Sets all in-game counters (the timer, player move counter and box move counter) to 0.
        /// </summary>
        private void RestartCounters() {
            playerMoveText.text = Translator.GetTranslation("gamescene.game.playermoves.counter") + ": 0";
            boxMoveText.text = Translator.GetTranslation("gamescene.game.boxmoves.counter") + ": 0";
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": 0s";
        }

        /// <summary>
        /// Restarts the level.
        /// </summary>
        public virtual void RestartLevel() {
            gameActive = true;
            RestartTimer();
            RestartCounters();
            levelFinishObject.SetActive(false);
            gameTilemapAdapter.RestartGame();
        }
        
        /// <summary>
        /// Loads a level corresponding to the registry ID.
        /// </summary>
        /// <param name="levelRegistryId">The ID in <see cref="LevelRegistry"/> that the level is loaded from.</param>
        public void LoadLevel(int levelRegistryId) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            RestartTimer();
            RestartCounters();
            
            gameTilemapAdapter.LoadLevel(levelRegistryId);
        }

        /// <summary>
        /// Loads the given level.
        /// </summary>
        /// <param name="level">The level that will be loaded.</param>
        public void LoadLevel(Level level) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            RestartTimer();
            RestartCounters();
            
            gameTilemapAdapter.LoadLevel(level);
        }
        
        /// <summary>
        /// Called by <see cref="gameTilemapAdapter"/> when the level is finished. It shows the <see cref="levelFinishObject"/>
        /// </summary>
        protected virtual void FinishLevelEvent() {
            gameActive = false;
            levelFinishObject.SetActive(true);
        }

        /// <summary>
        /// Called by the <see cref="gameTilemapAdapter"/> when the player moves. Updates the player move counter.
        /// </summary>
        /// <param name="playerMoveCount">The number of times the player has moved.</param>
        private void PlayerMovedEvent(int playerMoveCount) {
            playerMoveText.text = Translator.GetTranslation("gamescene.game.playermoves.counter") + ": " + playerMoveCount;
        }

        /// <summary>
        /// Called by the <see cref="gameTilemapAdapter"/> when a box moves. Updates the box move counter.
        /// </summary>
        /// <param name="boxMoveCount">The number of times a box has moved.</param>
        private void BoxMovedEvent(int boxMoveCount) {
            boxMoveText.text = Translator.GetTranslation("gamescene.game.boxmoves.counter") + ": " + boxMoveCount;
        }

        /// <summary>
        /// Called by the <see cref="gameTilemapAdapter"/> whenever a full second passes. Updates the time counter.
        /// </summary>
        /// <param name="time">The time in seconds that has passed since the start of the game.</param>
        private void TimePassedEvent(int time) {
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": " + time + "s";
        }
        #endregion
    }
}