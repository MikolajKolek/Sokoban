using System;
using ProgramSetup;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene1 {
    public class TilemapGameAdapter : MonoBehaviour {
        #region Serialized variables
        [SerializeField] private bool playMusic;
        [SerializeField] protected TileBase boxOnBoxArea;
        [SerializeField] protected TileBase boxArea;
        [SerializeField] protected TileBase playerOnBoxArea;
        [SerializeField] protected TileBase player;
        [SerializeField] protected TileBase floor;
        [SerializeField] protected TileBase wall;
        [SerializeField] protected TileBase box;
        [SerializeField] protected TileBase empty;
        
        [SerializeField] protected Tilemap tilemap;
        [SerializeField] private Grid grid;
        
        [SerializeField] private float horizontalTilemapSpan = 1f;
        [SerializeField] private int horizontalMargin;
        [SerializeField] private int verticalMargin;
        #endregion

        protected float baseXCoordinate;
        protected float baseYCoordinate;
        protected float tileSideLength;

        #region Private variables
        protected Vector3Int currentPlayerLocation;
        protected float horizontalMultiplier;
        protected int boxInPlaceCount;
        protected Level currentLevel;
        
        protected int boxMoveCount;
        protected int playerMoveCount;
        
        protected int currentTime;
        protected double timeDelta;
        protected bool gameTurnedOn;

        private AudioManager.AudioEffectClip moveClip;
        #endregion

        public delegate void LevelFinishEvent();
        public LevelFinishEvent onLevelFinish;
        public delegate void MovedBoxEvent(int boxMoveCount);
        public MovedBoxEvent onBoxMoved;
        public delegate void MovedPlayerEvent(int playerMoveCount);
        public MovedPlayerEvent onPlayerMoved;
        public delegate void TimePassedEvent(int time);
        public TimePassedEvent onTimePassed;

        //TODO: Remove this later! For development use only!
        private Vector3Int lastMovementVector;
        private bool movedABoxLastRound;

        //TODO: Remove this later! For development use only!
        public void RewindLastMove() {
            var previouslyMovedABox = false;
            
            if (movedABoxLastRound) {
                previouslyMovedABox = true;

                if (currentLevel.levelLayout[currentPlayerLocation.y + lastMovementVector.y][currentPlayerLocation.x + lastMovementVector.x] == Level.Tile.BoxArea)
                    tilemap.SetTile(currentPlayerLocation + lastMovementVector, boxArea);
                else
                    tilemap.SetTile(currentPlayerLocation + lastMovementVector, floor);
            }
            MovePlayer(-lastMovementVector);

            if(previouslyMovedABox)
                tilemap.SetTile(currentPlayerLocation + -lastMovementVector, box);
        }

        public void Update() {
            if (gameObject.activeSelf && gameTurnedOn) {
                timeDelta -= Time.deltaTime;
                if (timeDelta <= 0) {
                    timeDelta += 1f;
                    currentTime++;
                    
                    onTimePassed?.Invoke(currentTime);
                }
            }
        }

        public void RestartTimer() {
            timeDelta = 1d;
            currentTime = 0;
        }

        public virtual void MovePlayer(Vector3Int direction) {
            moveClip = AudioManager.AudioEffectClip.None;
            if(CollisionCheck(currentPlayerLocation, direction)) {
                //TODO: Remove this later! For development use only!
                lastMovementVector = direction;
                
                if(moveClip != AudioManager.AudioEffectClip.None)
                    AudioManager.Instance.PlayAudioEffect(moveClip);
                playerMoveCount++;
                onPlayerMoved(playerMoveCount);
                if (currentLevel.levelLayout[currentPlayerLocation.y][currentPlayerLocation.x] == Level.Tile.BoxArea || currentLevel.levelLayout[currentPlayerLocation.y][currentPlayerLocation.x] == Level.Tile.BoxOnBoxArea)
                    tilemap.SetTile(currentPlayerLocation, boxArea);
                else
                    tilemap.SetTile(currentPlayerLocation, floor);

                currentPlayerLocation += direction;
                
                if(currentLevel.levelLayout[currentPlayerLocation.y][currentPlayerLocation.x] == Level.Tile.BoxArea || currentLevel.levelLayout[currentPlayerLocation.y][currentPlayerLocation.x] == Level.Tile.BoxOnBoxArea)
                    tilemap.SetTile(currentPlayerLocation, playerOnBoxArea);
                else
                    tilemap.SetTile(currentPlayerLocation, player);
            }
        }

        private bool CollisionCheck(Vector3Int position, Vector3Int direction) {
            var newPlayerLocation = position + direction;
            var boxPlacedOnBoxArea = false;
            
            if (tilemap.GetTile(newPlayerLocation) == wall || tilemap.GetTile(newPlayerLocation) == empty || tilemap.GetTile(newPlayerLocation) == null)
                return false;
            if (tilemap.GetTile(newPlayerLocation) == box || tilemap.GetTile(newPlayerLocation) == boxOnBoxArea)
            {
                if (tilemap.GetTile(newPlayerLocation + direction) == wall || tilemap.GetTile(newPlayerLocation + direction) == box || tilemap.GetTile(newPlayerLocation + direction) == boxOnBoxArea || tilemap.GetTile(newPlayerLocation + direction) == empty || tilemap.GetTile(newPlayerLocation + direction) == null)
                    return false;
                
                if (tilemap.GetTile(newPlayerLocation + direction) == boxArea) {
                    moveClip = AudioManager.AudioEffectClip.BoxMovedInPlace;
                    boxInPlaceCount++;
                    boxPlacedOnBoxArea = true;
                    tilemap.SetTile(newPlayerLocation + direction, boxOnBoxArea);
                }
                if (tilemap.GetTile(newPlayerLocation) == boxOnBoxArea)
                    boxInPlaceCount--;
                if (boxInPlaceCount == currentLevel.boxCount) {
                    moveClip = AudioManager.AudioEffectClip.LevelFinished;
                    LevelFinished();
                }

                if(!boxPlacedOnBoxArea)
                    tilemap.SetTile(newPlayerLocation + direction, box);

                movedABoxLastRound = true;
                boxMoveCount++;
                onBoxMoved(boxMoveCount);
                if (moveClip != AudioManager.AudioEffectClip.BoxMovedInPlace && moveClip != AudioManager.AudioEffectClip.LevelFinished)
                    moveClip = AudioManager.AudioEffectClip.BoxMoved;
                return true;
            }
            
            movedABoxLastRound = false;
            return true;
        }

        protected virtual void LevelFinished() {
            AudioManager.Instance.StopMusic();
            gameTurnedOn = false;
            onLevelFinish();
        }

        public void WindowScaleUpdate() {
            WindowScaleUpdate(currentLevel.levelHeight, currentLevel.levelWidth);            
        }
        
        public void WindowScaleUpdate(int levelHeight, int levelWidth) {
            var baseTileAnchor = new Vector3(0, 0, 0);

            var currentTileSideLength = Screen.height / 10f;
            var horizontalTileCount = (Screen.width - horizontalMargin * 2) / currentTileSideLength * horizontalTilemapSpan;
            var horizontalScale = horizontalTileCount / levelWidth;
            var verticalTileCount = 10f - ((10f / Screen.height) * 2 * verticalMargin);
            var verticalScale = verticalTileCount / levelHeight;
            var scale = Math.Min(horizontalScale, verticalScale);
            var newTileSideLength = currentTileSideLength * scale;
            tileSideLength = newTileSideLength;
            
            baseTileAnchor.x -= (Screen.width / newTileSideLength) * horizontalMultiplier - 0.5f;
            baseTileAnchor.x += (((Screen.width / newTileSideLength) * horizontalTilemapSpan) - levelWidth) * 0.5f;
            baseTileAnchor.y -= (0.5f * levelHeight) - 0.5f;

            baseXCoordinate = (Screen.width / 2f) - (newTileSideLength / 2f);
            baseYCoordinate = (Screen.height / 2f) - (newTileSideLength / 2f);
            baseXCoordinate += newTileSideLength * baseTileAnchor.x;
            baseYCoordinate += newTileSideLength * baseTileAnchor.y;
            
            tilemap.tileAnchor = baseTileAnchor;

            var vectorScale = new Vector3(scale, scale);
            grid.transform.localScale = vectorScale;
        }

        public void UpdateLevelSize(int levelHeight, int levelWidth) {
            if (horizontalTilemapSpan >= 0.5f)
                horizontalMultiplier = horizontalTilemapSpan - 0.5f;
            else
                horizontalMultiplier = horizontalTilemapSpan - 0.5f;
            
            WindowScaleUpdate(levelHeight, levelWidth);
        }

        public void LoadLevel(Level level) {
            currentLevel = level;
            
            if (playMusic) {
                if (level.id == LevelRegistry.GetLevelCount() && currentLevel.difficultyLevel != Level.Difficulty.None)
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
                            AudioManager.Instance.PlayMusic(AudioManager.MusicClip.PlayerLevelMusic);
                            break;
                    }
                }
            }

            gameTurnedOn = true;
            boxMoveCount = 0;
            playerMoveCount = 0;
            
            UpdateLevelSize(currentLevel.levelHeight, currentLevel.levelWidth);
            boxInPlaceCount = 0;
            tilemap.ClearAllTiles();
            
            var cursorPosition = new Vector3Int(0, 0, 0);
            foreach (var row in currentLevel.levelLayout) {
                cursorPosition.x = 0;
                
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                foreach(var tile in row)
                {
                    switch (tile)
                    {
                        case Level.Tile.Wall:
                            tilemap.SetTile(cursorPosition, wall);
                            break;
                        case Level.Tile.Player:
                            tilemap.SetTile(cursorPosition, player);
                            currentPlayerLocation = cursorPosition;
                            break;
                        case Level.Tile.Floor:
                            tilemap.SetTile(cursorPosition, floor);
                            break;
                        case Level.Tile.Box:
                            tilemap.SetTile(cursorPosition, box);
                            break;
                        case Level.Tile.BoxArea:
                            tilemap.SetTile(cursorPosition, boxArea);
                            break;
                        case Level.Tile.Empty:
                            tilemap.SetTile(cursorPosition, empty);
                            break;
                        case Level.Tile.BoxOnBoxArea:
                            boxInPlaceCount++;
                            tilemap.SetTile(cursorPosition, boxOnBoxArea);
                            break;
                        case Level.Tile.None:
                            tilemap.SetTile(cursorPosition, null);
                            break;
                        case Level.Tile.PlayerOnBoxArea:
                            tilemap.SetTile(cursorPosition, playerOnBoxArea);
                            break;
                        default:
                            tilemap.SetTile(cursorPosition, null);
                            Debug.LogError("Invalid tile in current level: " + currentLevel.levelName);
                            break;
                    }

                    cursorPosition.x++;
                }

                cursorPosition.y++;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void LoadLevel(int levelRegistryId) {
            LoadLevel(LevelRegistry.GetLevel(levelRegistryId));
        }

        public void RestartGame() {
            boxMoveCount = 0;
            playerMoveCount = 0;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            LoadLevel(currentLevel);
        }
    }
}