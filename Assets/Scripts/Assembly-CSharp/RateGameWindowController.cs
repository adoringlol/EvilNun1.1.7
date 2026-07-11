using DarkTonic.MasterAudio;
using UnityEngine;

public class RateGameWindowController : MonoBehaviour
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

	public void ClickRateGame()
	{
		// PC build: no Play Store to rate on. Just remember the click and close.
		MasterAudio.PlaySound("button");
		PlayerPrefs.SetInt("click_rate", 1);
		CloseWindow();
	}

	public void ClickRateLater()
	{
		AnalyticsController.instance.OnClickRateGame("later");
		MasterAudio.PlaySound("button");
		CloseWindow();
	}
}
