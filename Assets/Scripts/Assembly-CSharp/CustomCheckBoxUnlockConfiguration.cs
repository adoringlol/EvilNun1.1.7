using UnityEngine;

public class CustomCheckBoxUnlockConfiguration : MonoBehaviour
{
	public GameObject adsEnabledText;

	public GameObject adsDisabledText;

	public void OnEnableAds(bool t)
	{
		adsEnabledText.SetActive(t);
		adsDisabledText.SetActive(!t);
	}
}
