using GameScene1;
using GameScene2;
using GameScene3;
using Internationalization;
using UnityEngine;

namespace ProgramSetup
{
    public static class StartingSetup {
        #region Methods
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