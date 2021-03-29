using System;
using System.Collections.Generic;
using System.IO;
using GameScene2;
using UnityEngine;

namespace GameScene1 {
    public static class LevelRegistry {
        #region Variables
        private static readonly List<Level> Registry = new List<Level>();
        #endregion

        #region Methods
        public static void InitializeLevelList() {
            var files = Directory.GetFiles("levels", "*.txt");

            foreach (var file in files) {
                var fileReader = new StreamReader(file);
                var id = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                var levelName = ProcessLine(fileReader.ReadLine());
                var time = Convert.ToInt32(ProcessLine(fileReader.ReadLine()));
                
                var difficultyString = ProcessLine(fileReader.ReadLine()?.ToLower());
                Level.Difficulty difficulty;
                switch (difficultyString) {
                    case "easy":
                        difficulty = Level.Difficulty.Easy;
                        break;
                    case "medium":
                        difficulty = Level.Difficulty.Medium;
                        break;
                    case "hard":
                        difficulty = Level.Difficulty.Hard;
                        break;
                    default:
                        difficulty = Level.Difficulty.Easy;
                        levelName += "Invalid Difficulty Level";
                        Debug.LogError("Invalid difficulty level set for file " + file);
                        break;
                }
                
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
                                default:
                                    levelRow.Add(Level.Tile.Empty);
                                    Debug.LogError("Invalid tile in level " + levelName);
                                    break;
                            }
                        }
                        
                        levelMap[levelHeight - i - 1] = levelRow;
                    }
                }

                var level = new Level(id, levelName, time, difficulty, boxCount, levelWidth, levelHeight, levelMap);
                Registry.Add(level);
            }

            Registry.Sort();
        }

        public static Level GetLevel(int levelRegistryId) {
            return Registry[levelRegistryId];
        }

        public static int GetLevelCount() {
            return Registry.Count;
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
        #endregion
    }
}