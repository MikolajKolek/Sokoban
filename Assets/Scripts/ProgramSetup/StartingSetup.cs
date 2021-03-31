using GameScene1;
using GameScene2;
using GameScene3;
using Internationalization;
using UnityEngine;

/// <summary>
/// A nameapsce for classes that are used all throughout the project
/// </summary>
namespace ProgramSetup
{
    /// <summary>
    /// This class contains <see cref="OnProgramSetup"/> which initializes different systems in the game.
    /// </summary>
    public static class StartingSetup {
        #region Methods
        /// <summary>
        /// Called before any scenes are loaded. Sets up all the systems in the game that require initialization (loads options, sets up the internationalization
        /// system and initializes the level, save data and profile registries.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnProgramSetup() {
            var optionsSaveObject = new OptionsManager();
            optionsSaveObject.LoadData();
            if ((Screen.currentResolution.height != optionsSaveObject.height || Screen.currentResolution.width != optionsSaveObject.width || Screen.fullScreen != optionsSaveObject.fullscreen) && optionsSaveObject.fullscreen) {
                Screen.SetResolution(optionsSaveObject.width, optionsSaveObject.height, optionsSaveObject.fullscreen);
            }

            Translator.SetupLanguageList();

            if (Translator.LanguageNameList.Count <= 0) return;
		
            var currentLanguageIndex = -1;
            var defaultLanguageIndex = 0;

            for (var i = 0; i < Translator.LanguageNameList.Count; i++) {
                if (Translator.LanguageNameList[i] == optionsSaveObject.language)
                    currentLanguageIndex = i;
                else if (Translator.LanguageNameList[i] == "English")
                    defaultLanguageIndex = i;
            }

            if (currentLanguageIndex >= 0) {
                Translator.SetLanguage(currentLanguageIndex);
            }
            else {
                Translator.SetLanguage(defaultLanguageIndex);

                optionsSaveObject.language = Translator.selectedLanguage;
                optionsSaveObject.SaveData();
            }
            
            LevelRegistry.InitializeLevelList();
            SaveDataRegistry.InitializeRegistry();
            ProfileManager.InitializeProfileRegistry();
        }
        #endregion
    }
}