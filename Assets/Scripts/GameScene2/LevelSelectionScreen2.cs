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

/// <summary>
/// A namespace for any classes used on the second game scene.
/// </summary>
namespace GameScene2
{
	/// <summary>
	/// Manages everything that happens on the level selection screen in stage 2.
	/// </summary>
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

        /// <summary>
        /// A list of <see cref="TMP_Text"/> elements which are parents of <see cref="UI.ToggleSelectable"/> elements. This list contains all the pressable
        /// toggles on levelSelectionScreen2 that correspond to all levels in the <see cref="LevelRegistry"/>
        /// </summary>
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

        /// <summary>
        /// SelectProfile switches the screen to the <see cref="profileSelectionScreen"/>.
        /// </summary>
        public void SelectProfile() {
            profileSelectionScreen.SetActive(true);
            gameObject.SetActive(false);
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }

        /// <summary>
        /// Start is executed when the <see cref="LevelSelectionScreen2"/> is first loaded. It creates a clone of <see cref="UI.ToggleSelectable"/> for each level that is
        /// loaded in <see cref="LevelRegistry"/> and puts it all into the <see cref="scrollViewContent"/>. 
        /// </summary>
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

        /// <summary>
        /// Update is called every frame. It calls WindowScaleUpdate on <see cref="previewTilemapAdapter"/> if the proportions of the screen change
        /// so the preview tilemap always fits perfectly on the screen. It also exits to <c>MainScene</c> if the Escape key is pressed.
        /// </summary>
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

        /// <summary>
        /// Exit switches the scene to <c>MainScene</c> and plays the button clicked audio effect.
        /// </summary>
        public void Exit() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// ToggleValueChanged is called every time a toggle's value is changed. It shows the level corresponding to the pressed toggle on the <see cref="levelPreview"/>.
        /// It also allows the playButton to be pressed if it wasn't before because a level wasn't selected.
        /// </summary>
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

        /// <summary>
        /// Called when the play button is pressed. It starts playing the level associated to the currently selected toggle.
        /// </summary>
        public void StartGame() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            var levelID = activeToggle;
            
            gameScreenManager.LoadLevel(levelID);
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }

        /// <summary>
        /// Calls gameScreenManager.LoadSaveData() to load the save data saved in the currently selected profile.
        /// </summary>
        public void LoadSaveData() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);

            ((GameScreenManager2) gameScreenManager).LoadSaveData();
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }
    }
}