using System.Collections;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;

namespace GameScene3 {
    public class GameScreenManager3 : GameScreenManager {
        [SerializeField] private GameObject levelEditingScreen;
        [SerializeField] private TMP_Text savedText;
        [SerializeField] private GameObject saveCreationSubscreen;
        [SerializeField] private GameObject noSaveNameWarningText;
        [SerializeField] private TMP_InputField saveNameField;
        [SerializeField] private GameObject duplicateSaveNameWarningText;
        
        private bool inEditMode;

        public void EnterEditMode() {
            inEditMode = true;
        }
        
        protected override void FinishLevelEvent() {
            if (inEditMode) {
                ((TilemapGameAdapter3) gameTilemapAdapter).EnterGameMode();
                AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
                inEditMode = false;
                levelEditingScreen.SetActive(true);
                gameObject.SetActive(false);
            }
            else 
                base.FinishLevelEvent();
        }

        public override void LeaveGame(bool playButtonClick) {
            StopAllCoroutines();
            savedText.alpha = 0f;
            
            if (inEditMode) {
                AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);
                gameActive = false;
                levelFinishObject.SetActive(false);

                if(playButtonClick)
                    AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
                levelEditingScreen.SetActive(true);
                gameObject.SetActive(false);
            }
            else
                base.LeaveGame(playButtonClick);
        }
        
        private IEnumerator ShowSavedText() {
            savedText.alpha = 1f;

            while (savedText.alpha > 0f) {
                savedText.alpha -= Time.deltaTime;
                yield return null;
            }
        }
        
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

        public void ShowSaveCreationSubscreen() {
            ((TilemapGameAdapter3) gameTilemapAdapter).SwitchTimerState(false);
            gameActive = false;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            saveNameField.text = "";
            noSaveNameWarningText.SetActive(false);
            duplicateSaveNameWarningText.SetActive(false);
            saveCreationSubscreen.SetActive(true);
        }

        public void SaveCreationSubscreenBack() {
            ((TilemapGameAdapter3) gameTilemapAdapter).SwitchTimerState(true);
            gameActive = true;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            saveCreationSubscreen.SetActive(false);
        }
        
        public void LoadSaveData(int saveDataRegistryID) {
            Start();
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            gameActive = true;
            
            ((TilemapGameAdapter3) gameTilemapAdapter).LoadSaveData(saveDataRegistryID);
        }
    }
}