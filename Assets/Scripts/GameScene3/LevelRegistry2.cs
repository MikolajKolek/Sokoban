using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using GameScene1;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene3 {
    public static class LevelRegistry2 {
        #region Variables
        private static TileBase boxOnBoxArea;
        private static TileBase boxArea;
        private static TileBase player;
        private static TileBase floor;
        private static TileBase wall;
        private static TileBase box;
        private static TileBase empty;

        private static bool registryInitialized;
        private static readonly List<Level> Registry = new List<Level>();
        #endregion
        
        #region Methods
        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        public static void InitializeLevelList(TileBase boxOnBoxArea, TileBase boxArea, TileBase player, TileBase floor, TileBase wall, TileBase box, TileBase empty) {
            if (!registryInitialized) {
                registryInitialized = true;
                LevelRegistry2.boxOnBoxArea = boxOnBoxArea;
                LevelRegistry2.boxArea = boxArea;
                LevelRegistry2.player = player;
                LevelRegistry2.floor = floor;
                LevelRegistry2.wall = wall;
                LevelRegistry2.box = box;
                LevelRegistry2.empty = empty;

                var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                @"\Sokoban\levels";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                var files = Directory.GetFiles(directory, "*.txt");

                foreach (var file in files) {
                    var fileReader = new StreamReader(file);
                    var id = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                    var levelName = ProcessLine(fileReader.ReadLine());
                    var boxCount = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                    var levelWidth = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                    var levelHeight = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                    fileReader.ReadLine();

                    var levelMap = new List<List<Level.Tile>>(new List<Level.Tile>[levelHeight]);
                    for (var i = 0; i < levelHeight; i++) {
                        var line = fileReader.ReadLine();

                        if (line != null) {

                            var levelRow = new List<Level.Tile>();
                            foreach (var c in line) {

                                switch (c) {
                                    case '#':
                                        levelRow.Add(Level.Tile.Wall);
                                        break;
                                    case ' ':
                                        levelRow.Add(Level.Tile.Floor);
                                        break;
                                    case 'P':
                                        levelRow.Add(Level.Tile.Player);
                                        break;
                                    case '$':
                                        levelRow.Add(Level.Tile.Box);
                                        break;
                                    case '*':
                                        levelRow.Add(Level.Tile.BoxArea);
                                        break;
                                    case '.':
                                        levelRow.Add(Level.Tile.Empty);
                                        break;
                                    case '&':
                                        levelRow.Add(Level.Tile.BoxOnBoxArea);
                                        break;
                                    default:
                                        levelRow.Add(Level.Tile.Empty);
                                        Debug.LogError("Invalid tile in level " + levelName);
                                        break;
                                }
                            }

                            levelMap[levelHeight - i - 1] = levelRow;
                        }
                    }

                    var level = new Level(id, levelName, 0, Level.Difficulty.None, boxCount, levelWidth, levelHeight,
                        levelMap);
                    Registry.Add(level);
                }

                Registry.Sort();
            }
        }

        public static void DeleteLevel(int levelID) {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban\levels\";
            
            for(var i = levelID + 1; i < Registry.Count; i++) {
                Registry[i] = Registry[i].DecrementID();
                SaveLevel(i);
            }
            Registry.RemoveAt(levelID);

            if (Registry.Count > 0) {
                var lastLevelId = (Registry.Count + 1).ToString();
                while (lastLevelId.Length != 3)
                    lastLevelId = lastLevelId.Insert(0, "0");

                File.Delete(directory + @"Level_" + lastLevelId + ".txt");
            }
        }

        private static void SaveLevel(int levelID) {
            var level = GetLevel(levelID);
            
            var stringId = levelID.ToString();
            while (stringId.Length != 3)
                stringId = stringId.Insert(0, "0");
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban\levels\";
            var file = directory + @"Level_" + stringId + ".txt";

            var outputString = "";
            outputString += "Id: " + level.id + "\n";
            outputString += "Name: " + level.levelName + "\n";
            outputString += "Box count: " + level.boxCount + "\n";
            outputString += "Width: " + level.levelWidth + "\n";
            outputString += "Height: " + level.levelHeight + "\n";
            outputString += "Level map:\n";
            
            for(var i = level.levelHeight - 1; i >= 0; i--)
            {
                foreach (var element in level.levelLayout[i]) {
                    outputString += (char) element;
                }

                outputString += "\n";
            }

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            var fs = File.Create(file);
            var bytes = Encoding.UTF8.GetBytes(outputString);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        public static Level GetLevel(int levelRegistryId) {
            return Registry[levelRegistryId];
        }

        public static int GetLevelCount() {
            return Registry.Count;
        }

        public static void SaveLevel(Level level) {
            Registry.Add(level);
            
            var stringId = level.id.ToString();
            while (stringId.Length != 3)
                stringId = stringId.Insert(0, "0");
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Sokoban\levels\";
            var file = directory + @"Level_" + stringId + ".txt";

            var outputString = "";
            outputString += "Id: " + level.id + "\n";
            outputString += "Name: " + level.levelName + "\n";
            outputString += "Box count: " + level.boxCount + "\n";
            outputString += "Width: " + level.levelWidth + "\n";
            outputString += "Height: " + level.levelHeight + "\n";
            outputString += "Level map:\n";
            
            for(var i = level.levelHeight - 1; i >= 0; i--)
            {
                foreach (var element in level.levelLayout[i]) {
                    outputString += (char) element;
                }

                outputString += "\n";
            }

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            var fs = File.Create(file);
            var bytes = Encoding.UTF8.GetBytes(outputString);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
        
        public static Level SerializeTilemapToLevel(Tilemap tilemap, string levelName, int levelHeight, int levelWidth) {
            var boxCount = 0;
            var levelLayout = new List<List<Level.Tile>>();

            for (var i = 0; i < levelHeight; i++) {
                var row = new List<Level.Tile>();
                for (var j = 0; j < levelWidth; j++) {
                    var currentTile = tilemap.GetTile(new Vector3Int(j, i, 0));
                    if (currentTile == boxOnBoxArea) {
                        row.Add(Level.Tile.BoxOnBoxArea);
                        boxCount++;
                    }
                    else if(currentTile == boxArea)
                        row.Add(Level.Tile.BoxArea);
                    else if(currentTile == player)
                        row.Add(Level.Tile.Player); 
                    else if(currentTile == floor)
                        row.Add(Level.Tile.Floor);
                    else if(currentTile == wall)
                        row.Add(Level.Tile.Wall);
                    else if (currentTile == box) {
                        row.Add(Level.Tile.Box);
                        boxCount++;
                    }
                    else if(currentTile == empty)
                        row.Add(Level.Tile.Empty);
                    else {
                        row.Add(Level.Tile.None);
                    }
                }
                
                levelLayout.Add(row);
            }
            
            var outputLevel = new Level(GetLevelCount() + 1, levelName, 0, Level.Difficulty.None, boxCount, levelWidth, levelHeight, levelLayout);
            return outputLevel;
        }

        public static bool LevelExists(string profileName) {
            foreach(var element in Registry)
                if (element.levelName == profileName)
                    return true;

            return false;
        }
        
        private static string ProcessLine(string line) {
            var colonFound = false; 
            
            for (var i = 0; i < line.Length; i++) {
                if (line[i] != ' ' && colonFound) {
                    line = line.Remove(0, i);

                    break;
                }
                        
                if (line[i] == ':')
                    colonFound = true;
            }
            
            return line;
        }

        public static void RenameLevel(int levelID, string newName) {
            Registry[levelID] = Registry[levelID].ChangeName(newName);
            SaveLevel(levelID);
        }
        #endregion
    }
}