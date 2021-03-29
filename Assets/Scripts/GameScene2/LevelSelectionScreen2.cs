using System;
using System.Collections.Generic;
using System.Linq;
using GameScene1;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace GameScene2
{
    public class LevelSelectionScreen2 : MonoBehaviour {
        [SerializeField] private Image scrollViewContent;
        [SerializeField] private TMP_Text exampleLevelToggle;
        [SerializeField] private Button playButton;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameScreenManager gameScreenManager;
        [SerializeField] private GameObject profileSelectionScreen;
        [SerializeField] private Button loadSaveDataButton;
        [SerializeField] private TMP_Text loadSaveDataButtonText;

        public List<TMP_Text> levelButtonList;
        private bool levelSelected;
        private int screenHeight;
        private int screenWidth;
        private Toggle selectedToggle;
        private int activeToggle;
        private bool sceneInitialized;
        
        [SerializeField] private TilemapGameAdapter previewTilemapAdapter;
        
        [SerializeField] private TMP_Text playButtonText;
        private Level levelPreview;

        public void SelectProfile() {
            profileSelectionScreen.SetActive(true);
            gameObject.SetActive(false);
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        public void Start() {
            for(var i = 0; i < LevelRegistry.GetLevelCount(); i++) {
                var loadedLevel = LevelRegistry.GetLevel(i);
                var button = Instantiate(exampleLevelToggle, scrollViewContent.transform, true);
                button.text = Translator.GetTranslation("gamescene.levelselection.level.text") + " " +  loadedLevel.levelName;
                button.name = i.ToString();

                levelButtonList.Add(button);
            }

            exampleLevelToggle.gameObject.SetActive(false);

            if (ProfileManager.selectedProfile.savedGame.isEmpty) {
                loadSaveDataButton.interactable = false;
                loadSaveDataButtonText.alpha = 0.5f;
            }
            
            playButton.interactable = false;
            playButtonText.alpha = 0.5f;
            levelSelected = false;
            sceneInitialized = true;
            if (group.ActiveToggles().SingleOrDefault() != null)
                group.ActiveToggles().First().isOn = false;
        }

        public void Update() {
            if (levelSelectionScreen.activeSelf) {
                if (levelSelected && (Screen.height != screenHeight || Screen.width != screenWidth)) {
                    screenHeight = Screen.height;
                    screenWidth = Screen.width;

                    previewTilemapAdapter.WindowScaleUpdate();
                }
                
                if (Input.GetKeyDown(KeyCode.Escape)) { 
                    SceneManager.LoadScene("MainScene");
                }
            }
        }

        public void Exit() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            SceneManager.LoadScene("MainScene");
        }

        public void ToggleValueChanged() {
            if(group.AnyTogglesOn() && sceneInitialized) {
                if(playButton.interactable == false) {
                    playButton.interactable = true;
                    playButtonText.alpha = 1f;
                    levelSelected = true;
                }

                group.allowSwitchOff = false;
                selectedToggle = group.ActiveToggles().First();
                var toggleID = Convert.ToInt32(selectedToggle.transform.parent.name);
                activeToggle = toggleID;
                previewTilemapAdapter.LoadLevel(toggleID);

                screenHeight = Screen.height;
                screenWidth = Screen.width;
            }
        }

        public void StartGame() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            var levelID = activeToggle;
            
            gameScreenManager.LoadLevel(levelID);
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }

        public void LoadSaveData() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);

            ((GameScreenManager2) gameScreenManager).LoadSaveData();
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }
    }
}