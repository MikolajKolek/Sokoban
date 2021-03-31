using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameScene3 {
	/// <summary>
	/// Manages the save data for gameScene3.
	/// </summary>
    public static class SaveDataRegistry {
        #region Variables
        /// <summary>
        /// The <see cref="List{T}"/> that stores all the <see cref="SaveData2"/>s.
        /// </summary>
        private static readonly List<SaveData2> Registry = new List<SaveData2>();
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the <see cref="Registry"/> by loading all the saves from %AppData%\Sokoban\saves into it and then sorts the saves by date created.
        /// </summary>
        public static void InitializeRegistry() {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban\saves";
            if (Directory.Exists(directory)) {
                var files = Directory.GetFiles(directory, "*.json");

                foreach (var file in files) {
                    var json = File.ReadAllText(file);
                    var loadedSaveData = JsonUtility.FromJson<SaveData2>(json);
                    Registry.Add(loadedSaveData);
                }
            }
            else
                Directory.CreateDirectory(directory);
            
            Registry.Sort();
        }

        /// <summary>
        /// Adds a new <see cref="SaveData2"/> to the <see cref="Registry"/>.
        /// </summary>
        /// <param name="saveData">The <see cref="SaveData2"/> that you want to add to the registry.</param>
        public static void AddSaveData(SaveData2 saveData) {
            Registry.Add(saveData);
            SaveSaveData(Registry.Count - 1);
        }

        /// <summary>
        /// Saves the save data at the given ID into a file.
        /// </summary>
        /// <param name="saveDataID">The index in the <see cref="Registry"/> of the <see cref="SaveData2"/> that will be saved.</param>
        private static void SaveSaveData(int saveDataID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\saves\" + Registry[saveDataID].saveDataName + ".json";
            
            var json = JsonUtility.ToJson(Registry[saveDataID]);
            var fs = File.Create(path);
            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        /// Gets the save data at the given ID.
        /// </summary>
        /// <param name="saveDataID">The ID of the save data that you want to get in the <see cref="Registry"/>.</param>
        /// <returns>The requested <see cref="SaveData2"/>.</returns>
        public static SaveData2 GetSaveData(int saveDataID) {
            return Registry[saveDataID];
        }
        
        /// <summary>
        /// Returns the save data count
        /// </summary>
        public static int GetSaveDataCount() {
            return Registry.Count;
        }

        /// <summary>
        /// Checks if a <see cref="SaveData2"/> with the given name exists in the <see cref="Registry"/>
        /// </summary>
        /// <param name="saveGameName">The name that is being checked in the <see cref="Registry"/></param>
        /// <returns>True if a <see cref="SaveData2"/> by that name exists, false if it doesn't.</returns>
        public static bool SaveGameExists(string saveGameName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\saves\" + saveGameName + ".json";

            if (File.Exists(path))
                return true;
            
            return false;
        }

        /// <summary>
        /// Deletes the <see cref="SaveData2"/> at the given ID.
        /// </summary>
        /// <param name="saveDataID">The index in <see cref="Registry"/> of the <see cref="SaveData2"/> that you want to delete.</param>
        public static void DeleteSaveData(int saveDataID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\saves\" + Registry[saveDataID].saveDataName + ".json";

            File.Delete(path);

            Registry.RemoveAt(saveDataID);
            Registry.Sort();
        }
        #endregion
    }
}