using System.Collections.Generic;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;

/// <summary>
/// A namespace for classes used in the main scene
/// </summary>
namespace MainScene
{
    /// <summary>
    /// Takes care of setting up the <c>MainScene</c> and loading options.
    /// </summary>
    public class MainSceneSetup : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The <c>mainScreen</c> <see cref="GameObject"/>
        /// </summary>
        [SerializeField] private GameObject mainScreen;
        [SerializeField] private GameObject optionsMenuScreen;
        [SerializeField] private GameObject infoScreen;
        [SerializeField] private GameObject stageSelectionScreen;
        /// <summary>
        /// The main scene's translator object
        /// </summary>
        [SerializeField] private MainSceneTranslator mainSceneTranslator;
        #endregion

        #region Methods
        /// <summary>
        /// Start is called before the first frame update.
        /// <para>It loads options from the options file, and sets up the language dropdown</para>
        /// </summary>
        private void Start()
        {
            var optionsSaveObject = new OptionsManager();
            optionsSaveObject.LoadData();
            AudioManager.Instance.SetMusicVolume(optionsSaveObject.musicVolume);
            AudioManager.Instance.SetAudioEffectsVolume(optionsSaveObject.audioEffectsVolume);
            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);

            mainSceneTranslator.UpdateTranslations();
        }

        /// <summary>
        /// Detects presses of the <c>Escape</c> key and quits out of the currently open screen.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (optionsMenuScreen.activeSelf)
                    optionsMenuScreen.SetActive(false);
                if (infoScreen.activeSelf)
                    infoScreen.SetActive(false);
                if (stageSelectionScreen.activeSelf)
                    stageSelectionScreen.SetActive(false);

                mainScreen.SetActive(true);
            }
        }

        /// <summary>
        /// Plays the <see cref="AudioManager.AudioEffectClip.ButtonClicked"/> sound effect
        /// </summary>
        public void PlayButtonClick() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        #endregion
    }
}