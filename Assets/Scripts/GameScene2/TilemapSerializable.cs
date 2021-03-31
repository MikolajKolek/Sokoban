using System;
using System.Collections.Generic;
using GameScene1;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene2 {
    /// <summary>
    /// A way to serialize Unity's <see cref="Tilemap"/> element using <see cref="JsonUtility"/>.toJson().
    /// </summary>
    [Serializable]
    public class TilemapSerializable {
        private TileBase boxOnBoxArea;
        private TileBase boxArea;
        private TileBase player;
        private TileBase playerOnBoxArea;
        private TileBase floor;
        private TileBase wall;
        private TileBase box;
        private TileBase empty;

        public List<Level.Tile> tilemapMap;
        public Vector3 tileAnchor;
        public int tilemapHeight;
        public int tilemapWidth;
        
        /// <summary>
        /// Initializes the tilemap deserialization by giving the <see cref="TilemapSerializable"/> all the <see cref="TileBase"/>s required.
        /// </summary>
        public void InitializeTilemapDeserialization(TileBase boxOnBoxAreaTile, TileBase boxAreaTile, TileBase playerTile, TileBase playerOnBoxAreaTile, TileBase floorTile, TileBase wallTile, TileBase boxTile, TileBase emptyTile) {
            boxOnBoxArea = boxOnBoxAreaTile;
            boxArea = boxAreaTile;
            player = playerTile;
            playerOnBoxArea = playerOnBoxAreaTile;
            floor = floorTile;
            wall = wallTile;
            box = boxTile;
            empty = emptyTile;
        }
        
        public TilemapSerializable(TileBase boxOnBoxArea, TileBase boxArea, TileBase player, TileBase playerOnBoxArea, TileBase floor, TileBase wall, TileBase box, TileBase empty) {
            this.boxOnBoxArea = boxOnBoxArea;
            this.boxArea = boxArea;
            this.player = player;
            this.playerOnBoxArea = playerOnBoxArea;
            this.floor = floor;
            this.wall = wall;
            this.box = box;
            this.empty = empty;
        }

        /// <summary>
        /// Serializes the given tilemap into itself.
        /// </summary>
        /// <param name="serializedTilemap">The tilemap that you want to serialize.</param>
        /// <param name="height">The tilemap's height.</param>
        /// <param name="width">The tilemap's width.</param>
        public void SerializeTilemap(Tilemap serializedTilemap, int height, int width) {
            tilemapHeight = height;
            tilemapWidth = width;

            var minCellBounds = serializedTilemap.cellBounds.min;
            
            var cursor = new Vector3Int(minCellBounds.x, minCellBounds.y, minCellBounds.z);
            var map = new List<List<Level.Tile>>();
            for (var i = 0; i < tilemapHeight; i++)
                map.Add(new List<Level.Tile>(tilemapWidth));

            for (var i = 0; i < tilemapWidth; i++) {
                cursor.y = minCellBounds.y;
                for (var j = 0; j < tilemapHeight; j++) {
                    if(serializedTilemap.GetTile(cursor) == boxOnBoxArea)
                        map[j].Add(Level.Tile.BoxOnBoxArea);
                    else if (serializedTilemap.GetTile(cursor) == boxArea)
                        map[j].Add(Level.Tile.BoxArea);
                    else if (serializedTilemap.GetTile(cursor) == player)
                        map[j].Add(Level.Tile.Player);
                    else if (serializedTilemap.GetTile(cursor) == floor)
                        map[j].Add(Level.Tile.Floor);
                    else if (serializedTilemap.GetTile(cursor) == wall)
                        map[j].Add(Level.Tile.Wall);
                    else if (serializedTilemap.GetTile(cursor) == box)
                        map[j].Add(Level.Tile.Box);
                    else if(serializedTilemap.GetTile(cursor) == playerOnBoxArea)
                        map[j].Add(Level.Tile.PlayerOnBoxArea);
                    else
                        map[j].Add(Level.Tile.Empty);
                    
                    cursor.y++;
                }

                cursor.x++;
            }

            tilemapMap = new List<Level.Tile>();
            foreach (var row in map) {
                tilemapMap.AddRange(row);
            }
        }

        /// <summary>
        /// Deserializes the <see cref="TilemapSerializable"/> into a <see cref="Tilemap"/> object.
        /// </summary>
        /// <param name="baseTilemap">The tilemap you want to put the deserialized tilemap into.</param>
        /// <returns>The deserialized tilemap</returns>
        public Tilemap DeserializeTilemap(Tilemap baseTilemap) {
            baseTilemap.tileAnchor = tileAnchor;
            
            for (var i = 0; i < tilemapWidth; i++) {
                for (var j = 0; j < tilemapHeight; j++) {
                    var tile = tilemapMap[j * tilemapWidth + i];
                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (tile) {
                        case Level.Tile.BoxOnBoxArea:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), boxOnBoxArea);
                            break;
                        case Level.Tile.Wall:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), wall);
                            break;
                        case Level.Tile.Floor:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), floor);
                            break;
                        case Level.Tile.Player:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), player);
                            break;
                        case Level.Tile.PlayerOnBoxArea:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), playerOnBoxArea);
                            break;
                        case Level.Tile.Box:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), box);
                            break;
                        case Level.Tile.BoxArea:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), boxArea);
                            break;
                        case Level.Tile.Empty:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), empty);
                            break;
                        default:
                            baseTilemap.SetTile(new Vector3Int(i, j, 0), null);
                            break;
                    }
                }
            }
            
            return baseTilemap;
        }
    }
}