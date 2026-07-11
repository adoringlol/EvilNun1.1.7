using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Analytics;

public class ExtraLifeWindowController : MonoBehaviour
{
	public GameObject optionsButtons;

	public void OnEnable()
	{
		optionsButtons.SetActive(true);
	}

	public void CloseWindow()
	{
		base.gameObject.SetActive(false);
		optionsButtons.SetActive(false);
	}

	public void ClickWatchVideo()
	{
		MasterAudio.PlaySound("button");
		if (AdsManager.instance.isRewardedAdReady)
		{
			AnalyticsEvent.Custom("show_rewardedAd");
			PlayerPrefs.SetInt("adAmount", PlayerPrefs.GetInt("adAmount", 0) + 1);
			AdsManager.lastPlacement = "extralife";
			AdsManager.instance.ShowRewardedAd();
			GameUIController.instance.watchedVideoToContinue = true;
			CloseWindow();
		}
	}

	public void ClickContinue()
	{
		MasterAudio.PlaySound("button");
		GameUIController.instance.ShowNewDayPanel();
		CloseWindow();
	}
}
