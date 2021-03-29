using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameScene3 {
    public static class SaveDataRegistry {
        #region Variables
        private static readonly List<SaveData2> Registry = new List<SaveData2>();
        #endregion

        #region Methods
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

        public static void AddSaveData(SaveData2 saveData) {
            Registry.Add(saveData);
            SaveSaveData(Registry.Count - 1);
        }

        private static void SaveSaveData(int saveDataID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\saves\" + Registry[saveDataID].saveDataName + ".json";
            
            var json = JsonUtility.ToJson(Registry[saveDataID]);
            var fs = File.Create(path);
            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        public static SaveData2 GetSaveData(int saveDataID) {
            return Registry[saveDataID];
        }
        
        public static int GetSaveDataCount() {
            return Registry.Count;
        }

        public static bool SaveGameExists(string saveGameName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\saves\" + saveGameName + ".json";

            if (File.Exists(path))
                return true;
            
            return false;
        }

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