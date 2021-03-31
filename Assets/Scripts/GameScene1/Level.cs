using System;
using System.Collections.Generic;

namespace GameScene1
{
    /// <summary>
    /// A struct that stores data about a level. Used in stage 1 and stage 2.
    /// </summary>
    [Serializable]
    public struct Level : IComparable {
        #region Data
        /// <summary>
        /// The level's id in the registry
        /// </summary>
        public int id;
        public string levelName;
        /// <summary>
        /// The time value is used to calculate the score the player gets from a level on stage 2.
        /// </summary>
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

        /// <summary>
        /// Decrements the <see cref="Level"/>'s id. It returns a <see cref="Level"/> as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <returns>The <see cref="Level"/> object.</returns>
        public Level DecrementID() {
            id--;
            return this;
        }

        /// <summary>
        /// Changes the <see cref="Level"/>'s name. It returns a <see cref="Level"/>; as structs are value types, so to change their value, you need to assign a new value to them.
        /// </summary>
        /// <param name="newName">The <see cref="Level"/>'s new name</param>
        /// <returns>The <see cref="Level"/> object.</returns>
        public Level ChangeName(string newName) {
            levelName = newName;
            return this;
        }
        
        /// <summary>
        /// An enum used for storing the <see cref="Level"/>'s difficulty level.
        /// </summary>
        public enum Difficulty {
            None = 0,
            Easy = 1,
            Medium = 2,
            Hard = 3
        }

        /// <summary>
        /// An enum used to store all the different types of tiles that are used in levels.
        /// </summary>
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
        
        /// <summary>
        /// A comparator operator used for sorting. It sorts the levels from the lowest id to the highest id.
        /// </summary>
        /// <param name="compareLevel">Another level that this level is being compared to.</param>
        public int CompareTo(object compareLevel) {
            return id.CompareTo(((Level) compareLevel).id);
        }
    }
}