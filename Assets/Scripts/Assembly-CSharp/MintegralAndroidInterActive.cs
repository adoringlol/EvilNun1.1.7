using UnityEngine;

public class MintegralAndroidInterActive
{
	private readonly AndroidJavaObject _interActivePlugin;

	public MintegralAndroidInterActive(MTGInterActiveInfo info)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interActivePlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGInterActive", info.adUnitId);
		}
	}

	public void requestInterActiveAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interActivePlugin.Call("preloadInterActive");
		}
	}

	public void showInterActiveAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interActivePlugin.Call("showInterActive");
		}
	}

	public int getInterActiveStatusAd()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return 0;
		}
		return _interActivePlugin.Call<int>("getInterActiveStatus", new object[0]);
	}
}
