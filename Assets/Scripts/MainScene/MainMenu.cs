using JetBrains.Annotations;
using UnityEngine;

namespace MainScene
{
    /// <summary>
    /// Adds functionality to the quit button on the main menu.
    /// </summary>
    public class MainMenu : MonoBehaviour {
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