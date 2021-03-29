using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ProgramSetup {
	//TODO: Most of this class is horribly written and could be fixed with an associative container. Please fix!
	public class AudioManager : MonoBehaviour {
		#region Serialized variables
		[SerializeField] private AudioSource audioEffectsSource;
		[SerializeField] private AudioSource musicSource;
		[SerializeField] private AudioSource transitionMusicSource;
		[SerializeField] private AudioSource fadingAudioEffectSource;
		[SerializeField] private AudioMixer musicMixer;
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
	
		#region Variables
		public static AudioManager Instance;
		private readonly System.Random random = new System.Random();

		private float easyLevelMusicTime;
		private float mediumLevelMusicTime;
		private float hardLevelMusicTime;
		private float lastLevelMusicTime;
		private float playerLevelMusicTime;
		private float menuMusicTime;
		#endregion

		#region Enums
		public enum MusicClip {
			EasyLevelMusic,
			MediumLevelMusic,
			HardLevelMusic,
			LastLevelMusic,
			PlayerLevelMusic,
			MenuMusic
		}
		public enum AudioEffectClip {
			None,
			BoxMoved,
			BoxMovedInPlace,
			ButtonClicked,
			LevelFinished
		}
		#endregion
		
		#region Methods
		private void Awake() {
			easyLevelMusicTime = 0;
			mediumLevelMusicTime = 0;
			hardLevelMusicTime = 0;
			lastLevelMusicTime = 0;
			playerLevelMusicTime = 0;
			menuMusicTime = 0;

			musicSource.volume = 0.75f;
			audioEffectsSource.volume = 1f;
			transitionMusicSource.volume = 0.75f;

			if (Instance == null)
				Instance = this;
			else if(Instance != this)
				Destroy(gameObject);
			
			DontDestroyOnLoad(gameObject);
		}

		public void StopAudioEffects() {
			audioEffectsSource.Stop();
			audioEffectsSource.clip = null;
		}
		
		public void StopMusic() {
			if (musicSource.clip == easyLevelMusic)
				easyLevelMusicTime = musicSource.time;
			else if (musicSource.clip == mediumLevelMusic)
				mediumLevelMusicTime = musicSource.time;
			else if (musicSource.clip == hardLevelMusic)
				hardLevelMusicTime = musicSource.time;
			else if (musicSource.clip == lastLevelMusic)
				lastLevelMusicTime = musicSource.time;
			else if (musicSource.clip == playerLevelMusic)
				playerLevelMusicTime = musicSource.time;
			else if (musicSource.clip == menuMusic)
				menuMusicTime = musicSource.time;
			
			StartCoroutine(TransitionMusicClips(musicSource.clip, null));
		}

		private IEnumerator FadeAudioClip(AudioClip clip) {
			if (audioEffectsSource.isPlaying == false)
				yield break;
			
			fadingAudioEffectSource.clip = clip;
			fadingAudioEffectSource.time = audioEffectsSource.time;
			fadingAudioEffectSource.volume = 0.75f;
			fadingAudioEffectSource.Play();
			var timer = 0.75f;

			while (timer > 0) {
				timer -= Time.deltaTime;
				fadingAudioEffectSource.volume = timer;
				yield return null;
			}

			fadingAudioEffectSource.volume = 0f;
			fadingAudioEffectSource.Stop();
		}
		
		private IEnumerator TransitionMusicClips(AudioClip clip1, AudioClip clip2) {
			// ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
			if (clip1 == null) {
				musicSource.clip = clip2;
				musicSource.volume = 0.75f;
			}
			else {
				transitionMusicSource.clip = clip1;
				transitionMusicSource.time = musicSource.time;
				musicSource.clip = clip2;
				transitionMusicSource.volume = 0.75f;
				musicSource.volume = 0f;
				transitionMusicSource.Play();
				yield return null;
				var timer = 0.75f;

				while (timer > 0f) {
					timer -= Time.deltaTime;
					transitionMusicSource.volume = timer;
					musicSource.volume = Math.Abs(timer - 0.75f);
					yield return null;
				}

				musicSource.volume = 0.75f;
				transitionMusicSource.volume = 0f;
				transitionMusicSource.clip = null;
			}
		}
		
		public void PlayMusic(MusicClip clip) {
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (clip) {
				case MusicClip.EasyLevelMusic when musicSource.clip == easyLevelMusic:
				case MusicClip.MediumLevelMusic when musicSource.clip == mediumLevelMusic:
				case MusicClip.HardLevelMusic when musicSource.clip == hardLevelMusic:
				case MusicClip.LastLevelMusic when musicSource.clip == lastLevelMusic:
				case MusicClip.PlayerLevelMusic when musicSource.clip == playerLevelMusic:
				case MusicClip.MenuMusic when musicSource.clip == menuMusic:
					return;
			}

			if (musicSource.clip == easyLevelMusic)
				easyLevelMusicTime = musicSource.time;
			else if (musicSource.clip == mediumLevelMusic)
				mediumLevelMusicTime = musicSource.time;
			else if (musicSource.clip == hardLevelMusic)
				hardLevelMusicTime = musicSource.time;
			else if (musicSource.clip == lastLevelMusic)
				lastLevelMusicTime = musicSource.time;
			else if (musicSource.clip == playerLevelMusic)
				playerLevelMusicTime = musicSource.time;
			else if (musicSource.clip == menuMusic)
				menuMusicTime = musicSource.time;
			
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (clip) {
				case MusicClip.EasyLevelMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, easyLevelMusic));
					musicSource.time = easyLevelMusicTime;
					break;
				case MusicClip.MediumLevelMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, mediumLevelMusic));
					musicSource.time = mediumLevelMusicTime;
					break;
				case MusicClip.HardLevelMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, hardLevelMusic));
					musicSource.time = hardLevelMusicTime;
					break;
				case MusicClip.LastLevelMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, lastLevelMusic));
					musicSource.time = lastLevelMusicTime;
					break;
				case MusicClip.PlayerLevelMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, playerLevelMusic));
					musicSource.time = playerLevelMusicTime;
					break;
				case MusicClip.MenuMusic:
					StartCoroutine(TransitionMusicClips(musicSource.clip, menuMusic));
					musicSource.time = menuMusicTime;
					break;
			}

			musicSource.Play();
		}

		private void PlayRandomClip(IReadOnlyList<AudioClip> clipList, AudioSource source) {
			var rand = random.Next(clipList.Count);

			source.clip = clipList[rand];
			source.Play();
		}
		
		public void PlayAudioEffect(AudioEffectClip clip) {
			if (audioEffectsSource.clip == levelFinishedEffect)
				StartCoroutine(FadeAudioClip(levelFinishedEffect));
			
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (clip) {
				case AudioEffectClip.BoxMoved:
					PlayRandomClip(new List<AudioClip> {boxMovedEffect1, boxMovedEffect2, boxMovedEffect3}, audioEffectsSource);
					break;
				case AudioEffectClip.BoxMovedInPlace:
					PlayRandomClip(new List<AudioClip> {boxMovedInPlaceEffect1, boxMovedInPlaceEffect2, boxMovedInPlaceEffect3}, audioEffectsSource);
					break;
				case AudioEffectClip.ButtonClicked:
					PlayRandomClip(new List<AudioClip> {buttonClickedEffect1, buttonClickedEffect2, buttonClickedEffect3}, audioEffectsSource);
					break;
				case AudioEffectClip.LevelFinished:
					audioEffectsSource.clip = levelFinishedEffect;
					audioEffectsSource.Play();
					break;
				default:
					audioEffectsSource.Stop();
					break;
			}
		}

		public void SetAudioEffectsVolume(float value) {
			SetAudioMixerVolume(value, audioEffectsMixer);
		}
		
		public float GetAudioEffectsVolume() {
			return GetAudioMixerVolume(audioEffectsMixer);
		}

		public void SetMusicVolume(float value) {
			SetAudioMixerVolume(value, musicMixer);
		}

		public float GetMusicVolume() {
			return GetAudioMixerVolume(musicMixer);
		}

		private static void SetAudioMixerVolume(float value, AudioMixer mixer) {
			mixer.SetFloat("GameVolume", Mathf.Log10(value) * 20);
		}

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