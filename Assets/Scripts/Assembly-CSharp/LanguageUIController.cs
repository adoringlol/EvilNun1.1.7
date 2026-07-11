using System.Collections.Generic;
using DarkTonic.MasterAudio;
using I2.Loc;
using UnityEngine;

public class LanguageUIController : MonoBehaviour
{
	public static LanguageUIController instance;

	public List<LanguageItem> languages;

	public LanguageItem selectedLanguage;

	public GameObject window;

	private void Awake()
	{
		instance = this;
		if (languages == null) languages = new List<LanguageItem>();
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
		{
			OnInitialize();
		}
		else
		{
			Invoke("OnInitialize", 1f);
		}
	}

	public void OnInitialize()
	{
		if (!string.IsNullOrEmpty(VariablesGlobales.currentLanguage))
		{
			selectedLanguage = GetLanguage(VariablesGlobales.currentLanguage);
			if (selectedLanguage != null && selectedLanguage.item != null) OnSwitchLanguage(selectedLanguage.item, true);
			return;
		}
		VariablesGlobales.currentLanguage = LocalizationManager.CurrentLanguage;
		Debug.Log("Device language = " + VariablesGlobales.currentLanguage);
		selectedLanguage = GetLanguage(VariablesGlobales.currentLanguage);
		if (selectedLanguage != null && selectedLanguage.item != null) OnSwitchLanguage(selectedLanguage.item, true);
	}

	public void ShowWindow()
	{
		MasterAudio.PlaySound("button");
		if (window != null) window.SetActive(true);
	}

	public void HideWindow()
	{
		MasterAudio.PlaySound("button");
		if (window != null) window.SetActive(false);
	}

	public LanguageItem GetLanguage(string language)
	{
		return languages.Find((LanguageItem l) => l != null && l.language == language);
	}

	public LanguageItem GetLanguage(CustomCheckBoxBehaviour language)
	{
		return languages.Find((LanguageItem l) => l != null && l.item == language);
	}

	public void OnSwitchLanguage(CustomCheckBoxBehaviour target, bool state)
	{
		if (languages == null) return;
		foreach (LanguageItem language in languages)
		{
			if (language != null && language.item != null && (language.item != target || !state))
			{
				language.item.OnDeselect();
			}
		}
		selectedLanguage = null;
		if (state)
		{
			selectedLanguage = GetLanguage(target);
		}
		if (selectedLanguage == null)
		{
			selectedLanguage = GetLanguage(VariablesGlobales.currentLanguage);
		}
		if (selectedLanguage != null && selectedLanguage.item != null) selectedLanguage.item.OnSelect();
	}
}
