using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameScene1 {
    /// <summary>
    /// The <see cref="LevelRegistry"/> is used to store and manage all levels in stage 1 and stage 2.
    /// </summary>
    public static class LevelRegistry {
        #region Variables
        /// <summary>
        /// A <see cref="List{T}"/> storing <see cref="Level"/> objects representing all loaded levels.
        /// </summary>
        private static readonly List<Level> Registry = new List<Level>();
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the <see cref="Registry"/> by loading all levels from the <c>\levels\</c> folder into it and sorting them by their ID.
        /// </summary>
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

        /// <summary>
        /// Returns the <see cref="Level"/> with the passed registryID.
        /// </summary>
        /// <param name="levelRegistryId">The index in the <see cref="Registry"/> of the <see cref="Level"/> you want to get.</param>
        /// <returns>The <see cref="Level"/> at <see cref="levelRegistryId"/> in <see cref="Registry"/></returns>
        public static Level GetLevel(int levelRegistryId) {
            return Registry[levelRegistryId];
        }

        /// <summary>
        /// Returns the count of loaded levels in the <see cref="Registry"/>.
        /// </summary>
        public static int GetLevelCount() {
            return Registry.Count;
        }

        /// <summary>
        /// Processes the passed line by removing everything in it before the first colon and also removing all the spaces. It is used while loading levels to remove the parts of the level format that make it human readable.
        /// </summary>
        /// <param name="line">The line that you want to process.</param>
        /// <returns>The processed line</returns>
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