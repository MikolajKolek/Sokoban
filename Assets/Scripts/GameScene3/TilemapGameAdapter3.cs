using System.Collections.Generic;
using GameScene1;
using GameScene2;
using ProgramSetup;
using UnityEngine;

namespace GameScene3 {
	/// <summary>
	/// Acts as an adapter between the <see cref="Tilemap"/> and <see cref="Grid"/> and the game. It has the ability to load and properly scale levels, move the player, restart the game and call delegates when certain events happen.
	/// It has improvements over the base <see cref="TilemapGameAdapter"/> that add a capability to save and load game data to it, and also to be used for new level playtesting.
	/// </summary>
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
        
        /// <summary>
        /// Saves the game's data into a new <see cref="SaveData2"/> object in the <see cref="SaveDataRegistry"/>.
        /// </summary>
        /// <param name="levelName">The name that you want the new <see cref="SaveData2"/> to have</param>
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

        /// <summary>
        /// Used to switch the timer on and off when creating a save.
        /// </summary>
        /// <param name="state">The state you want the timer to be in. True = timer is going, false = timer is not going</param>
        public void SwitchTimerState(bool state) {
            gameTurnedOn = state;
        }
        
        /// <summary>
        /// Loads the save data from the <see cref="SaveDataRegistry"/> at the given ID.
        /// </summary>
        /// <param name="saveDataRegistryID">The ID of the loaded save data in the <see cref="SaveDataRegistry"/>.</param>
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