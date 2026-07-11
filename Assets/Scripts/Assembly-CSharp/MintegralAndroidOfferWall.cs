using UnityEngine;

public class MintegralAndroidOfferWall
{
	private readonly AndroidJavaObject _offerWallPlugin;

	public MintegralAndroidOfferWall(MTGOfferWallInfo info)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			MTGAdCategory adCategory = info.adCategory;
			string text = adCategory.ToString("d");
			string text2 = JsonUtility.ToJson(info.alertTips);
			_offerWallPlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGOfferWall", info.adUnitId, info.userId, text, text2);
		}
	}

	public void requestOfferWallAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_offerWallPlugin.Call("loadOfferwall");
		}
	}

	public void showOfferWallAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_offerWallPlugin.Call("showOfferwall");
		}
	}

	public void queryOfferWallRewards()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_offerWallPlugin.Call("queryOfferwallRewards");
		}
	}
}
