using System.Collections;
using GameScene1;
using Internationalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene2 {
    public class GameScreenManager2 : GameScreenManager {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button loadSaveDataButton;
        [SerializeField] private TMP_Text loadSaveDataButtonText;
        [SerializeField] private TMP_Text savedText;
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void FinishLevelEvent() {
            var score = (int) ((TilemapGameAdapter2) gameTilemapAdapter).newLevelScore;
            scoreText.text = Translator.GetTranslation("gamescene.game.score.counter") + score + "/100";
            if (((TilemapGameAdapter2) gameTilemapAdapter).isHighScore)
                scoreText.text += " " + Translator.GetTranslation("gamescene.game.newrecord.text");
            
            base.FinishLevelEvent();
        }
        
        public void LoadSaveData() {
            Start();
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            
            ((TilemapGameAdapter2) gameTilemapAdapter).LoadSaveData();
        }

        public void SaveGameData() {
            loadSaveDataButton.interactable = true;
            loadSaveDataButtonText.alpha = 1f;
            
            ((TilemapGameAdapter2) gameTilemapAdapter).SaveGameData();
            StartCoroutine(ShowSavedText());
        }

        private IEnumerator ShowSavedText() {
            savedText.alpha = 1f;

            while (savedText.alpha > 0f) {
                savedText.alpha -= Time.deltaTime;
                yield return null;
            }
        }

        public override void LeaveGame(bool playButtonClick) {
            StopAllCoroutines();
            savedText.alpha = 0f;
            
            base.LeaveGame(playButtonClick);
        }
    }
}