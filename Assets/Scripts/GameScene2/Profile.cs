using System;
using System.Collections.Generic;

namespace GameScene2 {
    public struct Profile : IComparable {
        #region Data
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // I don't want it to be readonly because JsonUtility.ToJson() doesn't serialize readonly fields
        public int id;
        public int score;
        public string name;
        public List<int> levelScore;
        public SaveData savedGame;
        #endregion
        
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

        public Profile ChangeName(string newName) {
            name = newName;
            return this;
        }

        public Profile ChangeLevelScore(List<int> newLevelScore) {
            levelScore = newLevelScore;
            return this;
        }
        
        public int CompareTo(object compareProfile) {
            return id.CompareTo(((Profile) compareProfile).id);
        }

        public Profile ChangeSavedGame(SaveData newSavedGame) {
            savedGame = newSavedGame;
            return this;
        }

        public Profile ChangeScore(int newScore) {
            score = newScore;
            return this;
        }
    }
}