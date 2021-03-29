using System;
using System.Collections.Generic;
using System.Linq;
using GameScene1;
using Internationalization;
using ProgramSetup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene2 {
    public class LeaderboardManager : MonoBehaviour {
        [SerializeField] private TMP_Text exampleLeaderboardEntry;
        [SerializeField] private GameObject leaderboardScrollViewContent;
        [SerializeField] private Button seeIndividualScoresButton;
        [SerializeField] private TMP_Text seeIndividualScoresButtonText;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private GameObject individualScoresScreen;
        [SerializeField] private GameObject individualScoresScrollViewContent;
        [SerializeField] private TMP_Text exampleScoreEntry;

        public List<TMP_Text> individualScoresList;
        public List<TMP_Text> leaderboardEntryList;
        private bool leaderboardInitialized = false;
        
        public void InitializeLeaderboard() {
            foreach(var entry in leaderboardEntryList)
                Destroy(entry.gameObject);
            leaderboardEntryList = new List<TMP_Text>();
            exampleLeaderboardEntry.gameObject.SetActive(true);

            levelSelectionScreen.SetActive(false);
            gameObject.SetActive(true);
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            var leaderboard = ProfileManager.GetProfileLeaderboard();

            foreach (var entry in leaderboard) {
                var button = Instantiate(exampleLeaderboardEntry, leaderboardScrollViewContent.transform, true);
                button.text = entry.name + " - " + Translator.GetTranslation("gamescene.leaderboard.score.text") + " " + entry.score;
                button.name = entry.id.ToString();
                leaderboardEntryList.Add(button);
            }
            
            leaderboardInitialized = true;
            seeIndividualScoresButton.interactable = false;
            seeIndividualScoresButtonText.alpha = 0.5f;
            exampleLeaderboardEntry.gameObject.SetActive(false);
        }

        public void ToggleValueChanged() {
            if (leaderboardInitialized) {
                group.allowSwitchOff = false;
                seeIndividualScoresButton.interactable = true;
                seeIndividualScoresButtonText.alpha = 1f;
            }
        }
        
        public void Exit() {
            leaderboardInitialized = false;
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            levelSelectionScreen.SetActive(true);
            gameObject.SetActive(false);
        }

        public void SeeIndividualScores() {
            AudioManager.Instance.PlayAudioEffect(AudioManager.AudioEffectClip.ButtonClicked);
            
            foreach (var entry in individualScoresList)
                Destroy(entry.gameObject);
            individualScoresList = new List<TMP_Text>();
            exampleScoreEntry.gameObject.SetActive(true);
            
            individualScoresScreen.SetActive(true);

            var selectedProfile = ProfileManager.GetProfile(Convert.ToInt32(group.ActiveToggles().First().transform.parent.name));

            var i = 0;
            foreach (var score in selectedProfile.levelScore) {
                var button = Instantiate(exampleScoreEntry, individualScoresScrollViewContent.transform, true);
                button.text = Translator.GetTranslation("gamescene.levelselection.level.text") + " " +  LevelRegistry.GetLevel(i).levelName + " - " + score;
                individualScoresList.Add(button);
                
                i++;
            }
            
            exampleScoreEntry.gameObject.SetActive(false);
        }
    }
}