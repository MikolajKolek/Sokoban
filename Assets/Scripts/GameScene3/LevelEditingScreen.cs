using System;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene3 {
    /// <summary>
    /// Manages the level editing and creation screen on stage 3.
    /// </summary>
    public class LevelEditingScreen : MonoBehaviour {
        [SerializeField] private EditorTilemapGameAdapter levelEditorTilemapAdapter;
        [SerializeField] private TMP_Text levelWidthText;
        [SerializeField] private TMP_Text levelHeightText;

        [SerializeField] private Image floorButtonImage;
        [SerializeField] private Image wallButtonImage;
        [SerializeField] private Image boxButtonImage;
        [SerializeField] private Image boxOnBoxAreaButtonImage;
        [SerializeField] private Image boxAreaButtonImage;
        [SerializeField] private Image playerButtonImage;

        [SerializeField] private Sprite defaultFloorButtonSprite;
        [SerializeField] private Sprite defaultWallButtonSprite;
        [SerializeField] private Sprite defaultBoxButtonSprite;
        [SerializeField] private Sprite defaultBoxOnBoxAreaButtonSprite;
        [SerializeField] private Sprite defaultBoxAreaButtonSprite;
        [SerializeField] private Sprite defaultPlayerButtonSprite;

        [SerializeField] private Sprite selectedFloorButtonSprite;
        [SerializeField] private Sprite selectedWallButtonSprite;
        [SerializeField] private Sprite selectedBoxButtonSprite;
        [SerializeField] private Sprite selectedBoxOnBoxAreaButtonSprite;
        [SerializeField] private Sprite selectedBoxAreaButtonSprite;
        [SerializeField] private Sprite selectedPlayerButtonSprite;

        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameScreenManager3 gameScreenManager;

        [SerializeField] private TMP_Text exportLevelButtonText;
        [SerializeField] private Button exportLevelButton;
        [SerializeField] private TMP_Text playtestLevelButtonText;
        [SerializeField] private Button playtestLevelButton;

        [SerializeField] private GameObject noLevelNameWarningText;
        [SerializeField] private GameObject levelExportDuplicateNameWarning;
        [SerializeField] private GameObject nameLevelSubscreen;
        [SerializeField] private TMP_InputField levelNameField;

        [SerializeField] private GameObject quittingSubscreen;

        [SerializeField] private LevelSelectionScreen3 levelSelectionScreen;
        
        private int levelWidth = 3;
        private int levelHeight = 3;
        private int screenHeight;
        private int screenWidth;
        private Level.Tile selectedTileType = Level.Tile.None;

        /// <summary>
        /// Resets the tilemap and fills it with <see cref="Level.Tile.Empty"/>s
        /// </summary>
        public void Start() {
            exportLevelButton.interactable = false;
            playtestLevelButton.interactable = false;
            exportLevelButtonText.alpha = 0.5f;
            playtestLevelButtonText.alpha = 0.5f;
            
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            levelEditorTilemapAdapter.ClearAllTiles();
            
            for (var i = 0; i < levelWidth; i++) {
                for (var j = 0; j < levelHeight; j++) {
                    levelEditorTilemapAdapter.SetTile(new Vector3Int(i, j, 0), Level.Tile.Empty);
                }
            }

            levelEditorTilemapAdapter.onPlayerPlaced = PlayerPlacedEvent;
            levelEditorTilemapAdapter.onPlayerRemoved = PlayerRemovedEvent;
        }

        /// <summary>
        /// Is responsible for getting the mouse input and calling <see cref="DrawTile"/> when necessary.
        /// It also detects changes in the screen height and width and calls <see cref="levelEditorTilemapAdapter"/>'s WindowScaleUpdate to make the tilemap stay on the screen.
        /// It also detects a press on the escape key and calls <see cref="Quit"/>.
        /// </summary>
        public void Update() {
            if (screenHeight != Screen.height || screenWidth != Screen.width)
                levelEditorTilemapAdapter.WindowScaleUpdate(levelHeight, levelWidth);

            if (!nameLevelSubscreen.activeSelf && !quittingSubscreen.activeSelf) {
                if (Input.GetMouseButton(0))
                    DrawTile(Input.mousePosition, selectedTileType);
                if (Input.GetMouseButton(1))
                    DrawTile(Input.mousePosition, Level.Tile.Empty);
                if (Input.GetKeyDown(KeyCode.Escape))
                    Quit();
            }
        }

        /// <summary>
        /// Draws a tile in the given mouse position on the <see cref="levelEditorTilemapAdapter"/>/
        /// </summary>
        /// <param name="mousePosition">The current mouse position.</param>
        /// <param name="tile">The tile that you want to draw.</param>
        private void DrawTile(Vector3 mousePosition, Level.Tile tile) {
            var baseCoordinates = levelEditorTilemapAdapter.GetBaseCoordinates();
            var tileSideLength = levelEditorTilemapAdapter.GetTileSideLength();
            
            var locationOnTilemap = new Vector3Int {
                x = (int) Math.Floor((mousePosition.x - baseCoordinates.x) / tileSideLength),
                y = (int) Math.Floor((mousePosition.y - baseCoordinates.y) / tileSideLength)
            };

            if (locationOnTilemap.x < 0 || locationOnTilemap.x >= levelWidth)
                return;
            if (locationOnTilemap.y < 0 || locationOnTilemap.y >= levelHeight)
                return;
            
            if(tile != Level.Tile.None)
                levelEditorTilemapAdapter.SetTile(locationOnTilemap, tile);
        }
        
        /// <summary>
        /// Increases the level's width by one.
        /// </summary>
        public void IncreaseLevelWidth() {
            if (levelWidth + 1 > 30)
                return;
            
            levelWidth++;
            levelWidthText.text = levelWidth.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelHeight; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(levelWidth - 1, i, 0), Level.Tile.Empty);
        }
        
        /// <summary>
        /// Decreases the level's width by one.
        /// </summary>
        public void DecreaseLevelWidth() {
            if (levelWidth - 1 < 3)
                return;

            levelWidth--;
            levelWidthText.text = levelWidth.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelHeight; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(levelWidth, i, 0), Level.Tile.None);
        }

        /// <summary>
        /// Increases the leve's height by one.
        /// </summary>
        public void IncreaseLevelHeight() {
            if (levelHeight + 1 > 20)
                return;

            levelHeight++;
            levelHeightText.text = levelHeight.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelWidth; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(i, levelHeight - 1, 0), Level.Tile.Empty);
        }

        /// <summary>
        /// Decreases the level's height by one.
        /// </summary>
        public void DecreaseLevelHeight() {
            if (levelHeight - 1 < 3)
                return;

            levelHeight--;
            levelHeightText.text = levelHeight.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelWidth; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(i, levelHeight, 0), Level.Tile.None);
        }

        /// <summary>
        /// Resets the selected brush.
        /// </summary>
        private void ResetSelectedTile() {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (selectedTileType) {
                case Level.Tile.Wall:
                    wallButtonImage.overrideSprite = defaultWallButtonSprite;
                    break;
                case Level.Tile.Floor:
                    floorButtonImage.overrideSprite = defaultFloorButtonSprite;
                    break;
                case Level.Tile.Player:
                    playerButtonImage.overrideSprite = defaultPlayerButtonSprite;
                    break;
                case Level.Tile.Box:
                    boxButtonImage.overrideSprite = defaultBoxButtonSprite;
                    break;
                case Level.Tile.BoxArea:
                    boxAreaButtonImage.overrideSprite = defaultBoxAreaButtonSprite;
                    break;
                case Level.Tile.BoxOnBoxArea:
                    boxOnBoxAreaButtonImage.overrideSprite = defaultBoxOnBoxAreaButtonSprite;
                    break;
            }
        }
        
        /// <summary>
        /// Selects the floor tile as the brush.
        /// </summary>
        public void SelectFloorTile() {
            ResetSelectedTile();
            floorButtonImage.overrideSprite = selectedFloorButtonSprite;
            selectedTileType = Level.Tile.Floor;
        }

        /// <summary>
        /// Selects the wall tile as the brush.
        /// </summary>
        public void SelectWallTile() {
            ResetSelectedTile();
            wallButtonImage.overrideSprite = selectedWallButtonSprite;
            selectedTileType = Level.Tile.Wall;
        }

        /// <summary>
        /// Selects the box tile as the brush.
        /// </summary>
        public void SelectBoxTile() {
            ResetSelectedTile();
            boxButtonImage.overrideSprite = selectedBoxButtonSprite;
            selectedTileType = Level.Tile.Box;
        }

        /// <summary>
        /// Selects the boxOnBoxArea tile as the brush.
        /// </summary>
        public void SelectBoxOnBoxAreaTile() {
            ResetSelectedTile();
            boxOnBoxAreaButtonImage.overrideSprite = selectedBoxOnBoxAreaButtonSprite;
            selectedTileType = Level.Tile.BoxOnBoxArea;
        }

        /// <summary>
        /// Selects the boxArea tile as the brush.
        /// </summary>
        public void SelectBoxAreaTile() {
            ResetSelectedTile();
            boxAreaButtonImage.overrideSprite = selectedBoxAreaButtonSprite;
            selectedTileType = Level.Tile.BoxArea;
        }

        /// <summary>
        /// Selects the player tile as the brush.
        /// </summary>
        public void SelectPlayerTile() {
            ResetSelectedTile();
            playerButtonImage.overrideSprite = selectedPlayerButtonSprite;
            selectedTileType = Level.Tile.Player;
        }

        /// <summary>
        /// Opens the <see cref="nameLevelSubscreen"/>.
        /// </summary>
        public void ExportLevel() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            noLevelNameWarningText.gameObject.SetActive(false);
            levelExportDuplicateNameWarning.gameObject.SetActive(false);
            nameLevelSubscreen.gameObject.SetActive(true);
        }

        /// <summary>
        /// Goes back from the <see cref="nameLevelSubscreen"/> into the levelEditingScreen.
        /// </summary>
        public void LevelNameSubscreenBack() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            nameLevelSubscreen.SetActive(false);
        }

        /// <summary>
        /// If the <see cref="levelNameField"/> isn't empty and it doesn't contain a name of a level that already exists, it calls levelEditorTilemapAdapter.ExportLevel to export the level and then quits the levelEditingScreen going back to the levelSelectionScreen.
        /// </summary>
        public void LevelNameSubscreenExport() {
            if (levelNameField.text == "") {
                noLevelNameWarningText.gameObject.SetActive(true);
                levelExportDuplicateNameWarning.gameObject.SetActive(false);
                AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            }
            else {
                if (!LevelRegistry2.LevelExists(levelNameField.text)) {
                    noLevelNameWarningText.gameObject.SetActive(false);
                    levelExportDuplicateNameWarning.gameObject.SetActive(false);
                    nameLevelSubscreen.gameObject.SetActive(false);
                    levelEditorTilemapAdapter.ExportLevel(levelNameField.text, levelHeight, levelWidth);
                    levelSelectionScreen.AddNewLevel();
                    QuitEditing(true);
                }
                else {
                    levelExportDuplicateNameWarning.SetActive(true);
                    noLevelNameWarningText.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Opens the quitting subscreen.
        /// </summary>
        public void Quit() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            quittingSubscreen.SetActive(true);
        }

        /// <summary>
        /// Goes back from the quitting subscreen into the levelEditingScreen.
        /// </summary>
        public void QuittingSubscreenBack() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            quittingSubscreen.SetActive(false);
        }
        
        /// <summary>
        /// Confirms quitting editing and goes back to the levelSelectionScreen.
        /// </summary>
        /// <param name="playButtonClick"></param>
        public void QuitEditing(bool playButtonClick) {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.gameObject.SetActive(true);
            quittingSubscreen.SetActive(false);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Gets the level from levelEditorTilemapAdapter.GetLevel and then then loads the level on the <see cref="gameScreenManager"/>.
        /// </summary>
        public void PlayTestLevel() {
            var level = levelEditorTilemapAdapter.GetLevel(levelHeight, levelWidth);

            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            gameScreenManager.EnterEditMode();
            gameScreenManager.LoadLevel(level);
            gameScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// If a player is placed on the levelEditingScreen it allows the level to be exported and playtested.
        /// </summary>
        private void PlayerPlacedEvent() {
            exportLevelButton.interactable = true;
            playtestLevelButton.interactable = true;
            exportLevelButtonText.alpha = 1f;
            playtestLevelButtonText.alpha = 1f;
        }

        /// <summary>
        /// If a player is removed from the levelEditingScreen it makes is so the level can't be exported and playtested.
        /// </summary>
        private void PlayerRemovedEvent() {
            exportLevelButton.interactable = false;
            playtestLevelButton.interactable = false;
            exportLevelButtonText.alpha = 0.5f;
            playtestLevelButtonText.alpha = 0.5f;
        }
    }
}