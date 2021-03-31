using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace Internationalization {
	/// <summary>
	/// Translates all the <see cref="TMP_Text"/> objects on the <c>MainScene</c>.
	/// </summary>
    public class MainSceneTranslator : MonoBehaviour {
        #region Text fields
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text playButtonText;
        [SerializeField] private TMP_Text optionsButtonText;
        [SerializeField] private TMP_Text infoButtonText;
        [SerializeField] private TMP_Text quitGameText;

        [SerializeField] private TMP_Text stageSelectionText;
        [SerializeField] private TMP_Text playStageOneText;
        [SerializeField] private TMP_Text playStageTwoText;
        [SerializeField] private TMP_Text playStageThreeText;
        [SerializeField] private TMP_Text stageSelectionBackText;

        [SerializeField] private TMP_Text optionsText;
        [SerializeField] private TMP_Text musicVolumeSliderText;
        [SerializeField] private TMP_Text audioEffectsVolumeSliderText;
        [SerializeField] private TMP_Text resolutionDropdownText;
        [SerializeField] private TMP_Text languageDropdownText;
        [SerializeField] private TMP_Text fullscreenToggleText;
        [SerializeField] private TMP_Text optionsBackText;

        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text gameInformationText;
        [SerializeField] private TMP_Text infoBackText;
        #endregion

        /// <summary>
        /// Updates the text of all <see cref="TMP_Text"/> object to their translations from the <see cref="Translator"/>.
        /// </summary>
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void UpdateTranslations() {
            titleText.text = Translator.GetTranslation("mainscene.mainscreen.title.text");
            playButtonText.text = Translator.GetTranslation("mainscene.mainscreen.play.button");
            optionsButtonText.text = Translator.GetTranslation("mainscene.mainscreen.options.button");
            infoButtonText.text = Translator.GetTranslation("mainscene.mainscreen.info.button");
            quitGameText.text = Translator.GetTranslation("mainscene.mainscreen.quit.button");
            
            stageSelectionText.text = Translator.GetTranslation("mainscene.stageselection.selectstage.text");
            playStageOneText.text = Translator.GetTranslation("mainscene.stageselection.stage1.button");
            playStageTwoText.text = Translator.GetTranslation("mainscene.stageselection.stage2.button");
            playStageThreeText.text = Translator.GetTranslation("mainscene.stageselection.stage3.button");
            stageSelectionBackText.text = Translator.GetTranslation("mainscene.stageselection.back.button");
            
            optionsText.text = Translator.GetTranslation("mainscene.options.options.text");
            musicVolumeSliderText.text = Translator.GetTranslation("mainscene.options.musicvolume.slider");
            audioEffectsVolumeSliderText.text = Translator.GetTranslation("mainscene.options.audioeffectsvolume.slider");
            resolutionDropdownText.text = Translator.GetTranslation("mainscene.options.resolution.dropdown");
            languageDropdownText.text = Translator.GetTranslation("mainscene.options.language.dropdown");
            fullscreenToggleText.text = Translator.GetTranslation("mainscene.options.fullscreen.toggle");
            optionsBackText.text = Translator.GetTranslation("mainscene.options.back.button");
            
            infoText.text = Translator.GetTranslation("mainscene.info.info.text");
            gameInformationText.text = Translator.GetTranslation("mainscene.info.gameinformation.text");
            infoBackText.text = Translator.GetTranslation("mainscene.info.back.button");
        }
    }
}