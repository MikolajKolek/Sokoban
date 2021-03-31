using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ProgramSetup {
	/// <summary>
	/// The <see cref="AudioManager"/> is a singleton that is responsible for playing audio effects and music in the game.
	/// </summary>
	public class AudioManager : MonoBehaviour {
		#region Serialized variables
		/// <summary>
		/// The <see cref="AudioSource"/> that plays audio effects.
		/// </summary>
		[SerializeField] private AudioSource audioEffectsSource;
		/// <summary>
		/// The <see cref="AudioSource"/> that plays music.
		/// </summary>
		[SerializeField] private AudioSource musicSource;
		/// <summary>
		/// The <see cref="AudioSource"/> that is used when transitioning between two music clips.
		/// </summary>
		[SerializeField] private AudioSource transitionMusicSource;
		/// <summary>
		/// The <see cref="AudioSource"/> that is used when transitioning between audio effects clips.
		/// </summary>
		[SerializeField] private AudioSource transitionAudioEffectSource;
		/// <summary>
		/// The <see cref="AudioMixer"></see> that is used for playing music.
		/// </summary>
		[SerializeField] private AudioMixer musicMixer;
		/// <summary>
		/// The <see cref="AudioMixer"></see> that is used for playing audio effects.
		/// </summary>
		[SerializeField] private AudioMixer audioEffectsMixer;
		
		[SerializeField] private AudioClip easyLevelMusic;
		[SerializeField] private AudioClip mediumLevelMusic;
		[SerializeField] private AudioClip hardLevelMusic;
		[SerializeField] private AudioClip lastLevelMusic;
		[SerializeField] private AudioClip playerLevelMusic;
		[SerializeField] private AudioClip menuMusic;

		[SerializeField] private AudioClip levelFinishedEffect;
		[SerializeField] private AudioClip boxMovedEffect1;
		[SerializeField] private AudioClip boxMovedEffect2;
		[SerializeField] private AudioClip boxMovedEffect3;
		[SerializeField] private AudioClip boxMovedInPlaceEffect1;
		[SerializeField] private AudioClip boxMovedInPlaceEffect2;
		[SerializeField] private AudioClip boxMovedInPlaceEffect3;
		[SerializeField] private AudioClip buttonClickedEffect1;
		[SerializeField] private AudioClip buttonClickedEffect2;
		[SerializeField] private AudioClip buttonClickedEffect3;
		#endregion

		#region Enums
		/// <summary>
		/// An enum that stores all music clips in the game.
		/// </summary>
		public enum MusicClip {
			None = 0,
			EasyLevelMusic = 1,
			MediumLevelMusic = 2,
			HardLevelMusic = 3,
			LastLevelMusic = 4,
			PlayerLevelMusic = 5,
			MenuMusic = 6
		}

		/// <summary>
		/// An enum that stores all audio effect clips in the game.
		/// </summary>
		public enum AudioEffectClip {
			None = 0,
			BoxMoved = 1,
			BoxMovedInPlace = 2,
			ButtonClicked = 3,
			LevelFinished = 4
		}
		#endregion

		#region Variables
		/// <summary>
		/// Used to store the single instance of the <see cref="AudioManager"/> singleton.
		/// </summary>
		public static AudioManager Instance;
		/// <summary>
		/// Used to generate random values for playing random audio effects.
		/// </summary>
		private readonly System.Random random = new System.Random();
		/// <summary>
		/// Associates <see cref="MusicClip"/>s with their respective <see cref="AudioClip"/>s 
		/// </summary>
		private readonly Dictionary<MusicClip, AudioClip> musicClipDictionary = new Dictionary<MusicClip, AudioClip>();
		/// <summary>
		/// Associates <see cref="AudioClip"/>s with their respective <see cref="MusicClip"/>s
		/// </summary>
		private readonly Dictionary<AudioClip, MusicClip> reverseMusicClipDictionary = new Dictionary<AudioClip, MusicClip>();
		/// <summary>
		/// Associates <see cref="AudioEffectClip"/>s with the <see cref="List{T}"/> of their respective <see cref="AudioClip"/>s
		/// </summary>
		private readonly Dictionary<AudioEffectClip, List<AudioClip>> audioEffectClipDictionary = new Dictionary<AudioEffectClip, List<AudioClip>>();
		/// <summary>
		/// Stores the time at which a <see cref="MusicClip"/> was last stopped so it can be resumed at that same time.
		/// </summary>
		private readonly List<float> musicTime = new List<float>();
		#endregion

		#region Methods
		/// <summary>
		/// Called when the <see cref="AudioManager"/> is created, it makes sure that only one instance can exist at one time. Also sets up the dictionaries, <see cref="musicTime"/> and the <see cref="AudioSource"/>s.
		/// </summary>
		private void Awake() {
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);

			DontDestroyOnLoad(gameObject);

			for (var i = 0; i < 7; i++)
				musicTime.Add(0);

			musicClipDictionary.Add(MusicClip.EasyLevelMusic, easyLevelMusic);
			musicClipDictionary.Add(MusicClip.MediumLevelMusic, mediumLevelMusic);
			musicClipDictionary.Add(MusicClip.HardLevelMusic, hardLevelMusic);
			musicClipDictionary.Add(MusicClip.LastLevelMusic, lastLevelMusic);
			musicClipDictionary.Add(MusicClip.PlayerLevelMusic, playerLevelMusic);
			musicClipDictionary.Add(MusicClip.MenuMusic, menuMusic);

			reverseMusicClipDictionary.Add(easyLevelMusic, MusicClip.EasyLevelMusic);
			reverseMusicClipDictionary.Add(mediumLevelMusic, MusicClip.MediumLevelMusic);
			reverseMusicClipDictionary.Add(hardLevelMusic, MusicClip.HardLevelMusic);
			reverseMusicClipDictionary.Add(lastLevelMusic, MusicClip.LastLevelMusic);
			reverseMusicClipDictionary.Add(playerLevelMusic, MusicClip.PlayerLevelMusic);
			reverseMusicClipDictionary.Add(menuMusic, MusicClip.MenuMusic);

			audioEffectClipDictionary.Add(AudioEffectClip.BoxMoved, new List<AudioClip>(){boxMovedEffect1, boxMovedEffect2, boxMovedEffect3});
			audioEffectClipDictionary.Add(AudioEffectClip.BoxMovedInPlace, new List<AudioClip>() { boxMovedInPlaceEffect1, boxMovedInPlaceEffect2, boxMovedInPlaceEffect3 });
			audioEffectClipDictionary.Add(AudioEffectClip.ButtonClicked, new List<AudioClip>() { buttonClickedEffect1, buttonClickedEffect2, buttonClickedEffect3 });
			audioEffectClipDictionary.Add(AudioEffectClip.LevelFinished, new List<AudioClip>() { levelFinishedEffect });

			musicSource.volume = 0.75f;
			transitionMusicSource.volume = 0.75f;
			audioEffectsSource.volume = 1f;
		}

		/// <summary>
		/// Stops any currently playing audio effects.
		/// </summary>
		public void StopAudioEffects() {
			audioEffectsSource.Stop();
		}

		/// <summary>
		/// Stops any currently playing music.
		/// </summary>
		public void StopMusic() {
			musicTime[(int) reverseMusicClipDictionary[musicSource.clip]] = musicSource.time;

			StartCoroutine(TransitionAudioClips(musicSource, transitionMusicSource, musicSource.clip, null, 1f));
		}

		/// <summary>
		/// A coroutine that smoothly transitions between two <see cref="AudioClip"/>s.
		/// </summary>
		/// <param name="mainSource">The main audio source that will continue playing <see cref="clip2"/> after the coroutine is finished.</param>
		/// <param name="transitionSource">The transition source that is used to transition away from <see cref="clip1"/></param>
		/// <param name="clip1">The clip that you want to transition away from</param>
		/// <param name="clip2">The clip you want to transition to</param>
		/// <param name="transitionTime">The time (in seconds) you want the transition to take</param>
		private IEnumerator TransitionAudioClips(AudioSource mainSource, AudioSource transitionSource, AudioClip clip1, AudioClip clip2, float transitionTime) {
			if (clip1 == null)
			{
				mainSource.clip = clip2;
				mainSource.Play();
				yield break;
			}
				
			var maxVolume = 1f;
			if (mainSource == musicSource)
				maxVolume = 0.75f;
			
			transitionSource.clip = clip1;
			transitionSource.time = mainSource.time;
			transitionSource.volume = maxVolume;
			transitionSource.Play();

			mainSource.clip = clip2;
			mainSource.volume = 0f;
	
			if (reverseMusicClipDictionary.TryGetValue(clip1, out var mainSourceMusicClip)) { 
				mainSource.time = musicTime[(int) mainSourceMusicClip];
			}
			else
			{
				mainSource.time = 0f;
			}

			var timer = transitionTime;
			while (timer > 0f) {
				timer -= Time.deltaTime;

				transitionSource.volume = maxVolume * (timer / transitionTime);
				mainSource.volume = Math.Abs(maxVolume * (timer / transitionTime) - 0.75f);
				
				yield return null;
			}

			mainSource.volume = maxVolume;
			transitionSource.volume = 0f;
		}

		/// <summary>
		/// Plays the given <see cref="MusicClip"/> while smoothly transitioning away from the previous clip if one was already playing.
		/// </summary>
		/// <param name="clip">The music clip that you want to start playing</param>
		public void PlayMusic(MusicClip clip) {
			if (musicSource.clip != null) {
				if (reverseMusicClipDictionary[musicSource.clip] == clip)
					return;

				musicTime[(int)reverseMusicClipDictionary[musicSource.clip]] = musicSource.time;
			}

			StartCoroutine(TransitionAudioClips(musicSource, transitionMusicSource, musicSource.clip, musicClipDictionary[clip], 1f));
			musicSource.time = musicTime[(int) clip];	

			musicSource.Play();
		}

		/// <summary>
		/// Plays the given <see cref="AudioEffectClip"/> while smoothly transitioning away from the previous clip if one was already playing and it was over a second long.
		/// </summary>
		/// <param name="clip">The clip that you want to start playing</param>
		public void PlayAudioEffect(AudioEffectClip clip)
		{
			var rand = random.Next(audioEffectClipDictionary[clip].Count);

			var audioClip = audioEffectClipDictionary[clip][rand];

			if (audioEffectsSource.isPlaying && audioEffectsSource.clip.length > 1f)
				StartCoroutine(TransitionAudioClips(audioEffectsSource, transitionAudioEffectSource, audioEffectsSource.clip, audioClip, 1f));
			else
			{
				audioEffectsSource.clip = audioClip;
				audioEffectsSource.Play();
			}
		}

		/// <summary>
		/// Sets the audio effects volume on the <see cref="audioEffectsMixer"/>.
		/// </summary>
		/// <param name="value">The value you want to change the volume to.</param>
		public void SetAudioEffectsVolume(float value) {
			SetAudioMixerVolume(value, audioEffectsMixer);
		}

		/// <summary>
		/// Gets the audio effects volume from the <see cref="audioEffectsMixer"/>.
		/// </summary>
		/// <returns>The audio effects volume.</returns>
		public float GetAudioEffectsVolume() {
			return GetAudioMixerVolume(audioEffectsMixer);
		}

		/// <summary>
		/// Sets the music volume on the <see cref="musicMixer"/>.
		/// </summary>
		/// <param name="value">The value you want to change the volume to.</param>
		public void SetMusicVolume(float value) {
			SetAudioMixerVolume(value, musicMixer);
		}

		/// <summary>
		/// Gets the music volume from the <see cref="musicMixer"/>.
		/// </summary>
		/// <returns>The music volume.</returns>
		public float GetMusicVolume() {
			return GetAudioMixerVolume(musicMixer);
		}

		/// <summary>
		/// Sets the volume of the passed <see cref="AudioMixer"/>
		/// </summary>
		/// <param name="value">The value you want to change the volume to.</param>
		/// <param name="mixer">The <see cref="AudioMixer"/> you want to change the volume on.</param>
		private static void SetAudioMixerVolume(float value, AudioMixer mixer) {
			mixer.SetFloat("GameVolume", Mathf.Log10(value) * 20);
		}

		/// <summary>
		/// Gets the volume of the passed <see cref="AudioMixer"/>
		/// </summary>
		/// <param name="mixer">The <see cref="AudioMixer"/> you want to get the volume of.</param>
		/// <returns>The volume of the <see cref="AudioMixer"/></returns>
		private static float GetAudioMixerVolume(AudioMixer mixer) {
			var ok = mixer.GetFloat("GameVolume", out var volume);
			volume = Mathf.Pow(10, volume / 20);
			if (!ok)
				volume = 1f;

			return volume;
		}
		#endregion
	}
}