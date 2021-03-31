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

/// <summary>
/// A namespace for any classes used on the third game scene.
/// </summary>
namespace GameScene3
{
	/// <summary>
	/// Manages everything that happens on the level selection screen in stage 3.
	/// </summary>
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

        /// <summary>
        /// A list of <see cref="TMP_Text"/> elements which are parents of <see cref="UI.ToggleSelectable"/> elements. This list contains the Toggles
        /// that correspond to all levels in the <see cref="LevelRegistry2"/>.
        /// </summary>
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

        /// <summary>
        /// Start is executed when the <see cref="LevelSelectionScreen"/> is first loaded. It creates a clone of <see cref="UI.ToggleSelectable"/> for each level that is
        /// loaded in <see cref="LevelRegistry2"/>. 
        /// </summary>
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

        /// <summary>
        /// Executed after a new level is exported in <see cref="levelEditingScreen"/>. It adds it to the list of levels available to select on the levelSelectionScreen.
        /// </summary>
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

        /// <summary>
        /// Update is called every frame.
        /// If the escape key is pressed it quits to the main scene.
        /// If no toggles are selected it prevents the player from pressing the play and edit buttons, and if they are it allows the player to do it.
        /// It calls WindowScaleUpdate on <see cref="previewTilemapAdapter"/> if the proportions of the screen change
        /// so the preview tilemap always fits perfectly on the screen.
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

        /// <summary>
        /// Called when the play button is pressed. It starts playing the level associated to the currently selected toggle.
        /// </summary>
        public void StartGame() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            var levelID = activeToggle;
            
            gameScreenManager.LoadLevel(LevelRegistry2.GetLevel(levelID));
            gameScreen.SetActive(true);
            levelSelectionScreen.gameObject.SetActive(false);
        }

        /// <summary>
        /// Switches to the <see cref="levelEditingScreen"/>.
        /// </summary>
        public void CreateNewLevel() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            levelEditingScreen.gameObject.SetActive(true);
            levelEditingScreen.Start();
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Deletes the currently selected level.
        /// </summary>
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
        
        /// <summary>
        /// If the <see cref="levelRenameField"/> isn't empty or the same as another level stored in <see cref="LevelRegistry2"/>, it changes the currently selected level's name to the value of the <see cref="levelRenameField"/>.
        /// </summary>
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

        /// <summary>
        /// Goes back from the <see cref="editLevelSubscreen"/> into the levelSelectionScreen.
        /// </summary>
        public void BackFromEditing() {
            noLevelNameWarning.gameObject.SetActive(false);
            levelRenameDuplicateNameWarning.gameObject.SetActive(false);
            editLevelSubscreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        /// <summary>
        /// Goes into the <see cref="editLevelSubscreen"/>
        /// </summary>
        public void EditLevel() {
            levelRenameField.text = LevelRegistry2.GetLevel(Convert.ToInt32(group.ActiveToggles().First().transform.parent.name)).levelName;
            editLevelSubscreen.SetActive(true);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        /// <summary>
        /// Called when the "Load Save" button is pressed. It switches the open screen to <see cref="saveDataSelectionScreen"/>.
        /// </summary>
        public void LoadSavePressed() {
            saveDataSelectionScreen.gameObject.SetActive(true);
            saveDataSelectionScreen.Initialize();
            gameObject.SetActive(false);
        }
    }
}