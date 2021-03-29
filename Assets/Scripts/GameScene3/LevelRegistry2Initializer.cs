using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene3 {
    public class LevelRegistry2Initializer : MonoBehaviour {
        [SerializeField] protected TileBase boxOnBoxArea;
        [SerializeField] protected TileBase boxArea;
        [SerializeField] protected TileBase player;
        [SerializeField] protected TileBase floor;
        [SerializeField] protected TileBase wall;
        [SerializeField] protected TileBase box;
        [SerializeField] protected TileBase empty;
        
        public void Start() {
            LevelRegistry2.InitializeLevelList(boxOnBoxArea, boxArea, player, floor, wall, box, empty);
        }
    }
}