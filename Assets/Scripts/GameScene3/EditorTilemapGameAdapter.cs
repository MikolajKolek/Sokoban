using GameScene1;
using UnityEngine;

namespace GameScene3 {
    public class EditorTilemapGameAdapter : TilemapGameAdapter {
        public delegate void PlayerPlacedEvent();
        public PlayerPlacedEvent onPlayerPlaced;
        public delegate void PlayerRemovedEvent();
        public PlayerRemovedEvent onPlayerRemoved;
        
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

        public Vector2 GetBaseCoordinates() {
            var baseCoordinates = new Vector2(baseXCoordinate, baseYCoordinate);
            return baseCoordinates;
        }

        public float GetTileSideLength() {
            return tileSideLength;
        }

        public void ExportLevel(string levelName, int levelHeight, int levelWidth) {
            var level = LevelRegistry2.SerializeTilemapToLevel(tilemap, levelName, levelHeight, levelWidth);
            LevelRegistry2.SaveLevel(level);
            tilemap.ClearAllTiles();
        }
        
        public Level GetLevel(int levelHeight, int levelWidth) {
            var level = LevelRegistry2.SerializeTilemapToLevel(tilemap, "PlayTestingLevel", levelHeight, levelWidth);
            return level;
        }

        public void ClearAllTiles() {
            tilemap.ClearAllTiles();
        }
    }
}