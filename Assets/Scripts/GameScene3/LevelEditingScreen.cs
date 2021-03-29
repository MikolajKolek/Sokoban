using System;
using GameScene1;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene3 {
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
        
        public void IncreaseLevelWidth() {
            if (levelWidth + 1 > 30)
                return;
            
            levelWidth++;
            levelWidthText.text = levelWidth.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelHeight; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(levelWidth - 1, i, 0), Level.Tile.Empty);
        }
        
        public void DecreaseLevelWidth() {
            if (levelWidth - 1 < 3)
                return;

            levelWidth--;
            levelWidthText.text = levelWidth.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelHeight; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(levelWidth, i, 0), Level.Tile.None);
        }

        public void IncreaseLevelHeight() {
            if (levelHeight + 1 > 20)
                return;

            levelHeight++;
            levelHeightText.text = levelHeight.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelWidth; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(i, levelHeight - 1, 0), Level.Tile.Empty);
        }

        public void DecreaseLevelHeight() {
            if (levelHeight - 1 < 3)
                return;

            levelHeight--;
            levelHeightText.text = levelHeight.ToString();
            levelEditorTilemapAdapter.UpdateLevelSize(levelHeight, levelWidth);
            
            for (var i = 0; i < levelWidth; i++)
                levelEditorTilemapAdapter.SetTile(new Vector3Int(i, levelHeight, 0), Level.Tile.None);
        }

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
        
        public void SelectFloorTile() {
            ResetSelectedTile();
            floorButtonImage.overrideSprite = selectedFloorButtonSprite;
            selectedTileType = Level.Tile.Floor;
        }
        
        public void SelectWallTile() {
            ResetSelectedTile();
            wallButtonImage.overrideSprite = selectedWallButtonSprite;
            selectedTileType = Level.Tile.Wall;
        }
        
        public void SelectBoxTile() {
            ResetSelectedTile();
            boxButtonImage.overrideSprite = selectedBoxButtonSprite;
            selectedTileType = Level.Tile.Box;
        }
        
        public void SelectBoxOnBoxAreaTile() {
            ResetSelectedTile();
            boxOnBoxAreaButtonImage.overrideSprite = selectedBoxOnBoxAreaButtonSprite;
            selectedTileType = Level.Tile.BoxOnBoxArea;
        }
        
        public void SelectBoxAreaTile() {
            ResetSelectedTile();
            boxAreaButtonImage.overrideSprite = selectedBoxAreaButtonSprite;
            selectedTileType = Level.Tile.BoxArea;
        }
        
        public void SelectPlayerTile() {
            ResetSelectedTile();
            playerButtonImage.overrideSprite = selectedPlayerButtonSprite;
            selectedTileType = Level.Tile.Player;
        }

        public void ExportLevel() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            noLevelNameWarningText.gameObject.SetActive(false);
            levelExportDuplicateNameWarning.gameObject.SetActive(false);
            nameLevelSubscreen.gameObject.SetActive(true);
        }

        public void LevelNameSubscreenBack() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            nameLevelSubscreen.SetActive(false);
        }
        
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

        public void Quit() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            quittingSubscreen.SetActive(true);
        }

        public void QuittingSubscreenBack() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            quittingSubscreen.SetActive(false);
        }
        
        public void QuitEditing(bool playButtonClick) {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.gameObject.SetActive(true);
            quittingSubscreen.SetActive(false);
            gameObject.SetActive(false);
        }
        
        public void PlayTestLevel() {
            var level = levelEditorTilemapAdapter.GetLevel(levelHeight, levelWidth);

            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            gameScreenManager.EnterEditMode();
            gameScreenManager.LoadLevel(level);
            gameScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        private void PlayerPlacedEvent() {
            exportLevelButton.interactable = true;
            playtestLevelButton.interactable = true;
            exportLevelButtonText.alpha = 1f;
            playtestLevelButtonText.alpha = 1f;
        }

        private void PlayerRemovedEvent() {
            exportLevelButton.interactable = false;
            playtestLevelButton.interactable = false;
            exportLevelButtonText.alpha = 0.5f;
            playtestLevelButtonText.alpha = 0.5f;
        }
    }
}