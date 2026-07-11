using UnityEngine;

public class MintegralConfig
{
	private static bool isInit = false;

	private static readonly AndroidJavaObject _staticObject = new AndroidJavaObject("com.mintegral.msdk.unity.UnityAndroidBridge");

	public static void initialMTGSDK(string appId, string apiKey)
	{
		if (!isInit)
		{
			isInit = true;
			if (Application.platform != RuntimePlatform.OSXEditor)
			{
				_staticObject.Call("initApplication", appId, apiKey);
			}
		}
	}

	public static void showUserPrivateInfoTipsConfig()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_staticObject.Call("showUserPrivateInfoTips");
		}
	}

	public static void setUserPrivateInfoTypeConfig(string statusKey, string statusType)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_staticObject.Call("setUserPrivateInfoType", statusKey, statusType);
		}
	}

	public static int userPrivateInfoConfig(string statusKey)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			return _staticObject.Call<int>("getAuthPrivateInfoStatus", new object[1] { statusKey });
		}
		return 0;
	}

	public static void setConsentStatusInfoTypeConfig(int statusType)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_staticObject.Call("setConsentStatusInfoType", statusType);
		}
	}

	public static bool getConsentStatusInfoTypeConfig()
	{
		bool result = false;
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			result = _staticObject.Call<bool>("getConsentStatusInfoType", new object[0]);
		}
		return result;
	}
}
