using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

/// <summary>
/// A namespace for any classes used on the first game scene.
/// </summary>
namespace GameScene1
{
    /// <summary>
    /// Manages everything that happens on the level selection screen in stage 1.
    /// </summary>
    public class LevelSelectionScreen : MonoBehaviour {
        [SerializeField] private TMP_Text exampleLevelToggle;
        [SerializeField] private Image easyItemContent;
        [SerializeField] private Image mediumItemContent;
        [SerializeField] private Image hardItemContent;
        [SerializeField] private Button playButton;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameScreenManager gameScreenManager;
        
        /// <summary>
        /// A list of <see cref="TMP_Text"/> elements which are parents of <see cref="UI.ToggleSelectable"/> elements. This list contains the Toggles
        /// that correspond to easy levels in the <see cref="LevelRegistry"/>.
        /// </summary>
        public List<TMP_Text> easyButtonList;
        /// <summary>
        /// A list of <see cref="TMP_Text"/> elements which are parents of <see cref="UI.ToggleSelectable"/> elements. This list contains the Toggles
        /// that correspond to medium levels in the <see cref="LevelRegistry"/>.
        /// </summary>
        public List<TMP_Text> mediumButtonList;
        /// <summary>
        /// A list of <see cref="TMP_Text"/> elements which are parents of <see cref="UI.ToggleSelectable"/> elements. This list contains the Toggles
        /// that correspond to hard levels in the <see cref="LevelRegistry"/>.
        /// </summary>
        public List<TMP_Text> hardButtonList;
        private bool levelSelected;
        private int screenHeight;
        private int screenWidth;
        private Toggle selectedToggle;
        private int cyclingLevelType;
        private int activeToggle;
        
        [SerializeField] private TilemapGameAdapter previewTilemapAdapter;
        
        [SerializeField] private TMP_Text playButtonText;
        private Level levelPreview;
	
        /// <summary>
        /// Start is executed when the <see cref="LevelSelectionScreen"/> is first loaded. It creates a clone of <see cref="UI.ToggleSelectable"/> for each level that is
        /// loaded in <see cref="LevelRegistry"/> and puts it under the correct difficulty category on the screen. 
        /// </summary>
        public void Start() {
            for(var i = 0; i < LevelRegistry.GetLevelCount(); i++) {
                var loadedLevel = LevelRegistry.GetLevel(i);
                var button = Instantiate(exampleLevelToggle);
                button.text = Translator.GetTranslation("gamescene.levelselection.level.text") + " " +  loadedLevel.levelName;
                //The name is set to the level's id in the LevelRegistry, this way later on we can identify which Level this Toggle is associated with.
                button.name = i.ToString();

                switch (loadedLevel.difficultyLevel)
                {
                    case Level.Difficulty.Easy:
                        button.transform.SetParent(easyItemContent.transform);
                        easyButtonList.Add(button);
                        break;
                    case Level.Difficulty.Medium:
                        button.transform.SetParent(mediumItemContent.transform);
                        mediumButtonList.Add(button);
                        break;
                    case Level.Difficulty.Hard:
                        button.transform.SetParent(hardItemContent.transform);
                        hardButtonList.Add(button);
                        break;
                    case Level.Difficulty.None:
                        Debug.LogError("Invalid difficulty in level " + loadedLevel.levelName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            exampleLevelToggle.gameObject.SetActive(false);
            playButton.interactable = false;
            playButtonText.alpha = 0.5f;
            levelSelected = false;
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
            if(group.AnyTogglesOn()) {
                if(playButton.interactable == false) {
                    playButton.interactable = true;
                    playButtonText.alpha = 1f;
                    levelSelected = true;
                }

                selectedToggle = group.ActiveToggles().First();
                switch (selectedToggle.transform.parent.name) {
                    case "RandomEasyLevelText":
                        if (cyclingLevelType != 1) {
                            StopAllCoroutines();
                            StartCoroutine(CycleLevels(easyButtonList));
                            cyclingLevelType = 1;
                            activeToggle = -1;
                        }
                        
                        break;
                    case "RandomMediumLevelText":
                        if (cyclingLevelType != 2) {
                            StopAllCoroutines();
                            StartCoroutine(CycleLevels(mediumButtonList));
                            cyclingLevelType = 2;
                            activeToggle = -2;
                        }

                        break;
                    case "RandomHardLevelText":
                        if (cyclingLevelType != 3) {
                            StopAllCoroutines();
                            StartCoroutine(CycleLevels(hardButtonList));
                            cyclingLevelType = 3;
                            activeToggle = -3;
                        }

                        break;
                    default:
                        if (cyclingLevelType != 0) {
                            StopAllCoroutines();
                            cyclingLevelType = 0;
                        }
                        
                        var toggleID = Convert.ToInt32(selectedToggle.transform.parent.name);
                        activeToggle = toggleID;
                        previewTilemapAdapter.LoadLevel(toggleID);
                        break;
                }
                
                screenHeight = Screen.height;
                screenWidth = Screen.width;
            }
        }

        /// <summary>
        /// This coroutine cycles through <see cref="easyButtonList"/>, <see cref="mediumButtonList"/> or <see cref="hardButtonList"/> and displays all the
        /// easy, medium or hard levels in order on the <see cref="levelPreview"/>.
        /// </summary>
        /// <param name="buttonList"><see cref="easyButtonList"/>, <see cref="mediumButtonList"/> or <see cref="hardButtonList"/></param>
        private IEnumerator CycleLevels(IReadOnlyList<TMP_Text> buttonList) {
            var selectedIndex = 0;
            var time = 0f;

            if (buttonList.Count == 1) {
                previewTilemapAdapter.LoadLevel(Convert.ToInt32(buttonList[0].name));
                yield break;
            }

            while (true) {
                time -= Time.deltaTime;
                if (time <= 0) {
                    time = 1f;
                    previewTilemapAdapter.LoadLevel(Convert.ToInt32(buttonList[selectedIndex].name));
                    
                    selectedIndex++;
                    if (selectedIndex > buttonList.Count - 1)
                        selectedIndex = 0;
                }

                yield return null;
            }
        }
        
        /// <summary>
        /// Called when the play button is pressed. It starts playing the level associated to the currently selected toggle.
        /// </summary>
        public void StartGame() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            int levelID;
            
            var r = new System.Random();
            switch (activeToggle) {
                case -1:
                    levelID = Convert.ToInt32(easyButtonList[r.Next(0, easyButtonList.Count - 1)].name);
                    break;
                case -2:
                    levelID = Convert.ToInt32(mediumButtonList[r.Next(0, mediumButtonList.Count - 1)].name);
                    break;
                case -3:
                    levelID = Convert.ToInt32(hardButtonList[r.Next(0, hardButtonList.Count - 1)].name);
                    break;
                default:
                    levelID = activeToggle;
                    break;
            }
            
            gameScreenManager.LoadLevel(levelID);
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        } 
    }
}