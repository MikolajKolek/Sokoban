using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace GameScene2 {
    /// <summary>
    /// Manages the profiles for gameScene2.
    /// </summary>
    public static class ProfileManager {
        #region Variables
        /// <summary>
        /// The <see cref="List{T}"/> that stores all the profiles.
        /// </summary>
        private static readonly List<Profile> ProfileRegistry = new List<Profile>();
        /// <summary>
        /// The currently selected <see cref="Profile"/>
        /// </summary>
        public static Profile selectedProfile;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the <see cref="ProfileRegistry"/> by loading all profiles saved in %AppData%\Sokoban\profiles and then sorts them by their IDs.
        /// </summary>
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

        /// <summary>
        /// Creates a new profile and then saves it in the %AppData%\Sokoban\profiles folder.
        /// </summary>
        /// <param name="profileName">The new profile's name</param>
        /// <param name="levelCount">The current count of levels stored in the <see cref="GameScene1.LevelRegistry"/></param>
        public static void CreateProfile(string profileName, int levelCount) {
            var newProfile = new Profile(ProfileRegistry.Count, profileName, levelCount);
            ProfileRegistry.Add(newProfile);
            SaveProfile(ProfileRegistry.Count - 1);
        }

        /// <summary>
        /// Checks if a profile with the given name exists in the <see cref="ProfileRegistry"/>.
        /// </summary>
        /// <param name="profileName">The profile name that is being checked in the registry.</param>
        /// <returns>A bool that is equal to true if a profile by that name exists and false if it doesn't.</returns>
        public static bool ProfileExists(string profileName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + profileName + ".json";

            if (File.Exists(path))
                return true;
            
            return false;
        }

        /// <summary>
        /// Saves a profile with the given profileID to the %AppData%\Sokoban\profiles folder.
        /// </summary>
        /// <param name="profileID">The profile's id in the <see cref="ProfileRegistry"/></param>
        private static void SaveProfile(int profileID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + ProfileRegistry[profileID].name + ".json";
            
            var json = JsonUtility.ToJson(ProfileRegistry[profileID]);
            var fs = File.Create(path);
            var bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        /// <summary>
        /// Gets the profile with the given profileID.
        /// </summary>
        /// <param name="profileId">The profile's id in the <see cref="ProfileRegistry"/></param>
        /// <returns>The profile stored at <see cref="profileId"/></returns>
        public static Profile GetProfile(int profileId) {
            return ProfileRegistry[profileId];
        }

        /// <summary>
        /// Selects the profile stored at profileID as the currently used profile.
        /// </summary>
        /// <param name="profileID">The profile's id in the <see cref="ProfileRegistry"/></param>.
        public static void SelectProfile(int profileID) {
            selectedProfile = ProfileRegistry[profileID];
        }

        /// <summary>
        /// Saves the profile that is currently selected.
        /// </summary>
        public static void SaveSelectedProfile() {
            ProfileRegistry[selectedProfile.id] = selectedProfile;
            
            SaveProfile(selectedProfile.id);
        }

        /// <summary>
        /// Gets the current number of profiles stored in the <see cref="ProfileRegistry"/>.
        /// </summary>
        /// <returns>The number of profiles</returns>
        public static int GetProfileCount() {
            return ProfileRegistry.Count;
        }

        /// <summary>
        /// Deletes the profile stored at the given ID and adjusts the IDs of other profiles to be correct.
        /// </summary>
        /// <param name="profileID">The ID of the profile you want to delete in the <see cref="ProfileRegistry"/></param>
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

        /// <summary>
        /// Renames a profile at the given profileID.
        /// </summary>
        /// <param name="profileID">The ID of the profile you want to rename in the <see cref="ProfileRegistry"/><./param>
        /// <param name="newProfileName">The new profile name.</param>
        public static void RenameProfile(int profileID, string newProfileName) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban";
            var path = directory + @"\profiles\" + ProfileRegistry[profileID].name + ".json";
            File.Delete(path);
            
            ProfileRegistry[profileID] = ProfileRegistry[profileID].ChangeName(newProfileName);
            SaveProfile(profileID);
        }

        /// <summary>
        /// Returns a list containing all the profiles sorted in order from highest score to lowest score.
        /// </summary>
        /// <returns>The list of profiles.</returns>
        public static IEnumerable<Profile> GetProfileLeaderboard() {
            var leaderboard = new List<Profile>();
            foreach (var element in ProfileRegistry)
                leaderboard.Add(element);

            leaderboard.Sort((p1, p2) => p2.score.CompareTo(p1.score));
            return leaderboard;
        }
        #endregion
    }
}