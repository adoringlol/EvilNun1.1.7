using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
	public static StartMenuManager instance;

	public GameObject rateButton;

	public GameObject startButton;

	public GameObject processingPanel;

	public GameObject purchaseDonePanel;

	public GameObject purchaseFailedPanel;

	public GameObject restoringWindow;

	public GameObject restorePurchasesButton;

	public GameObject removeAdsButton;

	public GameObject alreadyRemoveAdsObject;

	public TextMeshProUGUI versionTxt;

	public GameObject settingsWindow;

	public GameObject creditsWindow;

	public string discord;

	public string fb;

	public string instagram;

	public string twitter;

	public string privacy;

	public CustomSlider musicSlider;

	public CustomSlider soundSlider;

	public CustomCheckBoxBehaviour adsCheckbox;

	private void Awake()
	{
		instance = this;
		restorePurchasesButton.SetActive(false);
		ToggleAdsState();
	}

	private void Start()
	{
		versionTxt.text = Application.version;
		soundSlider.Initialize();
		musicSlider.Initialize();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void ShowWindow()
	{
		MasterAudio.PlaySound("button");
		settingsWindow.SetActive(true);
	}

	public void HideWindow()
	{
		MasterAudio.PlaySound("button");
		settingsWindow.SetActive(false);
		soundSlider.OnSaveChanges();
		musicSlider.OnSaveChanges();
	}

	public void ShowCreditsWindow()
	{
		MasterAudio.PlaySound("button");
		creditsWindow.SetActive(true);
	}

	public void CloseCreditsWindow()
	{
		MasterAudio.PlaySound("button");
		creditsWindow.SetActive(false);
	}

	public void CheckAdDelayed()
	{
		AdsManager.instance.CheckAd();
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

	public void OnClickStart()
	{
		MasterAudio.PlaySound("button");
		startButton.gameObject.SetActive(false);
		GameManager.instance.OnLoadScene(2);
	}

	public void RateGame()
	{
		MasterAudio.PlaySound("button");
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.keplerians.evilnun");
		if (!PlayerPrefs.HasKey("click_rate"))
		{
			PlayerPrefs.SetInt("click_rate", 1);
			AnalyticsController.instance.OnClickRateGame("menu");
		}
	}

	public void OnClickOpenDiscord()
	{
		Application.OpenURL(discord);
	}

	public void OnClickOpenFacebook()
	{
		Application.OpenURL(fb);
	}

	public void OnClickOpenInstagram()
	{
		Application.OpenURL(instagram);
	}

	public void OnClickOpenTwitter()
	{
		Application.OpenURL(twitter);
	}

	public void OnClickRemoveAds()
	{
		if (RemoveAdsWindow.instance != null)
		{
			RemoveAdsWindow.instance.ShowWindow(true);
		}
	}

	public void ShowPrivacy()
	{
		MasterAudio.PlaySound("button");
		Application.OpenURL(privacy);
	}

	public void OnClickRestorePurchases()
	{
		MasterAudio.PlaySound("button");
		IAPManager.instance.RestorePurchases();
	}

	public void ShowProcessingWindow(bool t)
	{
		processingPanel.SetActive(t);
	}

	public void ShowPurchasedWindow()
	{
		purchaseDonePanel.SetActive(true);
	}

	public void ClosePurchasedWindow()
	{
		MasterAudio.PlaySound("button");
		purchaseDonePanel.SetActive(false);
	}

	public void ShowPurchaseFailedWindow()
	{
		purchaseFailedPanel.SetActive(true);
	}

	public void HidePurchaseFailedWindow()
	{
		MasterAudio.PlaySound("button");
		purchaseFailedPanel.SetActive(false);
	}

	public void ShowRestoringPurchasesWindow()
	{
		restoringWindow.SetActive(true);
	}

	public void HideRestoringPurchasesWindow()
	{
		restoringWindow.SetActive(false);
	}

	public void TestMintegralAds()
	{
		MintegralController.instance.ShowTestAd();
	}
}
