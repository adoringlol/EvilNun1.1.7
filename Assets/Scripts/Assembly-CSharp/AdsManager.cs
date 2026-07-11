using System.Collections;
using I2.Loc.SimpleJSON;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
	public enum AdNetwork
	{
		UNITY = 0,
		MINTEGRAL = 1
	}

	public static AdsManager instance;

	public float nextAdTime;

	public const int minimumAdsForDonation = 5;

	public const int minAdTime = 60;

	public const int plusTime = 15;

	public const int ghostModeSecondsForAd = 150;

	public bool isMintegralUser;

	public bool isConnected;

	public string country;

	public static string lastPlacement = string.Empty;

	public bool isAdReady
	{
		get
		{
			// PC build: no interstitial ads.
			return false;
		}
	}

	public bool isRewardedAdReady
	{
		get
		{
			// PC build: always "ready" so the optional reward is available for free.
			return true;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
		Initialize();
		CalculateNextAdTime(false, 0f);
	}

	private void Initialize()
	{
		// PC build: ads removed. Behave as if "remove ads" was purchased so no
		// ad SDK, startup ad, or ghost-mode ad ever runs.
		VariablesGlobales.removeAdsBought = true;
		VariablesGlobales.adsEnabled = false;
	}

	public void UpdateCountryConfig(bool start = false)
	{
		if (!PlayerPrefs.HasKey("country"))
		{
			CheckConnection();
			return;
		}
		if (start)
		{
			country = PlayerPrefs.GetString("country");
		}
		UpdateMintegralConfig();
	}

	public void CheckConnection()
	{
		// PC build: no ad network to bring online.
	}

	public void UpdateMintegralConfig()
	{
		// PC build: Mintegral ad network disabled.
	}

	public IEnumerator UpdateConnection()
	{
		yield return StartCoroutine("GetCountryCode");
		if (!string.IsNullOrEmpty(country))
		{
			UpdateMintegralConfig();
		}
	}

	public IEnumerator GetCountryCode()
	{
		string url = "https://geoip.nekudo.com/api/";
		using (WWW www = new WWW(url))
		{
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				Debug.Log(www.text);
				JSONNode jSONNode = JSON.Parse(www.text);
				string text = jSONNode["country"]["name"];
				Debug.Log("Your country is " + text);
				country = text;
				PlayerPrefs.SetString("country", country);
				AnalyticsController.instance.OnMintegralUserInitialized(country);
			}
		}
	}

	public void OnBackToMenu()
	{
		StopCoroutine("GhostModeAds");
		CheckAd("backtomenu");
	}

	public void OnStartGhostMode()
	{
		if (!VariablesGlobales.removeAdsBought)
		{
			StartCoroutine("GhostModeAds");
		}
	}

	public IEnumerator GhostModeAds()
	{
		while (true)
		{
			if (!GameUIController.instance.completed && !GameUIController.instance.gameOver)
			{
				yield return new WaitForSeconds(145f);
				MessagesManager.instance.ShowMessage("ghost_ad_message");
				yield return new WaitForSeconds(5f);
				if (isAdReady)
				{
					AnalyticsEvent.Custom("show_ad_ghostmode");
					PlayerPrefs.SetInt("adAmount", PlayerPrefs.GetInt("adAmount", 0) + 1);
					lastPlacement = "ingame";
					CalculateNextAdTime(true, Time.time - nextAdTime);
					ShowAd();
				}
			}
			yield return null;
		}
	}

	public void ShowAd()
	{
		// PC build: interstitial ads removed. No-op.
	}

	public void ShowRewardedAd()
	{
		// PC build: no ad to show, so grant the reward immediately.
		CalculateNextAdTime(true, Time.time - nextAdTime);
		if (ProgressManager.instance != null)
		{
			ProgressManager.instance.OnRewardedVideoCompleted();
		}
	}

	public IEnumerator ShowStartUpAdvertisement()
	{
		yield return new WaitForSeconds(0.5f);
		while (!VariablesGlobales.removeAdsBought)
		{
			if (SceneManager.GetActiveScene().buildIndex == 1)
			{
				if (isAdReady)
				{
					AnalyticsEvent.Custom("show_ad");
					PlayerPrefs.SetInt("adAmount", PlayerPrefs.GetInt("adAmount", 0) + 1);
					lastPlacement = "startup";
					ShowAd();
					break;
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex > 1)
			{
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	public void CalculateNextAdTime(bool isVideo, float diff)
	{
		if (isVideo)
		{
			if (diff > 30f)
			{
				nextAdTime = Time.time + 60f;
			}
			else
			{
				nextAdTime = Time.time + 60f + 15f;
			}
		}
		else if (diff > 15f)
		{
			nextAdTime = Time.time + 45f;
		}
		else
		{
			nextAdTime = Time.time + 60f;
		}
	}

	public void OnPayRemoveAds()
	{
		VariablesGlobales.removeAdsBought = true;
		VariablesGlobales.adsEnabled = false;
		PlayerPrefs.SetInt("remove_ads", 1);
		if (StartMenuManager.instance != null)
		{
			StartMenuManager.instance.ToggleAdsState();
		}
		else if (MenuManager.instance != null)
		{
			MenuManager.instance.ToggleAdsState();
			DifficultyMenuManager.instance.UpdateDifficulty();
		}
		else if (PauseController.instance != null)
		{
			PauseController.instance.ToggleAdsState();
		}
		IAPHelperWindow.instance.OnPurchaseCompleted();
	}

	public void FetchAd()
	{
	}

	public void CheckAd(string placement = "ingame")
	{
		if (!VariablesGlobales.removeAdsBought && Time.time >= nextAdTime && !GameUIController.instance.gameOver && isAdReady)
		{
			CalculateNextAdTime(true, Time.time - nextAdTime);
			AnalyticsEvent.Custom("show_ad");
			PlayerPrefs.SetInt("adAmount", PlayerPrefs.GetInt("adAmount", 0) + 1);
			lastPlacement = placement;
			ShowAd();
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			AnalyticsController.instance.OnCompleteAd("video", "unity", lastPlacement);
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

	private void HandleShowResultRewarded(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			AnalyticsController.instance.OnCompleteAd("rewarded-video", "unity", lastPlacement);
			ProgressManager.instance.OnRewardedVideoCompleted();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			ProgressManager.instance.OnRewardedVideoFailedOrSkip();
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			ProgressManager.instance.OnRewardedVideoFailedOrSkip();
			break;
		}
	}
}
