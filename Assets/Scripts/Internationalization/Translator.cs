using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// A namespace for classes that implement Sokoban's internationalization system.
/// </summary>
namespace Internationalization
{
    /// <summary>
    /// Adds an easily expandable localization system. 
    /// </summary>
    /// <remarks>
    /// <para>Loads a list of languages from the <c>lang</c> folder and gets translations from the <c>json</c> files in that folder using a system of translation keys.</para>
    /// This system allows for easily modifying and adding translations without changing any code.
    /// </remarks>
    public static class Translator {
        #region Variables
        /// <summary>
        /// Contains a list of names of all the languages in the <c>lang</c> folder.
        /// </summary>
        public static readonly List<string> LanguageNameList = new List<string>();
        /// <summary>
        /// Contains a list of paths to all the <c>json</c> files storing language data in the <c>lang</c> folder.
        /// </summary>
        private static readonly List<string> languageFileList = new List<string>();
        /// <summary>
        /// Contains the name of the currently selected language.
        /// </summary>
        public static string selectedLanguage;
        /// <summary>
        /// Contains the parsed data of the currently selected language <c>json</c> file.
        /// </summary>
        private static JObject langFile;
        /// <summary>
        /// The index of the currently selected language in the <see cref="LanguageNameList"/> and <see cref="languageFileList"/>
        /// </summary>
        public static int selectedLanguageIndex;
        #endregion

        #region Methods
        /// <summary>
        /// Loads all valid language files from the <c>lang</c> folder and puts their names in <see cref="LanguageNameList"/> and their paths in <see cref="languageFileList"/>. 
        /// </summary>
        public static void SetupLanguageList() {
            string[] files;
			
            //If the lang directory exists, the paths to all json files in the directory are loaded into the files array
            try {
                files = Directory.GetFiles("lang", "*.json");
            }
            catch(DirectoryNotFoundException) {
                Debug.Log("Missing lang folder");
                return;
            }
		
            foreach(var file in files) {
                var fileValue = File.ReadAllText(file);
                JObject parsedFile;

                //If the file is a valid json file, its value is parsed into parsedFile, otherwise the file is skipped
                try {
                    parsedFile = JObject.Parse(fileValue);
                }
                catch(JsonException) {
                    Debug.Log("Detected an invalid lang file: " + file);
                    continue;
                }

                //If the parsedFile contains the language.name key, its name is added to languageNameList and its path is added to languageFileList
                if (parsedFile["language.name"] != null) {
                    LanguageNameList.Add(parsedFile["language.name"].ToString());
                    languageFileList.Add(file); 
                }
            }
        }

        /// <summary>
        /// Changes the language that <see cref="Translator.GetTranslation(string)"/> returns translations in.
        /// </summary>
        /// <param name="languageIndex">The index of the language in <see cref="LanguageNameList"/> and <see cref="languageFileList"/>.</param>
        public static void SetLanguage(int languageIndex) {
            selectedLanguageIndex = languageIndex;
            selectedLanguage = LanguageNameList[languageIndex];
		
            //langFile is set to the parsed value of the file which path is stored in languageFileList[languageIndex]
            var fileValue = File.ReadAllText(languageFileList[languageIndex]);
            langFile = JObject.Parse(fileValue);
        }
	
        /// <summary>
        /// Returns the translation of the provided key to the language selected in <see cref="Translator.SetLanguage(int)"/>.
        /// </summary>
        /// <param name="key">The translation key that identifies a piece of text used in the game.</param>
        /// <returns>A <c>string</c> containing the translation of the key. If the translation is not found in the currently selected language file, the key is returned unchanged.</returns>
        public static string GetTranslation(string key) {
            if (key == null || langFile == null)
                return key;
            if (langFile[key] == null) {
                Debug.LogError("Translation for key " + key + " not found!");
                return key;
            }

            //If the currently selected file contains the translation and is properly loaded, the translation is returned
            return langFile[key].ToString();
        }
        #endregion
    }
}