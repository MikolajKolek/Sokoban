using JetBrains.Annotations;
using UnityEngine;

namespace MainScene
{
    /// <summary>
    /// Adds functionality to the play and quit buttons on the main menu.
    /// </summary>
    public class MainMenu : MonoBehaviour {
        #region Variables
        #endregion
    
        #region Methods
        /// <summary>
        /// Quits the game.
        /// </summary>
        [UsedImplicitly]
        public void Quit() {
            Application.Quit();
        }
        #endregion
    }
}