using System;
using System.Collections.Generic;
using System.Linq;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace GameScene3
{
    public class LevelSelectionScreen3 : MonoBehaviour {
        [SerializeField] private Image scrollViewContent;
        [SerializeField] private TMP_Text exampleLevelToggle;
        [SerializeField] private Button playButton;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameScreenManager3 gameScreenManager;
        
        [SerializeField] private LevelEditingScreen levelEditingScreen;
        [SerializeField] private GameObject editLevelSubscreen;
        [SerializeField] private Button editLevelButton;
        [SerializeField] private TMP_Text editLevelButtonText;
        [SerializeField] private LevelRegistry2Initializer levelRegistry2Initializer;

        [SerializeField] private GameObject noLevelNameWarning;
        [SerializeField] private GameObject levelRenameDuplicateNameWarning;
        [SerializeField] private TMP_InputField levelRenameField;
        [SerializeField] private Tilemap previewTilemap;

        [SerializeField] private SaveDataSelectionScreen saveDataSelectionScreen;
        
        public List<TMP_Text> buttonList;
        private bool levelSelected;
        private int screenHeight;
        private int screenWidth;
        private Toggle selectedToggle;
        private int activeToggle;
        private bool sceneInitialized;
        
        [SerializeField] private TilemapGameAdapter previewTilemapAdapter;
        
        [SerializeField] private TMP_Text playButtonText;
        private Level levelPreview;
	
        public void Start() {
            levelRegistry2Initializer.Start();
            
            for(var i = 0; i < LevelRegistry2.GetLevelCount(); i++) {
                var loadedLevel = LevelRegistry2.GetLevel(i);
                var button = Instantiate(exampleLevelToggle, scrollViewContent.transform, true);
                button.text = loadedLevel.levelName;
                button.name = i.ToString();

                buttonList.Add(button);
            }

            exampleLevelToggle.gameObject.SetActive(false);
            playButton.interactable = false;
            playButtonText.alpha = 0.5f;
            editLevelButton.interactable = false;
            editLevelButtonText.alpha = 0.5f;
            levelSelected = false;
            sceneInitialized = true;
            if (group.ActiveToggles().SingleOrDefault() != null)
                group.ActiveToggles().First().isOn = false;
        }

        public void AddNewLevel() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            exampleLevelToggle.gameObject.SetActive(true);
            
            var loadedLevel = LevelRegistry2.GetLevel(LevelRegistry2.GetLevelCount() - 1);
            var button = Instantiate(exampleLevelToggle, scrollViewContent.transform, true);
            button.text = loadedLevel.levelName;
            button.name = (LevelRegistry2.GetLevelCount() - 1).ToString();
            buttonList.Add(button);
            
            exampleLevelToggle.gameObject.SetActive(false);

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
                
                //TODO: This isn't a fast way to do this, but it works. If you have the time, please fix
                // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                if (group.ActiveToggles().SingleOrDefault() == null) {
                    playButton.interactable = false;
                    playButtonText.alpha = 0.5f;
                    editLevelButton.interactable = false;
                    editLevelButtonText.alpha = 0.5f;
                }
                else {
                    playButton.interactable = true;
                    playButtonText.alpha = 1f;
                    editLevelButton.interactable = true;
                    editLevelButtonText.alpha = 1f;
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
                    
                    editLevelButton.interactable = true;
                    editLevelButtonText.alpha = 1f;
                    levelSelected = true;
                }

                group.allowSwitchOff = false;
                selectedToggle = group.ActiveToggles().First();
                var toggleID = Convert.ToInt32(selectedToggle.transform.parent.name);
                activeToggle = toggleID;
                previewTilemapAdapter.LoadLevel(LevelRegistry2.GetLevel(toggleID));

                screenHeight = Screen.height;
                screenWidth = Screen.width;
            }
        }

        public void StartGame() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            var levelID = activeToggle;
            
            gameScreenManager.LoadLevel(LevelRegistry2.GetLevel(levelID));
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }

        public void CreateNewLevel() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            levelEditingScreen.gameObject.SetActive(true);
            levelEditingScreen.Start();
            gameObject.SetActive(false);
        }
        
        public void DeleteSelectedLevel() {
            var selectedLevelID = Convert.ToInt32(group.ActiveToggles().First().transform.parent.name);
            LevelRegistry2.DeleteLevel(selectedLevelID);
            Destroy(buttonList[selectedLevelID].gameObject);
            buttonList.RemoveAt(selectedLevelID);
            
            for (var i = selectedLevelID; i < buttonList.Count; i++) {
                buttonList[i].name = (Convert.ToInt32(buttonList[i].name) - 1).ToString();
            }
            
            if(buttonList.Count == 0)
                previewTilemap.ClearAllTiles();
            
            noLevelNameWarning.gameObject.SetActive(false);
            levelRenameDuplicateNameWarning.gameObject.SetActive(false);
            editLevelSubscreen.gameObject.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        public void ChangeName() {
            if (levelRenameField.text == "") {
                noLevelNameWarning.gameObject.SetActive(true);
                levelRenameDuplicateNameWarning.gameObject.SetActive(false);
            }
            else {
                if (!LevelRegistry2.LevelExists(levelRenameField.text)) {
                    var selectedProfileID = Convert.ToInt32(group.ActiveToggles().First().transform.parent.name);
                    noLevelNameWarning.gameObject.SetActive(false);
                    levelRenameDuplicateNameWarning.gameObject.SetActive(false);

                    LevelRegistry2.RenameLevel(selectedProfileID + 1, levelRenameField.text);
                    buttonList[selectedProfileID].text = levelRenameField.text;

                    editLevelSubscreen.SetActive(false);
                    AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
                }
                else {
                    levelRenameDuplicateNameWarning.gameObject.SetActive(true);
                    noLevelNameWarning.gameObject.SetActive(false);
                }
            }
        }

        public void BackFromEditing() {
            noLevelNameWarning.gameObject.SetActive(false);
            levelRenameDuplicateNameWarning.gameObject.SetActive(false);
            editLevelSubscreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        public void EditLevel() {
            levelRenameField.text = LevelRegistry2.GetLevel(Convert.ToInt32(group.ActiveToggles().First().transform.parent.name)).levelName;
            editLevelSubscreen.SetActive(true);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        public void LoadSavePressed() {
            saveDataSelectionScreen.gameObject.SetActive(true);
            saveDataSelectionScreen.Initialize();
            gameObject.SetActive(false);
        }
    }
}