using ProgramSetup;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainScene {
	public class StageSelectionScreen : MonoBehaviour {
		#region Variables
		#endregion
	
		#region Methods
		/// <summary>
		/// Loads the <c>Game</c> scene for the first stage.
		/// </summary>
		public void PlayStage1() {
			AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
			SceneManager.LoadScene("GameScene1");
		}
	
		/// <summary>
		/// Loads the <c>Game</c> scene for the second stage.
		/// </summary>
		public void PlayStage2() {
			AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
			SceneManager.LoadScene("GameScene2");
		}
	
		/// <summary>
		/// Loads the <c>Game</c> scene for the third stage.
		/// </summary>
		public void PlayStage3() {
			AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
			SceneManager.LoadScene("GameScene3");
		}
		#endregion
	}
}