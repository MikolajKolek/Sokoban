using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace GameScene2 {
    public static class ProfileManager {
        #region Variables
        private static readonly List<Profile> ProfileRegistry = new List<Profile>();
        public static Profile selectedProfile;
        #endregion

        #region Methods
        public static void InitializeProfileRegistry() {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles";
            if (Directory.Exists(path)) {
                var files = Directory.GetFiles(path, "*.json");

                foreach (var file in files) {
                    var json = File.ReadAllText(file);
                    var loadedProfile = JsonUtility.FromJson<Profile>(json);
                    ProfileRegistry.Add(loadedProfile);
                }
            }
            else
                Directory.CreateDirectory(path);
            
            ProfileRegistry.Sort();
        }
        
        public static void CreateProfile(string profileName, int levelCount) {
            var newProfile = new Profile(ProfileRegistry.Count, profileName, levelCount);
            ProfileRegistry.Add(newProfile);
            SaveProfile(ProfileRegistry.Count - 1);
        }

        public static bool ProfileExists(string profileName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + profileName + ".json";

            if (File.Exists(path))
                return true;
            
            return false;
        }
        
        private static void SaveProfile(int profileID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + ProfileRegistry[profileID].name + ".json";
            
            var json = JsonUtility.ToJson(ProfileRegistry[profileID]);
            var fs = File.Create(path);
            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        public static Profile GetProfile(int profileId) {
            return ProfileRegistry[profileId];
        }

        public static void SelectProfile(int profileID) {
            selectedProfile = ProfileRegistry[profileID];
        }

        [UsedImplicitly]
        public static void SaveSelectedProfile() {
            ProfileRegistry[selectedProfile.id] = selectedProfile;
            
            SaveProfile(selectedProfile.id);
        }

        public static int GetProfileCount() {
            return ProfileRegistry.Count;
        }

        public static void DeleteProfile(int profileID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + ProfileRegistry[profileID].name + ".json";
            
            File.Delete(path);

            for(var i = profileID + 1; i < ProfileRegistry.Count; i++) {
                ProfileRegistry[i] = new Profile(i - 1, ProfileRegistry[i].score,ProfileRegistry[i].name, ProfileRegistry[i].savedGame, ProfileRegistry[i].levelScore); 
                SaveProfile(i);
            }
            ProfileRegistry.RemoveAt(profileID);
        }

        public static void RenameProfile(int profileID, string newProfileName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + ProfileRegistry[profileID].name + ".json";
            File.Delete(path);
            
            ProfileRegistry[profileID] = ProfileRegistry[profileID].ChangeName(newProfileName);
            SaveProfile(profileID);
        }

        public static List<Profile> GetProfileLeaderboard() {
            var leaderboard = new List<Profile>();
            foreach (var element in ProfileRegistry)
                leaderboard.Add(element);

            leaderboard.Sort((p1, p2) => p2.score.CompareTo(p1.score));
            return leaderboard;
        }
        #endregion
    }
}