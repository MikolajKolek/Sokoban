using System;
using System.Collections.Generic;

namespace GameScene1
{
    [Serializable]
    public struct Level : IComparable {
        #region Data
        public int id;
        public string levelName;
        public readonly int time;
        public readonly Difficulty difficultyLevel;
        public readonly int boxCount;
        public readonly int levelWidth;
        public readonly int levelHeight;
        public readonly List<List<Tile>> levelLayout;
        #endregion
	
        public Level(int id, string levelName, int time, Difficulty difficultyLevel, int boxCount, int levelWidth, int levelHeight, List<List<Tile>> levelLayout) {
            this.id = id;
            this.levelName = levelName;
            this.time = time;
            this.difficultyLevel = difficultyLevel;
            this.boxCount = boxCount;
            this.levelWidth = levelWidth;
            this.levelHeight = levelHeight;
            this.levelLayout = levelLayout;
        }

        public Level DecrementID() {
            id--;
            return this;
        }

        public Level ChangeName(string newName) {
            levelName = newName;
            return this;
        }
        
        public enum Difficulty {
            None = 0,
            Easy = 1,
            Medium = 2,
            Hard = 3
        }

        public enum Tile {
            None = 0,
            Wall = '#',
            Floor = ' ',
            Player = 'P',
            PlayerOnBoxArea = 'B',
            Box = '$',
            BoxArea = '*',
            Empty = '.',
            BoxOnBoxArea = '&'
        }
        
        public int CompareTo(object compareLevel) {
            return id.CompareTo(((Level) compareLevel).id);
        }
    }
}