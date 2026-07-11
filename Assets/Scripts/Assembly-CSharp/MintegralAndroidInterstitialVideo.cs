using UnityEngine;

public class MintegralAndroidInterstitialVideo
{
	private readonly AndroidJavaObject _interstitialVideoPlugin;

	public MintegralAndroidInterstitialVideo(MTGInterstitialVideoInfo info)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialVideoPlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGInterstitialVideo", info.adUnitId);
		}
	}

	public void requestInterstitialVideoAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialVideoPlugin.Call("preloadInterstitialVideo");
		}
	}

	public void showInterstitialVideoAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialVideoPlugin.Call("showInterstitialVideo");
		}
	}
}
