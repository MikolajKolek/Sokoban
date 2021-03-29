using System;
using System.Collections.Generic;
using System.Linq;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene2 {
    public class ProfileSelectionScreen : MonoBehaviour {
        #region Serialized variables
        [SerializeField] private TMP_Text exampleProfileToggle;
        [SerializeField] private GameObject screenViewContent;
        [SerializeField] private Button selectButton;
        [SerializeField] private TMP_Text selectButtonText;
        [SerializeField] private ToggleGroup profileToggleGroup;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private TMP_InputField profileNameField;
        [SerializeField] private GameObject profileCreationScreen;
        [SerializeField] private TMP_Text noProfileNameWarningCreation;
        [SerializeField] private TMP_InputField profileRenameField;
        [SerializeField] private GameObject profileEditingScreen;
        [SerializeField] private TMP_Text noProfileNameWarningEditing;
        [SerializeField] private Button editProfileButton;
        [SerializeField] private TMP_Text editProfileText;

        [SerializeField] private TMP_Text duplicateProfileNameWarningCreation;
        [SerializeField] private TMP_Text duplicateProfileNameWarningEditing;
        #endregion

        #region Private variables
        private Toggle selectedToggle;
        private bool setupFinished;
        #endregion

        public List<TMP_Text> profileButtonList;

        public void Start() {
            for (var i = 0; i < ProfileManager.GetProfileCount(); i++) {
                var currentProfile = ProfileManager.GetProfile(i);
                var currentProfileToggle = Instantiate(exampleProfileToggle, screenViewContent.transform, true);

                currentProfileToggle.text = currentProfile.name;
                currentProfileToggle.name = i.ToString();
                profileButtonList.Add(currentProfileToggle);
            }

            exampleProfileToggle.gameObject.SetActive(false);
            if (profileToggleGroup.ActiveToggles().SingleOrDefault() != null)
                profileToggleGroup.ActiveToggles().First().isOn = false;
            selectButton.interactable = false;
            selectButtonText.alpha = 0.5f;
            editProfileButton.interactable = false;
            editProfileText.alpha = 0.5f;
            setupFinished = true;
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeSelf) {
                SceneManager.LoadScene("MainScene");
            }

            //TODO: This isn't a fast way to do this, but it works. If you have the time, please fix
            if (profileToggleGroup.ActiveToggles().SingleOrDefault() == null) {
                selectButton.interactable = false;
                selectButtonText.alpha = 0.5f;
                editProfileButton.interactable = false;
                editProfileText.alpha = 0.5f;
            }
            else {
                selectButton.interactable = true;
                selectButtonText.alpha = 1f;
                editProfileButton.interactable = true;
                editProfileText.alpha = 1f;
            }
        }

        public void ToggleValueChanged() {
            if (profileToggleGroup.AnyTogglesOn() && setupFinished) {
                profileToggleGroup.allowSwitchOff = false;
            }
        }

        public void DeleteSelectedProfile() {
            var selectedProfileID = Convert.ToInt32(profileToggleGroup.ActiveToggles().First().transform.parent.name);
            ProfileManager.DeleteProfile(selectedProfileID);
            Destroy(profileButtonList[selectedProfileID].gameObject);
            profileButtonList.RemoveAt(selectedProfileID);
            
            for (var i = selectedProfileID; i < profileButtonList.Count; i++) {
                profileButtonList[i].name = (Convert.ToInt32(profileButtonList[i].name) - 1).ToString();
            }

            noProfileNameWarningEditing.gameObject.SetActive(false);
            duplicateProfileNameWarningEditing.gameObject.SetActive(false);
            profileEditingScreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }

        public void ChangeName() {
            if (profileRenameField.text == "") {
                noProfileNameWarningEditing.gameObject.SetActive(true);
                duplicateProfileNameWarningEditing.gameObject.SetActive(false);
            }
            else {
                if (!ProfileManager.ProfileExists(profileRenameField.text)) {
                    var selectedProfileID =
                        Convert.ToInt32(profileToggleGroup.ActiveToggles().First().transform.parent.name);
                    noProfileNameWarningCreation.gameObject.SetActive(false);
                    duplicateProfileNameWarningEditing.gameObject.SetActive(false);

                    ProfileManager.RenameProfile(selectedProfileID, profileRenameField.text);
                    profileButtonList[selectedProfileID].text = profileRenameField.text;

                    profileEditingScreen.SetActive(false);
                    AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
                }
                else {
                    noProfileNameWarningEditing.gameObject.SetActive(false);
                    duplicateProfileNameWarningEditing.gameObject.SetActive(true);
                }
            }
        }

        public void BackFromEditing() {
            noProfileNameWarningEditing.gameObject.SetActive(false);
            duplicateProfileNameWarningEditing.gameObject.SetActive(false);
            
            profileEditingScreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }

        public void BackFromProfileCreation() {
            noProfileNameWarningCreation.gameObject.SetActive(false);
            duplicateProfileNameWarningCreation.gameObject.SetActive(false);
            
            profileCreationScreen.SetActive(false);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        
        public void Exit() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);

            SceneManager.LoadScene("MainScene");
        }

        public void SelectProfile() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            ProfileManager.SelectProfile(Convert.ToInt32(profileToggleGroup.ActiveToggles().First().transform.parent
                .name));

            levelSelectionScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        public void EditProfile() {
            profileRenameField.text = ProfileManager.GetProfile(Convert.ToInt32(profileToggleGroup.ActiveToggles().First().transform.parent.name)).name;
            profileEditingScreen.SetActive(true);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }

        public void CreateProfile() {
            profileNameField.text = "";
            profileCreationScreen.SetActive(true);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }

        public void ConfirmProfileCreation() {
            if (profileNameField.text == "") {
                noProfileNameWarningCreation.gameObject.SetActive(true);
                duplicateProfileNameWarningCreation.gameObject.SetActive(false);
            }
            else {
                if (!ProfileManager.ProfileExists(profileNameField.text)) {
                    noProfileNameWarningCreation.gameObject.SetActive(false);
                    duplicateProfileNameWarningCreation.gameObject.SetActive(false);
                    ProfileManager.CreateProfile(profileNameField.text, LevelRegistry.GetLevelCount());

                    var newProfile = ProfileManager.GetProfile(ProfileManager.GetProfileCount() - 1);
                    var newProfileToggle = Instantiate(exampleProfileToggle, screenViewContent.transform, true);
                    ToggleValueChanged();

                    newProfileToggle.text = newProfile.name;
                    newProfileToggle.name = (ProfileManager.GetProfileCount() - 1).ToString();
                    ((Toggle) newProfileToggle.GetComponentInChildren(typeof(Toggle))).isOn = true;
                    newProfileToggle.gameObject.SetActive(true);
                    profileButtonList.Add(newProfileToggle);

                    profileCreationScreen.SetActive(false);
                    AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
                }
                else {
                    duplicateProfileNameWarningCreation.gameObject.SetActive(true);
                    noProfileNameWarningCreation.gameObject.SetActive(false);
                }
            }
        }
    }
}