using UnityEngine;

public class MintegralAndroidRewardedVideo
{
	private readonly AndroidJavaObject _plugin;

	public MintegralAndroidRewardedVideo(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGReward", adUnitId);
		}
	}

	public void requestRewardedVideoAd(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("loadRewardView");
		}
	}

	public void showRewardedVideoAd(string adUnitId, string rewardId, string userId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showRewardView", rewardId, userId);
		}
	}

	public void cleanVideoFileCache()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("cleanRewardView");
		}
	}

	public bool isVideoReady(string adUnitId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("isReady", new object[0]);
	}
}
