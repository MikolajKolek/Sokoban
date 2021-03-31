using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene3 {
    /// <summary>
    /// Initializes the <see cref="LevelRegistry2"/>
    /// </summary>
    public class LevelRegistry2Initializer : MonoBehaviour {
        [SerializeField] protected TileBase boxOnBoxArea;
        [SerializeField] protected TileBase boxArea;
        [SerializeField] protected TileBase player;
        [SerializeField] protected TileBase floor;
        [SerializeField] protected TileBase wall;
        [SerializeField] protected TileBase box;
        [SerializeField] protected TileBase empty;
        
        /// <summary>
        /// Initializes <see cref="LevelRegistry2"/> because it requires references to <see cref="TileBase"/>s to deserialize levels.
        /// </summary>
        public void Start() {
            LevelRegistry2.InitializeLevelList(boxOnBoxArea, boxArea, player, floor, wall, box, empty);
        }
    }
}