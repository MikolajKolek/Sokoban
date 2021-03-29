using System.Collections.Generic;
using GameScene1;
using GameScene2;
using ProgramSetup;
using UnityEngine;

namespace GameScene3 {
    public class TilemapGameAdapter3 : TilemapGameAdapter {
        private bool inGameMode;
        
        public override void MovePlayer(Vector3Int direction) {
            base.MovePlayer(direction);
            if(inGameMode) {
                AudioManager.Instance.StopAudioEffects();
                inGameMode = false;
            }
        }

        public void EnterGameMode() {
            inGameMode = true;
        }
        
        public void SaveGameData(string levelName) {
            var serializedTilemap = new TilemapSerializable(boxOnBoxArea, boxArea, player, playerOnBoxArea, floor, wall, box, empty);
            serializedTilemap.SerializeTilemap(tilemap, currentLevel.levelHeight, currentLevel.levelWidth);

            var oneDimensionalLevelMap = new List<Level.Tile>();
            for(var i = 0; i < currentLevel.levelHeight; i++) {
                for(var j = 0 ; j < currentLevel.levelWidth; j++)
                {
                    oneDimensionalLevelMap.Add(currentLevel.levelLayout[i][j]);
                }
            }
            
            var newSavableLevel = new SaveData2.SavableLevel(currentLevel.boxCount, currentLevel.levelWidth, currentLevel.levelHeight, oneDimensionalLevelMap);
            
            var saveData = new SaveData2(levelName, newSavableLevel, serializedTilemap, currentPlayerLocation, horizontalMultiplier, boxInPlaceCount,
                boxMoveCount, playerMoveCount, currentTime, timeDelta);
            SaveDataRegistry.AddSaveData(saveData);
        }

        public void SwitchTimerState(bool state) {
            gameTurnedOn = state;
        }

        public void LoadSaveData(int saveDataRegistryID) {
            var saveData = SaveDataRegistry.GetSaveData(saveDataRegistryID);
            
            if (saveData == null)
                return;
            
            saveData.mapState.InitializeTilemapDeserialization(boxOnBoxArea, boxArea, player, playerOnBoxArea, floor, wall, box, empty);
            tilemap.ClearAllTiles();
            tilemap = saveData.mapState.DeserializeTilemap(tilemap);
            currentPlayerLocation = saveData.currentPlayerLocation;
            horizontalMultiplier = saveData.horizontalMultiplier;
            boxInPlaceCount = saveData.boxInPlaceCount;

            var twoDimensionalLevelMap = new List<List<Level.Tile>>();
            for (var i = 0; i < saveData.currentLevel.levelHeight; i++) {
                twoDimensionalLevelMap.Add(new List<Level.Tile>());
                for (var j = 0; j < saveData.currentLevel.levelWidth; j++) {
                    twoDimensionalLevelMap[i].Add(saveData.currentLevel.levelLayout[(i * saveData.currentLevel.levelWidth) + j]);
                }
            }
            currentLevel = new Level(0, "LoadedLevel", 0, Level.Difficulty.None, saveData.currentLevel.boxCount, 
                saveData.currentLevel.levelWidth, saveData.currentLevel.levelHeight, twoDimensionalLevelMap);
            
            boxMoveCount = saveData.boxMoveCount;
            playerMoveCount = saveData.playerMoveCount;
            currentTime = saveData.currentTime;
            timeDelta = saveData.timeDelta;
            
            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.PlayerLevelMusic);
            
            if (onBoxMoved != null) {
                onBoxMoved(boxMoveCount);
                onPlayerMoved(playerMoveCount);
                onTimePassed(currentTime);
            }
            
            gameTurnedOn = true;
            WindowScaleUpdate(currentLevel.levelHeight, currentLevel.levelWidth);
        }
    }
}