using UnityEngine;

public class MintegralAndroidAppWall
{
	private readonly AndroidJavaObject _appWallPlugin;

	public MintegralAndroidAppWall(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_appWallPlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGAppWall", adUnitId);
		}
	}

	public static void preLoadAppWallAd(string adUnitId)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.mintegral.msdk.unity.MTGAppWall", adUnitId);
		androidJavaObject.Call("preloadWall", adUnitId);
	}

	public void showAppWallAd(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_appWallPlugin.Call("openWall", adUnitId);
		}
	}
}
