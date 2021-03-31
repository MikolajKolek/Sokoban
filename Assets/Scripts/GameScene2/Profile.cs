using System;
using System.Collections.Generic;
using GameScene1;

namespace GameScene2 {
    /// <summary>
    /// Profile is a struct that stores all information about a profile.
    /// </summary>
    public struct Profile : IComparable {
        #region Data
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // I don't want it to be readonly because JsonUtility.ToJson() doesn't serialize readonly fields
        /// <summary>
        /// Each profile has an id that is used to identify it.
        /// </summary>
        public int id;
        /// <summary>
        /// The added score of the scores on all levels.
        /// </summary>
        public int score;
        /// <summary>
        /// The profile's name.
        /// </summary>
        public string name;
        /// <summary>
        /// This list stores the profile's max score on each level. It is indexed by id's of levels in the <see cref="GameScene1.LevelRegistry"/>
        /// </summary>
        public List<int> levelScore;
        /// <summary>
        /// The save associated to this profile.
        /// </summary>
        public SaveData savedGame;
        #endregion
        
        /// <summary>
        /// This constructor sets the <see cref="id"/> and <see cref="name"/> fields to the given parameters and it initializes <see cref="savedGame"/> and <see cref="levelScore"/>.
        /// </summary>
        public Profile(int id, string name, int levelCount) {
            this.id = id;
            this.name = name;

            levelScore = new List<int>();
            savedGame = new SaveData();
            score = 0;
            
            for(var i = 0; i < levelCount; i++)
                levelScore.Add(0);
        }
        
        public Profile(int id, int score, string name, SaveData savedGame, List<int> levelScore) {
            this.id = id;
            this.score = score;
            this.name = name;
            this.savedGame = savedGame;
            this.levelScore = levelScore;
        }

        /// <summary>
        /// Changes the <see cref="Profile"/>'s name. It returns a <see cref="Profile"/>; as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <param name="newName">The <see cref="Profile"/>'s new name</param>
        /// <returns>The <see cref="Profile"/> object.</returns>
        public Profile ChangeName(string newName) {
            name = newName;
            return this;
        }

        /// <summary>
        /// Changes the <see cref="Profile"/>'s levelScore list. It returns a <see cref="Profile"/>; as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <param name="newLevelScore">The <see cref="Profile"/>'s new <see cref="levelScore"/> list</param>
        /// <returns>The <see cref="Profile"/> object.</returns>
        public Profile ChangeLevelScore(List<int> newLevelScore) {
            levelScore = newLevelScore;
            return this;
        }

        /// <summary>
        /// A comparator operator used for sorting. It sorts the profiles from the lowest id to the highest id.
        /// </summary>
        /// <param name="compareProfile">Another profile that this level is being compared to.</param>
        public int CompareTo(object compareProfile) {
            return id.CompareTo(((Profile) compareProfile).id);
        }

        /// <summary>
        /// Changes the <see cref="Profile"/>'s saved game. It returns a <see cref="Profile"/>; as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <param name="newSavedGame">The <see cref="Profile"/>'s new stored <see cref="SaveData"/></param>
        /// <returns>The <see cref="Profile"/> object.</returns>
        public Profile ChangeSavedGame(SaveData newSavedGame) {
            savedGame = newSavedGame;
            return this;
        }

        /// <summary>
        /// Changes the <see cref="Profile"/>'s score. It returns a <see cref="Profile"/>; as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <param name="newScore">The <see cref="Profile"/>'s new score</param>
        /// <returns>The <see cref="Profile"/> object.</returns>
        public Profile ChangeScore(int newScore) {
            score = newScore;
            return this;
        }
    }
}