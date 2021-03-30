using System;
using GameScene1;
using ProgramSetup;

namespace GameScene2 {
    public class TilemapGameAdapter2 : TilemapGameAdapter {
        public double newLevelScore;
        public bool isHighScore;
        
        protected override void LevelFinished() {
            isHighScore = false;
            
            var time = currentTime + Math.Abs(timeDelta - 1);
            
            if (time >= currentLevel.time * 10)
                newLevelScore = 0;
            else if (time <= currentLevel.time)
                newLevelScore = 100;
            else
                newLevelScore = 100 * Math.Abs((float) (time - currentLevel.time) / (currentLevel.time * 9) - 1);
            
            var levelScore = ProfileManager.selectedProfile.levelScore;
            if (newLevelScore > levelScore[currentLevel.id]) {
                isHighScore = true;
                ProfileManager.selectedProfile = ProfileManager.selectedProfile.ChangeScore(ProfileManager.selectedProfile.score + (int) newLevelScore - levelScore[currentLevel.id]);
            }
            
            levelScore[currentLevel.id] = (int) Math.Max(levelScore[currentLevel.id], newLevelScore);
            ProfileManager.selectedProfile = ProfileManager.selectedProfile.ChangeLevelScore(levelScore);
            ProfileManager.SaveSelectedProfile();
            
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            base.LevelFinished();
        }
        
        public void SaveGameData() {
            var serializedTilemap = new TilemapSerializable(boxOnBoxArea, boxArea, player, playerOnBoxArea, floor, wall, box, empty);
            serializedTilemap.SerializeTilemap(tilemap, currentLevel.levelHeight, currentLevel.levelWidth);
            
            var saveData = new SaveData(serializedTilemap, currentPlayerLocation, horizontalMultiplier, boxInPlaceCount,
                currentLevel.id, boxMoveCount, playerMoveCount, currentTime, timeDelta);
            
            ProfileManager.selectedProfile = ProfileManager.selectedProfile.ChangeSavedGame(saveData);
            ProfileManager.SaveSelectedProfile();
        }
        
        public void LoadSaveData() {
            var saveData = ProfileManager.selectedProfile.savedGame;
            
            if (saveData == null)
                return;
            
            saveData.mapState.InitializeTilemapDeserialization(boxOnBoxArea, boxArea, player, playerOnBoxArea,  floor, wall, box, empty);
            tilemap = saveData.mapState.DeserializeTilemap(tilemap);
            currentPlayerLocation = saveData.currentPlayerLocation;
            horizontalMultiplier = saveData.horizontalMultiplier;
            boxInPlaceCount = saveData.boxInPlaceCount;
            currentLevel = LevelRegistry.GetLevel(saveData.currentLevelID - 1);
            boxMoveCount = saveData.boxMoveCount;
            playerMoveCount = saveData.playerMoveCount;
            currentTime = saveData.currentTime;
            timeDelta = saveData.timeDelta;
            
            if (currentLevel.Equals(LevelRegistry.GetLevel(LevelRegistry.GetLevelCount() - 1)))
                AudioManager.Instance.PlayMusic(AudioManager.MusicClip.LastLevelMusic);
            else {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (currentLevel.difficultyLevel) {
                    case Level.Difficulty.Easy:
                        AudioManager.Instance.PlayMusic(AudioManager.MusicClip.EasyLevelMusic);
                        break;
                    case Level.Difficulty.Medium:
                        AudioManager.Instance.PlayMusic(AudioManager.MusicClip.MediumLevelMusic);
                        break;
                    case Level.Difficulty.Hard:
                        AudioManager.Instance.PlayMusic(AudioManager.MusicClip.HardLevelMusic);
                        break;
                    case Level.Difficulty.None:
                        AudioManager.Instance.StopMusic();
                        break;
                }
            }
            
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