using DarkTonic.MasterAudio;
using I2.Loc;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
	public static SettingsController instance;

	public const string androidRateUrl = "https://play.google.com/store/apps/details?id=com.keplerians.evilnun";

	public const string iOSRateUrl = "https://itunes.apple.com/us/app/the-nun/id1410194646?mt=8";

	private void Awake()
	{
		instance = this;
		Screen.sleepTimeout = -1;
		VariablesGlobales.currentLanguage = string.Empty;
		VariablesGlobales.musicEnabled = true;
		// Sound, like music, should default on for first run (before the "sound"
		// PlayerPrefs key exists); otherwise sound effects are silent until toggled.
		VariablesGlobales.soundEnabled = true;
		LoadSettings();
	}

	public void LoadSettings()
	{
		if (PlayerPrefs.HasKey("lang"))
		{
			VariablesGlobales.currentLanguage = PlayerPrefs.GetString("lang");
		}
		if (PlayerPrefs.HasKey("music"))
		{
			VariablesGlobales.musicEnabled = PlayerPrefs.GetInt("music") == 1;
		}
		if (PlayerPrefs.HasKey("music_volume"))
		{
			VariablesGlobales.musicVolume = PlayerPrefs.GetFloat("music_volume");
		}
		if (PlayerPrefs.HasKey("sound"))
		{
			VariablesGlobales.soundEnabled = PlayerPrefs.GetInt("sound") == 1;
		}
		if (PlayerPrefs.HasKey("sound_volume"))
		{
			VariablesGlobales.soundVolume = PlayerPrefs.GetFloat("sound_volume");
		}
		if (PlayerPrefs.HasKey("diff"))
		{
			VariablesGlobales.difficultyMode = PlayerPrefs.GetInt("diff");
		}
		if (PlayerPrefs.HasKey("quality"))
		{
			VariablesGlobales.qualityMode = PlayerPrefs.GetInt("quality");
		}
		if (PlayerPrefs.HasKey("tutorial"))
		{
			VariablesGlobales.tutorialEnabled = PlayerPrefs.GetInt("tutorial", 1) == 1;
		}
		if (!PlayerPrefs.HasKey("sound_volume"))
		{
			PlayerPrefs.SetFloat("sound_volume", 0.5f);
		}
		if (!PlayerPrefs.HasKey("music_volume"))
		{
			PlayerPrefs.SetFloat("music_volume", 0.5f);
		}
		VariablesGlobales.plays = PlayerPrefs.GetInt("plays", 0);
	}

	public void OnChangeLanguage()
	{
		MasterAudio.PlaySound("button");
		if (LanguageUIController.instance == null || LanguageUIController.instance.selectedLanguage == null) return;
		VariablesGlobales.currentLanguage = LanguageUIController.instance.selectedLanguage.language;
		LocalizationManager.CurrentLanguage = VariablesGlobales.currentLanguage;
		PlayerPrefs.SetString("lang", VariablesGlobales.currentLanguage);
		if (GameManager.instance != null) GameManager.instance.OnLoadScene(1);
	}

	public void OnMusicChange()
	{
		MasterAudio.PlaySound("button");
		VariablesGlobales.musicEnabled = !VariablesGlobales.musicEnabled;
		PlayerPrefs.SetInt("music", VariablesGlobales.musicEnabled ? 1 : 0);
	}

	public void OnSwitchTutorial(bool new_state)
	{
		VariablesGlobales.tutorialEnabled = new_state;
		PlayerPrefs.SetInt("tutorial", VariablesGlobales.tutorialEnabled ? 1 : 0);
	}

	public void OnSwitchBlood(bool new_state)
	{
		VariablesGlobales.bloodEnabled = new_state;
		PlayerPrefs.SetInt("blood", VariablesGlobales.bloodEnabled ? 1 : 0);
	}

	public void OnChangeQuality()
	{
		if (QualityMenuManager.instance == null || QualityMenuManager.instance.selectedQuality == null) return;
		VariablesGlobales.qualityMode = QualityMenuManager.instance.selectedQuality.id;
		PlayerPrefs.SetInt("quality", VariablesGlobales.qualityMode);
		QualitySettings.SetQualityLevel(VariablesGlobales.qualityMode);
	}
}
