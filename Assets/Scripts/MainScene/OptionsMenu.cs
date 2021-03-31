using System.Collections.Generic;
using System.Linq;
using Internationalization;
using Lean.Gui;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainScene
{
    /// <summary>
    /// This class adds functionality to the options menu.
    /// </summary>
    public class OptionsMenu : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The <see cref="TMP_Dropdown"/> in the options menu that's used to select languages
        /// </summary>
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private LeanToggle fullscreenToggle;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider audioEffectsVolumeSlider;
        [SerializeField] private MainSceneTranslator mainSceneTranslator;

        /// <summary>
        /// An array of <see cref="Resolution"/>'s. Used to store available resolutions for the display the user is using.
        /// </summary>
        private Resolution[] resolutions;
        #endregion

        #region Methods

        /// <summary>
        /// Start is called before the first frame update.
        /// It sets up the options menu when it's first entered.
        /// </summary>
        private void Start()
        {
            //Setting the fullscreenToggle value to the current fullscreen state
            fullscreenToggle.Set(Screen.fullScreen);

            //Setting the volumeSlider value to the game volume and the audioEffectsSlider value to the audio effects volume
            musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
            audioEffectsVolumeSlider.value = AudioManager.Instance.GetAudioEffectsVolume();
            
            //If the user is not using fullscreen, the resolution dropdown is disabled as changing the resolution with fullscreen off doesn't work
            resolutionDropdown.interactable = Screen.fullScreen;

            resolutions = Screen.resolutions.Select(res => new Resolution {width = res.width, height = res.height}).Distinct().ToArray();
            resolutionDropdown.ClearOptions();
            var options = new List<string>();
            var curResolutionIndex = 0;

            for (var i = 0; i < resolutions.Length; i++)
            {
                var option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height) curResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(options);

            //Setting the selected option on the resolutionDropdown to the current resolution
            resolutionDropdown.value = curResolutionIndex;
            resolutionDropdown.RefreshShownValue();


            if (Translator.LanguageNameList.Count > 0) {
	            languageDropdown.ClearOptions();
	            languageDropdown.AddOptions(Translator.LanguageNameList);
	            languageDropdown.value = Translator.selectedLanguageIndex;
	            languageDropdown.RefreshShownValue();
            }
            else {
	            languageDropdown.ClearOptions();
	            languageDropdown.AddOptions(new List<string> { "Missing lang files" });
	            languageDropdown.value = 0;
	            languageDropdown.RefreshShownValue();

	            return;
            }
        }

        /// <summary>
        /// Sets the music volume to the passed value and saves the new value in options.
        /// </summary>
        /// <param name="value">The new music volume value</param>
        public void SetMusicVolume(float value)
        {
            AudioManager.Instance.SetMusicVolume(value);
            
            var volumeSaveObject = new OptionsManager();
            volumeSaveObject.SaveData();
        }

        /// <summary>
        /// Sets the audio effects volume to the passed value and saves the new value in options.
        /// </summary>
        /// <param name="value">The new audio effects volume value</param>
        public void SetAudioEffectsVolume(float value)
        {
            AudioManager.Instance.SetAudioEffectsVolume(value);
            
            var volumeSaveObject = new OptionsManager();
            volumeSaveObject.SaveData();
        }

        /// <summary>
        /// Changes if the game is running in fullscreen mode or not.
        /// </summary>
        /// <param name="isFullscreen">The value you want the fullscreen to be (true = fullscreen, false = no fullscreen)</param>
        public void SetFullscreen(bool isFullscreen)
        {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            var fullscreenSaveObject = new OptionsManager();
            fullscreenSaveObject.LoadData();
            if (isFullscreen != Screen.fullScreen)
                Screen.SetResolution(fullscreenSaveObject.width, fullscreenSaveObject.height, isFullscreen);

            resolutionDropdown.interactable = isFullscreen;

            fullscreenSaveObject.fullscreen = isFullscreen;
            fullscreenSaveObject.SaveData();
        }

        /// <summary>
        /// Changes the game's resolution.
        /// </summary>
        /// <param name="resolutionIndex">The index in <see cref="resolutions"/> at which the resolution you want to change to is stored.</param>
        public void SetResolution(int resolutionIndex)
        {
            var resolution = resolutions[resolutionIndex];
            if (resolution.height != Screen.currentResolution.height ||
                resolution.width != Screen.currentResolution.width)
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            var resolutionSaveObject = new OptionsManager {
                height = resolution.height,
                width = resolution.width
            };
            resolutionSaveObject.SaveData();
        }

        /// <summary>
        /// Changes the game's language.
        /// </summary>
        /// <param name="languageIndex">The index in the language dropdown as well as in the <see cref="Translator"/> at which the language you want to change to is stored</param>
        public void SetLanguage(int languageIndex) {
	        if (Translator.selectedLanguage != Translator.LanguageNameList[languageIndex]) {
		        Translator.SetLanguage(languageIndex);
		        mainSceneTranslator.UpdateTranslations();

		        var languageSaveObject = new OptionsManager {
			        language = Translator.LanguageNameList[languageIndex]
		        };
		        languageSaveObject.SaveData();
	        }
        }
        #endregion
    }
}