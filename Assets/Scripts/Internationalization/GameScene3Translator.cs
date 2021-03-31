using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace Internationalization {
	/// <summary>
	/// Translates all the <see cref="TMP_Text"/> objects on <c>GameScene3</c>.
	/// </summary>
    public class GameScene3Translator : MonoBehaviour {
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
        [SerializeField] private TMP_Text saveGameButtonText;
        [SerializeField] private TMP_Text noSaveNameWarning;
        [SerializeField] private TMP_Text saveNameInputFieldPlaceholder;
        [SerializeField] private TMP_Text saveCreationText;
        [SerializeField] private TMP_Text confirmSaveCreationButtonText;
        [SerializeField] private TMP_Text backFromSaveCreationButtonText;
        [SerializeField] private TMP_Text duplicateSaveNameWarning;
        
        [SerializeField] private TMP_Text exitLevelSelectionButtonText;
        [SerializeField] private TMP_Text playButtonText;
        [SerializeField] private TMP_Text loadSaveButtonText;
        [SerializeField] private TMP_Text editLevelText;
        [SerializeField] private TMP_Text levelSelectionScreenNoLevelNameWarning;
        [SerializeField] private TMP_Text renameLevelInputFieldPlaceholder;
        [SerializeField] private TMP_Text deleteLevelButtonText;
        [SerializeField] private TMP_Text changeLevelNameButtonText;
        [SerializeField] private TMP_Text backFromEditLevel;
        [SerializeField] private TMP_Text editLevelButtonText;
        [SerializeField] private TMP_Text newLevelButtonText;

        [SerializeField] private TMP_Text levelEditorDescriptionText;
        [SerializeField] private TMP_Text levelEditorText;
        [SerializeField] private TMP_Text brushSelectionText;
        [SerializeField] private TMP_Text levelHeightText;
        [SerializeField] private TMP_Text levelWidthText;
        [SerializeField] private TMP_Text levelEditorNoLevelNameWarning;
        [SerializeField] private TMP_Text levelEditorLevelNameInputFieldPlaceholder;
        [SerializeField] private TMP_Text levelEditorQuittingBackButtonText;
        [SerializeField] private TMP_Text levelEditorConfirmQuittingButtonText;
        [SerializeField] private TMP_Text quitWhileEditingWarning;
        [SerializeField] private TMP_Text exportLevelText;
        [SerializeField] private TMP_Text playtestLevelButtonText;
        [SerializeField] private TMP_Text quitLevelEditor;
        [SerializeField] private TMP_Text levelEditorQuitText;
        [SerializeField] private TMP_Text exportLevelButtonText;
        [SerializeField] private TMP_Text confirmExportLevelButton;
        [SerializeField] private TMP_Text levelExportDuplicateNameWarning;
        [SerializeField] private TMP_Text levelRenameDuplicateNameWarning;
        [SerializeField] private TMP_Text backFromExportLevelButtonText;

        [SerializeField] private TMP_Text exitSaveDataSelectionButtonText;
        [SerializeField] private TMP_Text deleteSaveDataButtonText;
        [SerializeField] private TMP_Text loadSaveDataButtonText;

        /// <summary>
        /// Calls <see cref="UpdateTranslations"/>.
        /// </summary>
        public void Start() {
            UpdateTranslations();
        }

        /// <summary>
        /// Updates the text of all <see cref="TMP_Text"/> object to their translations from the <see cref="Translator"/>.
        /// </summary>
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
            saveGameButtonText.text = Translator.GetTranslation("gamescene.game.savegame.button");
            noSaveNameWarning.text = Translator.GetTranslation("gamescene.game.nosavename.text");
            saveNameInputFieldPlaceholder.text = Translator.GetTranslation("gamescene.game.savename.inputfieldplaceholder");
            saveCreationText.text = Translator.GetTranslation("gamescene.game.savecreation.text");
            confirmSaveCreationButtonText.text = Translator.GetTranslation("gamescene.game.savecreationconfirm.button");
            backFromSaveCreationButtonText.text = Translator.GetTranslation("gamescene.game.savecreationback.button");
            duplicateSaveNameWarning.text = Translator.GetTranslation("gamescene.game.savecreationduplicatename.text");
            
            exitLevelSelectionButtonText.text = Translator.GetTranslation("gamescene.levelselection.exit.button");
            playButtonText.text = Translator.GetTranslation("gamescene.levelselection.play.button");
            loadSaveButtonText.text = Translator.GetTranslation("gamescene.levelselection.loadsave.button");
            editLevelText.text = Translator.GetTranslation("gamescene.levelselection.editlevel.text");
            levelSelectionScreenNoLevelNameWarning.text = Translator.GetTranslation("gamescene.levelselection.nolevelname.text");
            renameLevelInputFieldPlaceholder.text = Translator.GetTranslation("gamescene.levelselection.renamelevel.inputfieldplaceholder");
            deleteLevelButtonText.text = Translator.GetTranslation("gamescene.levelselection.deletelevel.button");
            changeLevelNameButtonText.text = Translator.GetTranslation("gamescene.levelselection.changelevelname.button");
            levelRenameDuplicateNameWarning.text = Translator.GetTranslation("gamescene.levelselection.duplicatelevelname.text");
            backFromEditLevel.text = Translator.GetTranslation("gamescene.levelselection.editlevelback");
            editLevelButtonText.text = Translator.GetTranslation("gamescene.levelselection.editlevel.button");
            newLevelButtonText.text = Translator.GetTranslation("gamescene.levelselection.newlevel.button");

            levelEditorDescriptionText.text = Translator.GetTranslation("gamescene.leveleditor.description.text");
            levelEditorText.text = Translator.GetTranslation("gamescene.leveleditor.leveleditor.text");
            brushSelectionText.text = Translator.GetTranslation("gamescene.leveleditor.brushselection.text");
            levelHeightText.text = Translator.GetTranslation("gamescene.leveleditor.levelheight.slider");
            levelWidthText.text = Translator.GetTranslation("gamescene.leveleditor.levelwidth.slider");
            levelEditorNoLevelNameWarning.text = Translator.GetTranslation("gamescene.leveleditor.nolevelname.text");
            levelEditorLevelNameInputFieldPlaceholder.text = Translator.GetTranslation( "gamescene.leveleditor.levelname.inputfieldplaceholder");
            levelEditorQuittingBackButtonText.text = Translator.GetTranslation("gamescene.leveleditor.quittingback.button");
            levelEditorConfirmQuittingButtonText.text = Translator.GetTranslation("gamescene.leveleditor.confirmquit.button");
            quitWhileEditingWarning.text = Translator.GetTranslation("gamescene.leveleditor.quitwarning.text");
            exportLevelText.text = Translator.GetTranslation("gamescene.leveleditor.exportlevel.text");
            playtestLevelButtonText.text = Translator.GetTranslation("gamescene.leveleditor.playtestlevel.button");
            quitLevelEditor.text = Translator.GetTranslation("gamescene.leveleditor.quit.button");
            levelEditorQuitText.text = Translator.GetTranslation("gamescene.leveleditor.quit.text");
            exportLevelButtonText.text = Translator.GetTranslation("gamescene.leveleditor.exportlevel.button");
            confirmExportLevelButton.text = Translator.GetTranslation("gamescene.leveleditor.confirmexportlevel.button");
            levelExportDuplicateNameWarning.text = Translator.GetTranslation("gamescene.leveleditor.duplicatename.text");
            backFromExportLevelButtonText.text = Translator.GetTranslation("gamescene.leveleditor.backfromexportlevel.button");

            exitSaveDataSelectionButtonText.text = Translator.GetTranslation("gamescene.savedataselection.exit.button");
            deleteSaveDataButtonText.text = Translator.GetTranslation("gamescene.savedataselection.deletesave.button");
            loadSaveDataButtonText.text = Translator.GetTranslation("gamescene.savedataselection.loadsave.button");
        }
    }
}