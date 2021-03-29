using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lean.Gui;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainScene
{
    /// <summary>
    ///     This class adds functionality to the options menu
    /// </summary>
    public class OptionsMenu : MonoBehaviour
    {
        #region Variables

        /// <summary>
        ///     The game's main audio mixer
        /// </summary>
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private LeanToggle fullscreenToggle;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider audioEffectsVolumeSlider;
        
        /// <summary>
        ///     An array of <c>Resolution</c>'s. Used to store available resolutions for the display the user is using
        /// </summary>
        private Resolution[] resolutions;

        #endregion

        #region Methods

        /// <summary>
        ///     Start is called before the first frame update.
        ///     It sets up the options menu when it's entered
        /// </summary>
        private void Start()
        {
            //Setting the fullscreenToggle value to the current fullscreen state
            fullscreenToggle.Set(Screen.fullScreen);

            //Setting the volumeSlider value to the game volume
            musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
            audioEffectsVolumeSlider.value = AudioManager.Instance.GetAudioEffectsVolume();
            
            resolutionDropdown.interactable = Screen.fullScreen;

            resolutions = Screen.resolutions.Select(res => new Resolution {width = res.width, height = res.height})
                .Distinct().ToArray();
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
        }

        public void SetMusicVolume(float value)
        {
            AudioManager.Instance.SetMusicVolume(value);
            
            var volumeSaveObject = new OptionsManager();
            volumeSaveObject.SaveData();
        }
        
        public void SetAudioEffectsVolume(float value)
        {
            AudioManager.Instance.SetAudioEffectsVolume(value);
            
            var volumeSaveObject = new OptionsManager();
            volumeSaveObject.SaveData();
        }

        [UsedImplicitly]
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
        #endregion
    }
}