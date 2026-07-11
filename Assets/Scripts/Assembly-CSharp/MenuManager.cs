using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using I2.Loc;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;

	public GameObject playButton;

	public string names;

	public string lastnames;

	public TextMeshProUGUI nameTxt;

	public CustomCheckBoxBehaviour ownNameCheckBox;

	public CustomCheckBoxBehaviour tutorialCheckBox;

	public CustomCheckBoxBehaviour adsCheckbox;

	public CustomCheckBoxBehaviour bloodCheckbox;

	public TMP_InputField inputOwnName;

	public GameObject optionsButtons;

	public GameObject continueButton;

	public GameObject levelConfigurationCanvas;

	public List<TextMeshProUGUI> streetTexts;

	public List<StreetByLanguage> streetsByLanguages;

	public string[] youtuberNames;

	public Transform noteAnimation;

	private void Awake()
	{
		instance = this;
		if (Random.value > 0.5f)
		{
			noteAnimation.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			noteAnimation.transform.DORotate(new Vector3(0f, 0f, -2f), 0.6f).SetDelay(0.12f);
		}
		else
		{
			noteAnimation.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			noteAnimation.transform.DORotate(new Vector3(0f, 0f, 2f), 0.6f).SetDelay(0.12f);
		}
	}

	private void Start()
	{
		SelectRandomName();
		Initialize();
		Invoke("EnableStuff", 0.8f);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			BackToFirstMenu();
		}
	}

	public void ToggleAdsState()
	{
		if (VariablesGlobales.adsEnabled)
		{
			adsCheckbox.OnSelect();
		}
		else
		{
			adsCheckbox.OnDeselect();
		}
	}

	public void EnableStuff()
	{
		optionsButtons.SetActive(true);
		levelConfigurationCanvas.SetActive(true);
		playButton.SetActive(true);
	}

	public void BackToFirstMenu()
	{
		GameManager.instance.OnLoadScene(1);
	}

	public void SelectRandomName()
	{
		if (Random.value > 0.95f)
		{
			VariablesGlobales.randomYoutubeName = true;
			nameTxt.text = youtuberNames[Random.Range(0, youtuberNames.Length)];
			VariablesGlobales.youtuberName = nameTxt.text;
			return;
		}
		VariablesGlobales.randomYoutubeName = false;
		string[] array = names.Split(',');
		string[] array2 = lastnames.Split(',');
		nameTxt.text = array[Random.Range(0, array.Length)] + " " + array2[Random.Range(0, array2.Length)];
		VariablesGlobales.randomName = nameTxt.text;
	}

	public void OnClickStart()
	{
		MasterAudio.PlaySound("button");
		OnStartGame();
		GameManager.instance.OnLoadScene(3);
	}

	private void OnStartGame()
	{
		playButton.gameObject.SetActive(false);
		if (VariablesGlobales.ownName && !string.IsNullOrEmpty(inputOwnName.text))
		{
			VariablesGlobales.playerOwnName = inputOwnName.text;
			PlayerPrefs.SetString("own_name", VariablesGlobales.playerOwnName);
			VariablesGlobales.randomYoutubeName = false;
		}
		else if (VariablesGlobales.randomYoutubeName)
		{
			VariablesGlobales.ownName = false;
		}
		else
		{
			VariablesGlobales.ownName = false;
		}
	}

	public void Initialize()
	{
		DifficultyMenuManager.instance.OnInitialize();
		if (VariablesGlobales.ownName)
		{
			ownNameCheckBox.OnSelect();
		}
		else
		{
			ownNameCheckBox.OnDeselect();
		}
		VariablesGlobales.tutorialEnabled = PlayerPrefs.GetInt("tutorial", 1) == 1;
		if (VariablesGlobales.tutorialEnabled)
		{
			tutorialCheckBox.OnSelect();
		}
		else
		{
			tutorialCheckBox.OnDeselect();
		}
		if (VariablesGlobales.bloodEnabled)
		{
			bloodCheckbox.OnSelect();
		}
		else
		{
			bloodCheckbox.OnDeselect();
		}
		ToggleAdsState();
		UpdateOwnName();
		SetStreet();
	}

	public void OnNamePropertyChanged()
	{
		VariablesGlobales.ownName = !VariablesGlobales.ownName;
		UpdateOwnName();
	}

	public void UpdateOwnName()
	{
		if (VariablesGlobales.ownName)
		{
			if (PlayerPrefs.HasKey("own_name"))
			{
				inputOwnName.gameObject.SetActive(true);
				inputOwnName.text = PlayerPrefs.GetString("own_name");
				nameTxt.gameObject.SetActive(false);
			}
			else
			{
				inputOwnName.gameObject.SetActive(true);
				nameTxt.gameObject.SetActive(false);
			}
		}
		else
		{
			inputOwnName.gameObject.SetActive(false);
			inputOwnName.text = string.Empty;
			nameTxt.gameObject.SetActive(true);
			if (VariablesGlobales.randomYoutubeName)
			{
				nameTxt.text = VariablesGlobales.youtuberName;
			}
			else
			{
				nameTxt.text = VariablesGlobales.randomName;
			}
		}
	}

	public void OnChangeDifficulty()
	{
		VariablesGlobales.difficultyMode = DifficultyMenuManager.instance.selectedDifficulty.id;
		PlayerPrefs.SetInt("diff", VariablesGlobales.difficultyMode);
	}

	[ContextMenu("TestAdress")]
	public void TestAdress()
	{
	}

	public IEnumerator TestingAdress()
	{
		while (true)
		{
			SetStreet();
			yield return new WaitForSeconds(2f);
		}
	}

	public void SetStreet()
	{
		StreetByLanguage streetByLanguage = streetsByLanguages.Find((StreetByLanguage s) => s.language == VariablesGlobales.currentLanguage);
		if (streetByLanguage != null)
		{
			string data = streetByLanguage.data;
			string[] array = data.Split(';');
			string text = array[Random.Range(0, array.Length)];
			string[] array2 = text.Split(',');
			int num = 0;
			for (int num2 = 0; num2 < Mathf.Clamp(array2.Length, 0, 3); num2++)
			{
				streetTexts[num2].text = array2[num2];
				streetTexts[num2].GetComponent<Localize>().OnLocalize();
				num++;
			}
			for (; num < 3; num++)
			{
				streetTexts[num].text = string.Empty;
			}
		}
	}
}
