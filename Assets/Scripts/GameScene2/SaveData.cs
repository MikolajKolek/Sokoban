using System;
using UnityEngine;

namespace GameScene2 {
    /// <summary>
    /// Stores all the data required to restore a savestate in stage 2.
    /// </summary>
    [Serializable]
    public class SaveData {
        #region Data
        /// <summary>
        /// The serialized tilemap.
        /// </summary>
        public TilemapSerializable mapState;
        public Vector3Int currentPlayerLocation;
        public float horizontalMultiplier;
        public int boxInPlaceCount;
        public int currentLevelID;
        public int boxMoveCount;
        public int playerMoveCount;
        public int currentTime;
        public double timeDelta;
        
        /// <summary>
        /// Stores information on weather the <see cref="SaveData"/> is empty or not.
        /// </summary>
        public bool isEmpty;
        #endregion

        /// <summary>
        /// Initializes the <see cref="SaveData"/> object with all it's fields.
        /// </summary>
        public SaveData(TilemapSerializable mapState, Vector3Int currentPlayerLocation, float horizontalMultiplier, int boxInPlaceCount,
            int currentLevelID, int boxMoveCount, int playerMoveCount, int currentTime, double timeDelta) {
            this.mapState = mapState;
            this.currentPlayerLocation = currentPlayerLocation;
            this.horizontalMultiplier = horizontalMultiplier;
            this.boxInPlaceCount = boxInPlaceCount;
            this.currentLevelID = currentLevelID;
            this.boxMoveCount = boxMoveCount;
            this.playerMoveCount = playerMoveCount;
            this.currentTime = currentTime;
            this.timeDelta = timeDelta;
            isEmpty = false;
        }

        /// <summary>
        /// Sets the <see cref="SaveData"/> object to empty.
        /// </summary>
        public SaveData() {
            isEmpty = true;
        }
    }
}