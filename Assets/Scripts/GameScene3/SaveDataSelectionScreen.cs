using System;
using System.Collections.Generic;
using System.Linq;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene3 {
    /// <summary>
    /// Manages the saveDataSelectionScreen
    /// </summary>
    public class SaveDataSelectionScreen : MonoBehaviour {
        [SerializeField] private GameScreenManager3 gameScreenManager;
        [SerializeField] private GameObject gameScreen;
        
        [SerializeField] private TMP_Text exampleSaveEntry;
        [SerializeField] private GameObject scrollViewContent;
        [SerializeField] private Button deleteSaveButton;
        [SerializeField] private TMP_Text deleteSaveButtonText;
        [SerializeField] private Button loadSaveButton;
        [SerializeField] private TMP_Text loadSaveButtonText;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private ToggleGroup group;
        
        private bool initialized;
        
        public List<TMP_Text> buttonList;

        /// <summary>
        /// Initialize is called whenever the saveDataSelectionScreen is entered. It puts all the <see cref="SaveData2"/> objects from <see cref="SaveDataRegistry"/> into the screen and destroys any save entries that were left over from the previous time the screen was shown.
        /// </summary>
        public void Initialize() {
            foreach (var element in buttonList)
                Destroy(element.gameObject);
            buttonList = new List<TMP_Text>();
            exampleSaveEntry.gameObject.SetActive(true);
            
            levelSelectionScreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);

            for (var i = 0; i < SaveDataRegistry.GetSaveDataCount(); i++) {
                var saveDataElement = SaveDataRegistry.GetSaveData(i);
                
                var button = Instantiate(exampleSaveEntry, scrollViewContent.transform, true);
                button.text = saveDataElement.saveDataName + " - " + new DateTime(saveDataElement.timeCreated);
                button.name = i.ToString();
                buttonList.Add(button);
            }

            initialized = true;
            deleteSaveButton.interactable = false;
            deleteSaveButtonText.alpha = 0.5f;
            loadSaveButton.interactable = false;
            loadSaveButtonText.alpha = 0.5f;
            exampleSaveEntry.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called whenever a toggle on the is pressed. It allows the <see cref="deleteSaveButton"/> and <see cref="loadSaveButton"/> to be pressed.
        /// </summary>
        public void ToggleValueChanged() {
            if (initialized) {
                group.allowSwitchOff = false;
                deleteSaveButton.interactable = true;
                deleteSaveButtonText.alpha = 1f;
                loadSaveButton.interactable = true;
                loadSaveButtonText.alpha = 1f;
            }
        }
        
        /// <summary>
        /// Exits out of the saveDataSelectionScreen into the levelEditingScreen.
        /// </summary>
        public void Exit() {
            initialized = false;
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Deletes the currently selected save data.
        /// </summary>
        public void DeleteSelectedSave() {
            var selectedSaveID = Convert.ToInt32(group.ActiveToggles().First().transform.parent.name);

            for (var i = selectedSaveID + 1; i < SaveDataRegistry.GetSaveDataCount(); i++)
                buttonList[i].name = (Convert.ToInt32(buttonList[i].name) - 1).ToString();

            SaveDataRegistry.DeleteSaveData(selectedSaveID);
            Destroy(buttonList[selectedSaveID].gameObject);
            buttonList.RemoveAt(selectedSaveID);

            if (group.ActiveToggles().SingleOrDefault() == null) {
                deleteSaveButton.interactable = false;
                deleteSaveButtonText.alpha = 0.5f;
                loadSaveButton.interactable = false;
                loadSaveButtonText.alpha = 0.5f;
            }
        }

        /// <summary>
        /// Loads the <see cref="SaveData2"/> associated with the currently selected toggle.
        /// </summary>
        public void LoadSaveData() {
            var selectedSaveID = Convert.ToInt32(group.ActiveToggles().First().transform.parent.name);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);

            gameScreenManager.LoadSaveData(selectedSaveID);
            gameScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}