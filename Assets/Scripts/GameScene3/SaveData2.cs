using System;
using System.Collections.Generic;
using GameScene1;
using GameScene2;
using UnityEngine;

namespace GameScene3 {
	/// <summary>
	/// Stores all the data required to restore a savestate in stage 3.
	/// </summary>
    [Serializable]
    public class SaveData2 : IComparable {
        #region Data
        public long timeCreated;
        public string saveDataName;
        public SavableLevel currentLevel;
        public TilemapSerializable mapState;
        public Vector3Int currentPlayerLocation;
        public float horizontalMultiplier;
        public int boxInPlaceCount;
        public int boxMoveCount;
        public int playerMoveCount;
        public int currentTime;
        public double timeDelta;
        #endregion

        public SaveData2(string saveDataName, SavableLevel currentLevel, TilemapSerializable mapState,
            Vector3Int currentPlayerLocation, float horizontalMultiplier, int boxInPlaceCount,
            int boxMoveCount, int playerMoveCount, int currentTime, double timeDelta) {
            this.saveDataName = saveDataName;
            timeCreated = DateTime.Now.Ticks;

            this.currentLevel = currentLevel;
            this.mapState = mapState;
            this.currentPlayerLocation = currentPlayerLocation;
            this.horizontalMultiplier = horizontalMultiplier;
            this.boxInPlaceCount = boxInPlaceCount;
            this.boxMoveCount = boxMoveCount;
            this.playerMoveCount = playerMoveCount;
            this.currentTime = currentTime;
            this.timeDelta = timeDelta;
        }

        /// <summary>
        /// A comparator operator used for sorting. It sorts the profiles from the first time created to the last time created.
        /// </summary>
        /// <param name="compareSaveData">Another save data that this save data is being compared to.</param>
        public int CompareTo(object compareSaveData) {
            return timeCreated.CompareTo(((SaveData2) compareSaveData).timeCreated);
        }

        /// <summary>
        /// A struct that stores data about the level that the player was playing in case it gets deleted or moved in the <see cref="LevelRegistry2"/>
        /// </summary>
        [Serializable]
        public struct SavableLevel {
            public int boxCount;
            public int levelWidth;
            public int levelHeight;
            public List<Level.Tile> levelLayout;

            public SavableLevel(int boxCount, int levelWidth, int levelHeight, List<Level.Tile> levelLayout) {
                this.boxCount = boxCount;
                this.levelWidth = levelWidth;
                this.levelHeight = levelHeight;
                this.levelLayout = levelLayout;
            }
        }
    }
}