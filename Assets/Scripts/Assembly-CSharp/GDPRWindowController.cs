using DarkTonic.MasterAudio;
using UnityEngine;

public class GDPRWindowController : MonoBehaviour
{
	public GameObject window;

	public const string pprefsKey = "gdpr_consent";

	private bool isNewUser = true;

	private void Start()
	{
		isNewUser = !PlayerPrefs.HasKey("gdpr_consent");
	}

	public void ShowWindow()
	{
		window.SetActive(true);
	}

	public void HideWindow()
	{
		window.SetActive(false);
	}

	public void OnAccept()
	{
		MasterAudio.PlaySound("button");
		HideWindow();
		PlayerPrefs.SetInt("gdpr_consent", 1);
		AnalyticsController.instance.OnFinishGDPR(true);
		isNewUser = false;
	}

	public void OnDecline()
	{
		MasterAudio.PlaySound("button");
		HideWindow();
		PlayerPrefs.SetInt("gdpr_consent", 0);
		AnalyticsController.instance.OnFinishGDPR(false);
		isNewUser = false;
	}
}
