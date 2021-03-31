using System;
using System.IO;
using Internationalization;
using UnityEngine;

namespace ProgramSetup
{
    /// <summary>
    /// Handles saving and loading options data to a json file.
    /// </summary>
    public class OptionsManager
    {
        #region Variables
        /// <summary>
        /// The name of the language that the game is using.
        /// </summary>
        public string language;
        /// <summary>
        /// The game's fullscreen state.
        /// </summary>
        public bool fullscreen;
        /// <summary>
        /// The game's music volume state.
        /// </summary>
        public float musicVolume;
        /// <summary>
        /// The game's audio effects volume state.
        /// </summary>
        public float audioEffectsVolume;
        /// <summary>
        /// The width of the screen.
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the screen.
        /// </summary>
        public int height;
        #endregion

        #region Methods
        /// <summary>
        /// The <see cref="OptionsManager" /> constructor. It sets all the fields to the current display and audio settings.
        /// </summary>
        public OptionsManager()
        {
            fullscreen = Screen.fullScreen;
            height = Screen.height;
            width = Screen.width;

            if (Translator.selectedLanguage != null)
                language = Translator.selectedLanguage;
            else
                language = "English";

            if (AudioManager.Instance != null) {
                musicVolume = AudioManager.Instance.GetMusicVolume();
                audioEffectsVolume = AudioManager.Instance.GetAudioEffectsVolume();
            }
            else {
                musicVolume = 1f;
                audioEffectsVolume = 1f;
            }
        }
        
        /// <summary>
        /// Loads data from <c>options.json</c> into the <see cref="OptionsManager" /> fields. If <c>options.json</c> doesn't
        /// exist, it creates a new one and fills it with current settings.
        /// </summary>
        public void LoadData() {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\options.json";
            
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var loadOptionsObject = JsonUtility.FromJson<OptionsManager>(json);
                
                fullscreen = loadOptionsObject.fullscreen;
                language = loadOptionsObject.language;
                musicVolume = loadOptionsObject.musicVolume;
                audioEffectsVolume = loadOptionsObject.audioEffectsVolume;
                height = loadOptionsObject.height;
                width = loadOptionsObject.width;
            }
            else if (Directory.Exists(directory))
                SaveData();
            else {
                Directory.CreateDirectory(directory);
                SaveData();
            }
        }

        /// <summary>
        /// Saves all the <see cref="OptionsManager" /> fields into the <c>options.json</c> file.
        /// </summary>
        public void SaveData()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban\options.json";

            var json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }
        #endregion
    }
}