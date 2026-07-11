using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class CustomCheckBoxBehaviour : MonoBehaviour
{
	public bool state;

	public GameObject graphic;

	public CheckBoxType type;

	public CheckBoxActivationType activationType;

	public Sprite checkedSprite;

	public Sprite notCheckedSprite;

	private void Awake()
	{
	}

	public void OnSwitch()
	{
		MasterAudio.PlaySound("button");
		switch (type)
		{
		case CheckBoxType.Language:
			LanguageUIController.instance.OnSwitchLanguage(this, !state);
			SettingsController.instance.OnChangeLanguage();
			break;
		case CheckBoxType.Music:
			SettingsController.instance.OnMusicChange();
			if (VariablesGlobales.musicEnabled)
			{
				OnSelect();
			}
			else
			{
				OnDeselect();
			}
			break;
		case CheckBoxType.CustomName:
			MenuManager.instance.OnNamePropertyChanged();
			if (VariablesGlobales.ownName)
			{
				OnSelect();
			}
			else
			{
				OnDeselect();
			}
			break;
		case CheckBoxType.GameDifficulty:
			DifficultyMenuManager.instance.OnSwitchDifficulty(this, !state);
			MenuManager.instance.OnChangeDifficulty();
			break;
		case CheckBoxType.Quality:
			QualityMenuManager.instance.OnSwitchQuality(this, !state);
			SettingsController.instance.OnChangeQuality();
			break;
		case CheckBoxType.Tutorial:
			SettingsController.instance.OnSwitchTutorial(!state);
			if (VariablesGlobales.tutorialEnabled)
			{
				OnSelect();
			}
			else
			{
				OnDeselect();
			}
			break;
		case CheckBoxType.Advertisements:
			if (state && !VariablesGlobales.removeAdsBought)
			{
				IAPHelperWindow.instance.MakePurchase("com.evilnun.remove_ads");
			}
			break;
		case CheckBoxType.Blood:
			SettingsController.instance.OnSwitchBlood(!state);
			if (VariablesGlobales.bloodEnabled)
			{
				OnSelect();
			}
			else
			{
				OnDeselect();
			}
			break;
		}
	}

	public void OnSelect()
	{
		state = true;
		if (graphic == null) return;
		if (activationType == CheckBoxActivationType.Check)
		{
			Image image = graphic.GetComponent<Image>();
			if (image != null) image.sprite = checkedSprite;
		}
		else
		{
			graphic.transform.localScale = new Vector3(-1f, 1f, 1f);
			Image image = graphic.GetComponent<Image>();
			if (image == null) return;
			Color color = image.color;
			color.a = 1f;
			image.color = color;
		}
		if (type == CheckBoxType.Advertisements)
		{
			GetComponent<CustomCheckBoxUnlockConfiguration>().OnEnableAds(true);
		}
	}

	public void OnDeselect()
	{
		state = false;
		if (graphic == null) return;
		if (activationType == CheckBoxActivationType.Check)
		{
			Image image = graphic.GetComponent<Image>();
			if (image != null) image.sprite = notCheckedSprite;
		}
		else
		{
			graphic.transform.localScale = new Vector3(1f, 1f, 1f);
			Image image = graphic.GetComponent<Image>();
			if (image == null) return;
			Color color = image.color;
			color.a = 0.3f;
			image.color = color;
		}
		if (type == CheckBoxType.Advertisements)
		{
			GetComponent<CustomCheckBoxUnlockConfiguration>().OnEnableAds(false);
		}
	}
}
