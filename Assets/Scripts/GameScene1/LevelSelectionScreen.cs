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

namespace GameScene1
{
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
        
        public List<TMP_Text> easyButtonList;
        public List<TMP_Text> mediumButtonList;
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
	
        public void Start() {
            for(var i = 0; i < LevelRegistry.GetLevelCount(); i++) {
                var loadedLevel = LevelRegistry.GetLevel(i);
                var button = Instantiate(exampleLevelToggle);
                button.text = Translator.GetTranslation("gamescene.levelselection.level.text") + " " +  loadedLevel.levelName;
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