using System;
using UnityEngine;

namespace GameScene2 {
    //TODO: If possible, revert back to struct, it was changed to a class to make it nullable
    [Serializable]
    public class SaveData {
        #region Data
        public TilemapSerializable mapState;
        public Vector3Int currentPlayerLocation;
        public float horizontalMultiplier;
        public int boxInPlaceCount;
        public int currentLevelID;
        public int boxMoveCount;
        public int playerMoveCount;
        public int currentTime;
        public double timeDelta;
        
        public bool isEmpty;
        #endregion

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

        public SaveData() {
            isEmpty = true;
        }
    }
}