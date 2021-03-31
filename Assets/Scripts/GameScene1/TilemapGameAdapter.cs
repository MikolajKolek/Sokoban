using System;
using ProgramSetup;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene1 {
    /// <summary>
    /// Acts like an adapter between the <see cref="Tilemap"/> and <see cref="Grid"/> and the game. It has the ability to load and properly scale levels, move the player, restart the game and call delegates when certain events happen.
    /// </summary>
    public class TilemapGameAdapter : MonoBehaviour {
        #region Serialized variables
        /// <summary>
        /// This serialized bool tells the <see cref="TilemapGameAdapter"/> if it should play music after it loads a level.
        /// </summary>
        [SerializeField] private bool playMusic;
        [SerializeField] protected TileBase boxOnBoxArea;
        [SerializeField] protected TileBase boxArea;
        [SerializeField] protected TileBase playerOnBoxArea;
        [SerializeField] protected TileBase player;
        [SerializeField] protected TileBase floor;
        [SerializeField] protected TileBase wall;
        [SerializeField] protected TileBase box;
        [SerializeField] protected TileBase empty;
        
        /// <summary>
        /// The <see cref="Tilemap"/> that the <see cref="TilemapGameAdapter"/> is controlling.
        /// </summary>
        [SerializeField] protected Tilemap tilemap;
        /// <summary>
        /// The <see cref="Grid"/> that the <see cref="TilemapGameAdapter"/> is controlling.
        /// </summary>
        [SerializeField] private Grid grid;
        
	    /// <summary>
        /// The <see cref="horizontalTilemapSpan"/> float tells the <see cref="TilemapGameAdapter"/> how much screen it can take up (if it's set to 1f the tilemap will take up the entire screen, if it's set to 0,5f it will take up the right half of the screen, and if it's set to 0,75f it will take up the right 75% of the screen.
        /// </summary>
        [SerializeField] private float horizontalTilemapSpan = 1f;
        /// <summary>
        /// The margin in pixels that is kept from the horizontal edges of the <see cref="Tilemap"/> as set by <see cref="horizontalTilemapSpan"/>.
        /// </summary>
        [SerializeField] private int horizontalMargin;
	    /// <summary>
        /// The margin in pixels that is kept from the top and bottom of the screen.
        /// </summary>
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
        /// <summary>
        /// This event is called when the level finishes.
        /// </summary>
        public LevelFinishEvent onLevelFinish;
        public delegate void MovedBoxEvent(int boxMoveCount);
        /// <summary>
        /// This event is called each time a box is moved.
        /// </summary>
        public MovedBoxEvent onBoxMoved;
        public delegate void MovedPlayerEvent(int playerMoveCount);
        /// <summary>
        /// This event is called each time the player moves.
        /// </summary>
        public MovedPlayerEvent onPlayerMoved;
        public delegate void TimePassedEvent(int time);
        /// <summary>
        /// This event is called every time a full second passes from the loading of the level.
        /// </summary>
        public TimePassedEvent onTimePassed;

        /// <summary>
        /// The Update function runs every frame and keeps the timer running. When it passes a full second, it calls <see cref="onTimePassed"/>.
        /// </summary>
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
        
        /// <summary>
        /// Restarts the in-game timer.
        /// </summary>
        public void RestartTimer() {
            timeDelta = 1d;
            currentTime = 0;
        }

        /// <summary>
        /// Moves the player in the given direction if it is possible.
        /// </summary>
        /// <param name="direction">The direction the player is moving in.</param>
        public virtual void MovePlayer(Vector3Int direction) {
            moveClip = AudioManager.AudioEffectClip.None;
            if(CollisionCheck(currentPlayerLocation, direction)) {

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

        /// <summary>
        /// Checks if a player can move in a direction and also handles the interactions between the boxes and the player.
        /// </summary>
        /// <param name="position">The player's current position.</param>
        /// <param name="direction">The direction the player is going in.</param>
        /// <returns>Returns true if the player can move in <see cref="direction"/>, false if the player can't move there.</returns>
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

                boxMoveCount++;
                onBoxMoved(boxMoveCount);
                if (moveClip != AudioManager.AudioEffectClip.BoxMovedInPlace && moveClip != AudioManager.AudioEffectClip.LevelFinished)
                    moveClip = AudioManager.AudioEffectClip.BoxMoved;
                return true;
            }
            
            return true;
        }

        /// <summary>
        /// Called when the level is finished. Shuts down the game and calls <see cref="onLevelFinish"/>.
        /// </summary>
        protected virtual void LevelFinished() {
            AudioManager.Instance.StopMusic();
            gameTurnedOn = false;
            onLevelFinish();
        }

        /// <summary>
        /// The version of <see cref="WindowScaleUpdate()"></see> that doesn't require any parameters because it uses the level height and level width from <see cref="currentLevel"/>.
        /// </summary>
        public void WindowScaleUpdate() {
            WindowScaleUpdate(currentLevel.levelHeight, currentLevel.levelWidth);            
        }

        /// <summary>
        /// WindowScaleUpdate updates the scale of the grid and the <see cref="Tilemap"/>'s tileAnchor to fit perfectly on the screen in accordance with <see cref="horizontalTilemapSpan"/>, <see cref="horizontalMargin"/> and <see cref="verticalMargin"/>.
        /// </summary>
        /// <param name="levelHeight">The height of the <see cref="Tilemap"/> that you want to see on the screen.</param>
        /// <param name="levelWidth">The width of the <see cref="Tilemap"/> that you want to see on the screen.</param>
        public void WindowScaleUpdate(int levelHeight, int levelWidth) {
            //The tileAnchor is an offset at which the tile with the position x=0, y=0 is located on the screen.
            //By default the tileAnchor is always set to 0, 0, 0, which means the tile at position x=0, y=0
            //is located at the exact center of the screen.
            //If we were to change the tileAnchor's value to 1, 1, 0 for example, the location of the tile at the exact center of the screen
            //would change to x=-1, y=-1 and the center of the tilemap would be offset by exactly 1 tile to the top and one tile to the right.
	        var baseTileAnchor = new Vector3(0, 0, 0);

            //The currentTileSideLength variable is set to the current length of a tile's side. Tiles are always square by default, and
            //although I couldn't find any documentation stating this, I found that by default in all possible screen orientations
            //a Tilemap fits exactly 10 tiles vertically on the screen.
            var currentTileSideLength = Screen.height / 10f;
            //The horizontalTileCount variable stores the information of how many tiles currently fit on the screen in the area set by
            //horizontalTilemapSpan excluding the horizontal margins. 
            var horizontalTileCount = (Screen.width - horizontalMargin * 2) / currentTileSideLength * horizontalTilemapSpan;
            //The horizontalScale is a scale value that if the scale was set to, exactly levelWidth tiles would fit on the area set by
            //horizontalTilemapSpan and horizontalMargin
            var horizontalScale = horizontalTileCount / levelWidth;
            //The verticalTileCount variable stores information on exactly how many tiles can fit vertically on the screen excluding the
            //vertical margins
            var verticalTileCount = 10f - ((10f / Screen.height) * 2 * verticalMargin);
            //The verticalScale is a scale value that if the scale was set to, exactly levelHeight tiles would fit on the area set by
            //verticalMarginMargin
            var verticalScale = verticalTileCount / levelHeight;
            //The final scale value selects the lower one of horizontalScale and verticalScale. This way, at all times, at least
            //levelHeight tiles will fit on the screen vertically, and at least levelWidth tiles will fit on the screen horizontally
            var scale = Math.Min(horizontalScale, verticalScale);
            
            //The newTileSideLength variable stores the information on how long the sides of a tile will be with the new scale value.
            var newTileSideLength = currentTileSideLength * scale;
            tileSideLength = newTileSideLength;

            //The baseTileAnchor is offset to the left the exact number that is needed for the x=0 column to be at the left edge of the area
            //the borders of which are set by horizontalTilemapSpan and horizontalMargin
            baseTileAnchor.x -= (Screen.width / newTileSideLength) * horizontalMultiplier - 0.5f;
            //The baseTileAnchor is offset to the left the exact number that is needed for it to be located at the exact centre of the area 
            //the borders of which are set by horizontalTilemapSpan and horizontalMargin
            baseTileAnchor.x += (((Screen.width / newTileSideLength) * horizontalTilemapSpan) - levelWidth) * 0.5f;
            //The baseTileAnchor is offset to the left by the exact number that is needed for it to be located at the exact vertical center
            //of the screen.
            //The 0.5f needs to be subtracted because when the tileAnchor's y coordinate is dividable by 1, an odd number of tiles can
            //be displayed on the screen vertically, while when the tileAnchor's y coordinate isn't dividable by 1 but is dividable by 0,5
            //an even number of tiles can be displayed on the screen vertically.
            baseTileAnchor.y -= (0.5f * levelHeight) - 0.5f;

            //The baseXCoordinate variable is set to the exact x coordinate of the left corner of the tile that with the tileAnchor.x = 0
            //would be located at the center of the screen
            baseXCoordinate = (Screen.width / 2f) - (newTileSideLength / 2f);
            //The baseXCoordinate is offset in such a way that it is now set to the exact x coordinate of the left corner of the tile that will
            //be located at x = 0 on the tilemap when the baseTilemapAnchor becomes the new tilemapAnchor
            baseXCoordinate += newTileSideLength * baseTileAnchor.x;
            //The baseYCoordinate variable is set to the exact y coordinate of the bottom corner of the tile that with the tileAnchor.y = 0
            //would be located at the center of the screen
            baseYCoordinate = (Screen.height / 2f) - (newTileSideLength / 2f);
            //The baseYCoordinate is offset in such a way that it is now set to the exact y coordinate of the bottom corner of the tile that will
            //be located at y = 0 on the tilemap when the baseTilemapAnchor becomes the new tilemapAnchor
            baseYCoordinate += newTileSideLength * baseTileAnchor.y;
            
            //The actual tilemap's tileAnchor is set to the baseTileAnchor.
            tilemap.tileAnchor = baseTileAnchor;

            //And the scale is set to the new scale.
            var vectorScale = new Vector3(scale, scale);
            grid.transform.localScale = vectorScale;
        }
        
        /// <summary>
        /// Called each time a level size updates. It sets the value of the horizontalMultiplier and then calls <see cref="WindowScaleUpdate()"/>.
        /// </summary>
        /// <param name="levelHeight">The current level's height.</param>
        /// <param name="levelWidth">The current level's width.</param>
        public void UpdateLevelSize(int levelHeight, int levelWidth) {
            //The horizontalMultiplier is set to where the left border of the tilemap will be with the current horizontalMultiplier
            //(but excluding the current horizontalMargin). It is stored as a float, and for example:
            //0f would mean the left border is at the middle of the screen
            //0.25f would mean that the border is a quarter of the screen offset from the center of the screen to the right, so it fills 75% of the screen.
            
            //For example, with horizontalTilemapSpan = 0.25f the tilemap will only take up the right 25% of the screen, meaning that
            //the horizontalMultiplier will be equal to -0.25f.
            if (horizontalTilemapSpan >= 0.5f)
                horizontalMultiplier = horizontalTilemapSpan - 0.5f;
            else
                horizontalMultiplier = horizontalTilemapSpan - 0.5f;
            
            WindowScaleUpdate(levelHeight, levelWidth);
        }

        /// <summary>
        /// Loads the level it is provided as a parameter and starts playing appropriate music if <see cref="playMusic"/> is true.
        /// </summary>
        /// <param name="level"></param>
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
        /// <summary>
        /// Loads the level with the given levelRegistryID
        /// </summary>
        /// <param name="levelRegistryId">The ID of the level you want to load in the <see cref="LevelRegistry"/>.</param>
        public void LoadLevel(int levelRegistryId) {
            LoadLevel(LevelRegistry.GetLevel(levelRegistryId));
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame() {
            boxMoveCount = 0;
            playerMoveCount = 0;
            
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            LoadLevel(currentLevel);
        }
    }
}