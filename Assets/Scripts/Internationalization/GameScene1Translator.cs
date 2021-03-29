using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace Internationalization {
    public class GameScene1Translator : MonoBehaviour {
        [SerializeField] private TMP_Text exitGameButtonText;
        [SerializeField] private TMP_Text restartButtonText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text boxMovesText;
        [SerializeField] private TMP_Text playerMovesText;
        [SerializeField] private TMP_Text congratulationsText;
        [SerializeField] private TMP_Text levelFinishText;
        [SerializeField] private TMP_Text retryButtonText;
        [SerializeField] private TMP_Text backToMenuButtonText;

        [SerializeField] private TMP_Text randomLevelsCategoryText;
        [SerializeField] private TMP_Text easyLevelsCategoryText;
        [SerializeField] private TMP_Text mediumLevelsCategoryText;
        [SerializeField] private TMP_Text hardLevelsCategoryText;
        [SerializeField] private TMP_Text randomEasyLevelText;
        [SerializeField] private TMP_Text randomMediumLevelText;
        [SerializeField] private TMP_Text randomHardLevelText;
        [SerializeField] private TMP_Text exitLevelSelectionButtonText;
        [SerializeField] private TMP_Text playButtonText;
        
        public void Start() {
            UpdateTranslations();
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private void UpdateTranslations() {
            exitGameButtonText.text = Translator.GetTranslation("gamescene.game.exit.button");
            restartButtonText.text = Translator.GetTranslation("gamescene.game.restart.button");
            timerText.text = Translator.GetTranslation("gamescene.game.time.counter") + ": 0s";
            boxMovesText.text = Translator.GetTranslation("gamescene.game.boxmoves.counter") + ": 0";
            playerMovesText.text = Translator.GetTranslation("gamescene.game.playermoves.counter") + ": 0";
            congratulationsText.text = Translator.GetTranslation("gamescene.game.congratulations.text");
            levelFinishText.text = Translator.GetTranslation("gamescene.game.levelfinish.text");
            retryButtonText.text = Translator.GetTranslation("gamescene.game.retry.button");
            backToMenuButtonText.text = Translator.GetTranslation("gamescene.game.backtomenu.button");

            randomLevelsCategoryText.text = Translator.GetTranslation("gamescene.levelselection.randomlevels.category");
            easyLevelsCategoryText.text = Translator.GetTranslation("gamescene.levelselection.easylevels.category");
            mediumLevelsCategoryText.text = Translator.GetTranslation("gamescene.levelselection.mediumlevels.category");
            hardLevelsCategoryText.text = Translator.GetTranslation("gamescene.levelselection.hardlevels.category");
            randomEasyLevelText.text = Translator.GetTranslation("gamescene.levelselection.randomeasylevel.button");
            randomMediumLevelText.text = Translator.GetTranslation("gamescene.levelselection.randommediumlevel.button");
            randomHardLevelText.text = Translator.GetTranslation("gamescene.levelselection.randomhardlevel.button");
            exitLevelSelectionButtonText.text = Translator.GetTranslation("gamescene.levelselection.exit.button");
            playButtonText.text = Translator.GetTranslation("gamescene.levelselection.play.button");
        }
    }
}