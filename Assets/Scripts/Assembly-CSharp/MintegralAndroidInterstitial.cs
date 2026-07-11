using UnityEngine;

public class MintegralAndroidInterstitial
{
	private readonly AndroidJavaObject _interstitialPlugin;

	public MintegralAndroidInterstitial(MTGInterstitialInfo info)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			MTGAdCategory adCategory = info.adCategory;
			string text = adCategory.ToString("d");
			_interstitialPlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGInterstitial", info.adUnitId, text);
		}
	}

	public void requestInterstitialAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialPlugin.Call("preloadInterstitial");
		}
	}

	public void showInterstitialAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialPlugin.Call("showInterstitial");
		}
	}
}
