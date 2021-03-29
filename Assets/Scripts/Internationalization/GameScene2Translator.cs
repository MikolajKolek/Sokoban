using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace Internationalization {
    public class GameScene2Translator : MonoBehaviour {
        [SerializeField] private TMP_Text exitGameButtonText;
        [SerializeField] private TMP_Text restartButtonText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text boxMovesText;
        [SerializeField] private TMP_Text playerMovesText;
        [SerializeField] private TMP_Text congratulationsText;
        [SerializeField] private TMP_Text levelFinishText;
        [SerializeField] private TMP_Text retryButtonText;
        [SerializeField] private TMP_Text backToMenuButtonText;
        [SerializeField] private TMP_Text savedGameText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text saveGameButtonText;
        
        [SerializeField] private TMP_Text exitLevelSelectionButtonText;
        [SerializeField] private TMP_Text playButtonText;
        [SerializeField] private TMP_Text levelSelectionSelectProfileButtonText;
        [SerializeField] private TMP_Text loadSaveButtonText;
        [SerializeField] private TMP_Text leaderboardButtonText;

        [SerializeField] private TMP_Text profileCreationInputFieldPlaceholder;
        [SerializeField] private TMP_Text profileEditingInputFieldPlaceholder;
        [SerializeField] private TMP_Text profileCreationNoNameWarning;
        [SerializeField] private TMP_Text profileEditingNoNameWarning;
        [SerializeField] private TMP_Text profileCreationText;
        [SerializeField] private TMP_Text profileEditingText;
        [SerializeField] private TMP_Text profileEditingBackButtonText;
        [SerializeField] private TMP_Text deleteProfileButtonText;
        [SerializeField] private TMP_Text profileSelectionExitButtonText;
        [SerializeField] private TMP_Text newProfileButtonText;
        [SerializeField] private TMP_Text newProfileBackButtonText;
        [SerializeField] private TMP_Text editProfileButtonText;
        [SerializeField] private TMP_Text changeProfileNameButtonText;
        [SerializeField] private TMP_Text confirmProfileCreationButtonText;
        [SerializeField] private TMP_Text profileSelectionSelectProfileButtonText;
        [SerializeField] private TMP_Text duplicateProfileNameWarningCreation;
        [SerializeField] private TMP_Text duplicateProfileNameWarningEditing;
        
        [SerializeField] private TMP_Text leaderboardExitButtonText;
        [SerializeField] private TMP_Text leaderboardSeeScoresButtonText;
        [SerializeField] private TMP_Text individualScoresBackButtonText;

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
            savedGameText.text = Translator.GetTranslation("gamescene.game.savedgame.text");
            scoreText.text = Translator.GetTranslation("gamescene.game.score.counter") + ": 0";
            saveGameButtonText.text = Translator.GetTranslation("gamescene.game.savegame.button");
            
            exitLevelSelectionButtonText.text = Translator.GetTranslation("gamescene.levelselection.exit.button");
            playButtonText.text = Translator.GetTranslation("gamescene.levelselection.play.button");
            levelSelectionSelectProfileButtonText.text = Translator.GetTranslation("gamescene.levelselection.selectprofile.button");
            loadSaveButtonText.text = Translator.GetTranslation("gamescene.levelselection.loadsave.button");
            leaderboardButtonText.text = Translator.GetTranslation("gamescene.levelselection.leaderboard.button");
            
            profileCreationInputFieldPlaceholder.text = Translator.GetTranslation("gamescene.profiles.profilecreation.inputfieldplaceholder");
            profileEditingInputFieldPlaceholder.text = Translator.GetTranslation("gamescene.profiles.profileediting.inputfieldplaceholder");
            profileCreationNoNameWarning.text = Translator.GetTranslation("gamescene.profiles.profilecreationnonamewarning.text");
            profileEditingNoNameWarning.text = Translator.GetTranslation("gamescene.profiles.profileeditingnonamewarning.text");
            profileCreationText.text = Translator.GetTranslation("gamescene.profiles.profilecreation.text");
            profileEditingText.text = Translator.GetTranslation("gamescene.profiles.profileediting.text");
            profileEditingBackButtonText.text = Translator.GetTranslation("gamescene.profiles.profileeditingback.button");
            deleteProfileButtonText.text = Translator.GetTranslation("gamescene.profiles.deleteprofile.button");
            profileSelectionExitButtonText.text = Translator.GetTranslation("gamescene.profiles.exit.button");
            newProfileButtonText.text = Translator.GetTranslation("gamescene.profiles.newprofile.button");
            newProfileBackButtonText.text = Translator.GetTranslation("gamescene.profiles.newprofileback.button");
            editProfileButtonText.text = Translator.GetTranslation("gamescene.profiles.editprofile.button");
            changeProfileNameButtonText.text = Translator.GetTranslation("gamescene.profiles.changename.button");
            confirmProfileCreationButtonText.text = Translator.GetTranslation("gamescene.profiles.confirmchangename.button");
            profileSelectionSelectProfileButtonText.text = Translator.GetTranslation("gamescene.profiles.selectprofile.button");
            duplicateProfileNameWarningCreation.text = Translator.GetTranslation("gamescene.profiles.duplicateprofilenamecreation.text");
            duplicateProfileNameWarningEditing.text = Translator.GetTranslation("gamescene.profiles.duplicateprofilenameediting.text");

            leaderboardExitButtonText.text = Translator.GetTranslation("gamescene.leaderboard.exit.button");
            leaderboardSeeScoresButtonText.text = Translator.GetTranslation("gamescene.leaderboard.seescores.button");
            individualScoresBackButtonText.text = Translator.GetTranslation("gamescene.leaderboard.individualscoresback.button");
        }
    }
}