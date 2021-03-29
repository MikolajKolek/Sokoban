using System.Collections.Generic;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;

namespace MainScene
{
    /// <summary>
    ///     Takes care of setting up the <c>MainScene</c>, loading options and languages.
    /// </summary>
    public class MainSceneSetup : MonoBehaviour
    {
        #region Variables
        /// <summary>
        ///     The <see cref="TMP_Dropdown"/> in the options menu that's used to select languages
        /// </summary>
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject infoMenu;
        [SerializeField] private GameObject stageSelectionMenu;
        [SerializeField] private MainSceneTranslator mainSceneTranslator;
        
        #endregion

        #region Methods

        /// <summary>
        ///     Start is called before the first frame update.
        ///     <para>It loads options from the options file, and also sets up languages as well as the language dropdown</para>
        /// </summary>
        private void Start()
        {
            var optionsSaveObject = new OptionsManager();
            optionsSaveObject.LoadData();
            AudioManager.Instance.SetMusicVolume(optionsSaveObject.musicVolume);
            AudioManager.Instance.SetAudioEffectsVolume(optionsSaveObject.audioEffectsVolume);
            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MenuMusic);

            if (Translator.LanguageNameList.Count > 0)
            {
                languageDropdown.ClearOptions();
                languageDropdown.AddOptions(Translator.LanguageNameList);
                languageDropdown.value = Translator.selectedLanguageIndex;
                languageDropdown.RefreshShownValue();
            }
            else
            {
                languageDropdown.ClearOptions();
                languageDropdown.AddOptions(new List<string> {"Missing lang files"});
                languageDropdown.value = 0;
                languageDropdown.RefreshShownValue();

                return;
            }

            mainSceneTranslator.UpdateTranslations();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (optionsMenu.activeSelf)
                    optionsMenu.SetActive(false);
                if (infoMenu.activeSelf)
                    infoMenu.SetActive(false);
                if (stageSelectionMenu.activeSelf)
                    stageSelectionMenu.SetActive(false);

                mainMenu.SetActive(true);
            }
        }

        public void SetLanguage(int languageIndex)
        {
            if (Translator.selectedLanguage != Translator.LanguageNameList[languageIndex])
            {
                Translator.SetLanguage(languageIndex);
                mainSceneTranslator.UpdateTranslations();

                var languageSaveObject = new OptionsManager
                {
                    language = Translator.LanguageNameList[languageIndex]
                };
                languageSaveObject.SaveData();
            }
        }

        public void PlayButtonClick() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
        }
        #endregion
    }
}