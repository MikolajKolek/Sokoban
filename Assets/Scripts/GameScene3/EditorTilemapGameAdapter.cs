using GameScene1;
using UnityEngine;

namespace GameScene3 {
    /// <summary>
    /// Inherits from <see cref="TilemapGameAdapter"/> and modifies it so it can be used for level creation.
    /// </summary>
    public class EditorTilemapGameAdapter : TilemapGameAdapter {
        public delegate void PlayerPlacedEvent();
        public PlayerPlacedEvent onPlayerPlaced;
        public delegate void PlayerRemovedEvent();
        public PlayerRemovedEvent onPlayerRemoved;
        
        /// <summary>
        /// Sets the tile at the given location to the given tile.
        /// </summary>
        /// <param name="position">The position you want to place the tile in</param>
        /// <param name="tile">The tile that you want to place</param>
        public void SetTile(Vector3Int position, Level.Tile tile) {
            if (tilemap.GetTile(position) == player && tile != Level.Tile.Player)
                onPlayerRemoved();
            
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (tile) {
                case Level.Tile.Wall:
                    tilemap.SetTile(position, wall);
                    break;
                case Level.Tile.Floor:
                    tilemap.SetTile(position, floor);
                    break;
                case Level.Tile.Player:
                    onPlayerPlaced();
                    
                    if(tilemap.GetTile(currentPlayerLocation) == player)
                        tilemap.SetTile(currentPlayerLocation, floor);
                    
                    currentPlayerLocation = position;
                    tilemap.SetTile(position, player);
                    break;
                case Level.Tile.Box:
                    tilemap.SetTile(position, box);
                    break;
                case Level.Tile.BoxArea:
                    tilemap.SetTile(position, boxArea);
                    break;
                case Level.Tile.Empty:
                    tilemap.SetTile(position, empty);
                    break;
                case Level.Tile.BoxOnBoxArea:
                    tilemap.SetTile(position, boxOnBoxArea);
                    break;
                default:
                    tilemap.SetTile(position, null);
                    break;
            }
        }

        /// <summary>
        /// Gets the baseXCoordinate and baseYCoordinate and returns them
        /// </summary>
        /// <returns>A <see cref="Vector2"/> containing the baseXCoordinate and baseYCoordinate</returns>
        public Vector2 GetBaseCoordinates() {
            var baseCoordinates = new Vector2(baseXCoordinate, baseYCoordinate);
            return baseCoordinates;
        }

        /// <summary>
        /// Gets the current length of a tile's side.
        /// </summary>
        /// <returns>The tile side length.</returns>
        public float GetTileSideLength() {
            return tileSideLength;
        }

        /// <summary>
        /// Exports the level and puts it into levelRegistry2.
        /// </summary>
        /// <param name="levelName">The new level's name</param>
        /// <param name="levelHeight">The new level's height</param>
        /// <param name="levelWidth">The new level's width</param>
        public void ExportLevel(string levelName, int levelHeight, int levelWidth) {
            var level = LevelRegistry2.SerializeTilemapToLevel(tilemap, levelName, levelHeight, levelWidth);
            LevelRegistry2.SaveLevel(level);
            tilemap.ClearAllTiles();
        }
        
        /// <summary>
        /// Gets the level that is currently being created in the editor.
        /// </summary>
        /// <param name="levelHeight">The current level's height</param>
        /// <param name="levelWidth">The current level's width</param>
        /// <returns>The <see cref="Level"/> that is being created</returns>
        public Level GetLevel(int levelHeight, int levelWidth) {
            var level = LevelRegistry2.SerializeTilemapToLevel(tilemap, "PlayTestingLevel", levelHeight, levelWidth);
            return level;
        }

        /// <summary>
        /// Clears all tiles on the tilemap.
        /// </summary>
        public void ClearAllTiles() {
            tilemap.ClearAllTiles();
        }
    }
}