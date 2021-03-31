using System.Collections;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;

namespace GameScene3 {
	/// <summary>
	/// Manages everything that happens in the game on stage 3.
	/// </summary>
    public class GameScreenManager3 : GameScreenManager {
        [SerializeField] private GameObject levelEditingScreen;
        [SerializeField] private TMP_Text savedText;
        [SerializeField] private GameObject saveCreationSubscreen;
        [SerializeField] private GameObject noSaveNameWarningText;
        [SerializeField] private TMP_InputField saveNameField;
        [SerializeField] private GameObject duplicateSaveNameWarningText;
        [SerializeField] private GameObject saveGameButton;

        private bool inEditMode;

        /// <summary>
        /// Enters the gameScene into edit mode, which disables saving and makes it so quitting or finishing the level will put the user back onto <see cref="levelEditingScreen"/>.
        /// </summary>
        public void EnterEditMode() {
            inEditMode = true;
            saveGameButton.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Overrides the FinishLevelEvent to makes sure that in edit mode it puts the user back onto <see cref="levelEditingScreen"/>.
        /// </summary>
        protected override void FinishLevelEvent() {
            if (inEditMode) {
                ((TilemapGameAdapter3) gameTilemapAdapter).EnterGameMode();
                AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
                inEditMode = false;
                saveGameButton.gameObject.SetActive(true);
                levelEditingScreen.SetActive(true);
                gameObject.SetActive(false);
            }
            else 
                base.FinishLevelEvent();
        }

        /// <summary>
        /// Overrides LeaveGame to makes sure that in edit mode it puts the user back onto <see cref="levelEditingScreen"/>.
        /// </summary>
        public override void LeaveGame(bool playButtonClick) {
            StopAllCoroutines();
            savedText.alpha = 0f;
            
            if (inEditMode) {
                AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
                gameActive = false;
                levelFinishObject.SetActive(false);

                inEditMode = false;
                saveGameButton.gameObject.SetActive(true);
                
                if(playButtonClick)
                    AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
                levelEditingScreen.SetActive(true);
                gameObject.SetActive(false);
            }
            else
                base.LeaveGame(playButtonClick);
        }
        
        /// <summary>
        /// It shows the text "Saved" on the screen and then smoothly fades it over the course of a second.
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
        /// If the <see cref="saveNameField"/> isn't empty and doesn't contain the name of a save state that already exists, it calls gameTilemapAdapter.SaveGameData to save the game data in the <see cref="SaveDataRegistry"/>
        /// </summary>
        public void SaveGameData() {
            if (saveNameField.text == "") {
                noSaveNameWarningText.gameObject.SetActive(true);
                duplicateSaveNameWarningText.gameObject.SetActive(false);
            }
            else {
                if (!SaveDataRegistry.SaveGameExists(saveNameField.text)) {
                    noSaveNameWarningText.gameObject.SetActive(false);
                    duplicateSaveNameWarningText.gameObject.SetActive(false);
                    
                    ((TilemapGameAdapter3) gameTilemapAdapter).SaveGameData(saveNameField.text);
                    StartCoroutine(ShowSavedText());
                    SaveCreationSubscreenBack();
                }
                else {
                    noSaveNameWarningText.gameObject.SetActive(false);
                    duplicateSaveNameWarningText.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Shows the save creation subscreen.
        /// </summary>
        public void ShowSaveCreationSubscreen() {
            ((TilemapGameAdapter3) gameTilemapAdapter).SwitchTimerState(false);
            gameActive = false;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            saveNameField.text = "";
            noSaveNameWarningText.SetActive(false);
            duplicateSaveNameWarningText.SetActive(false);
            saveCreationSubscreen.SetActive(true);
        }

        /// <summary>
        /// Backs away from the save creation subscreen into the gameScreen.
        /// </summary>
        public void SaveCreationSubscreenBack() {
            ((TilemapGameAdapter3) gameTilemapAdapter).SwitchTimerState(true);
            gameActive = true;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            saveCreationSubscreen.SetActive(false);
        }

        /// <summary>
        /// Calls gameTilemapAdapter.LoadSaveData to load the save data stored at the given ID
        /// </summary>
        /// <param name="saveDataRegistryID">The ID in the <see cref="SaveDataRegistry"/> of the save state that you want to load.</param>
        public void LoadSaveData(int saveDataRegistryID) {
            Start();
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            
            ((TilemapGameAdapter3) gameTilemapAdapter).LoadSaveData(saveDataRegistryID);
        }
    }
}