using System.Collections;
using GameScene1;
using Internationalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene2 {
	/// <summary>
	/// Manages everything that happens in the game on stage 2.
	/// </summary>
    public class GameScreenManager2 : GameScreenManager {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button loadSaveDataButton;
        [SerializeField] private TMP_Text loadSaveDataButtonText;
        [SerializeField] private TMP_Text savedText;
        
        /// <summary>
        /// Overrides the <see cref="GameScreenManager"/>'s FinishLevelEvent to add a score text to the levelFinishObject.
        /// </summary>
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void FinishLevelEvent() {
            var score = (int) ((TilemapGameAdapter2) gameTilemapAdapter).newLevelScore;
            scoreText.text = Translator.GetTranslation("gamescene.game.score.counter") + score + "/100";
            if (((TilemapGameAdapter2) gameTilemapAdapter).isHighScore)
                scoreText.text += " " + Translator.GetTranslation("gamescene.game.newrecord.text");
            
            base.FinishLevelEvent();
        }

        /// <summary>
        /// Calls gameTilemapAdapter.LoadSaveData() to load the save data from the currently selected profile.
        /// </summary>
        public void LoadSaveData() {
            Start();
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            
            ((TilemapGameAdapter2) gameTilemapAdapter).LoadSaveData();
        }

        /// <summary>
        /// Calls gameTilemapAdapter.SaveGameData() to save save data to the currently selected profile and also shows the text "Saved" on the screen by calling <see cref="ShowSavedText"/>.
        /// </summary>
        public void SaveGameData() {
            loadSaveDataButton.interactable = true;
            loadSaveDataButtonText.alpha = 1f;
            
            ((TilemapGameAdapter2) gameTilemapAdapter).SaveGameData();
            StartCoroutine(ShowSavedText());
        }

        /// <summary>
        /// This coroutine shows the text "Saved" on the screen and then fades it away over the course of a second.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowSavedText() {
            savedText.alpha = 1f;

            while (savedText.alpha > 0f) {
                savedText.alpha -= Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// LeaveGame overriders the LeaveGame method of <see cref="GameScreenManager"/> to make sure the SavedText from <see cref="ShowSavedText"/> doesn't remain on screen.
        /// </summary>
        /// <param name="playButtonClick"></param>
        public override void LeaveGame(bool playButtonClick) {
            StopAllCoroutines();
            savedText.alpha = 0f;
            
            base.LeaveGame(playButtonClick);
        }
    }
}