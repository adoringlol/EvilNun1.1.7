using UnityEngine;

public class MintegralAndroidNative
{
	private readonly AndroidJavaObject _nativePlugin;

	public MintegralAndroidNative(MTGNativeInfo nativeAdInfo)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string text = JsonUtility.ToJson(new MTGTemplateWrapper
			{
				objects = nativeAdInfo.supportedTemplate
			});
			_nativePlugin = new AndroidJavaObject("com.mintegral.msdk.unity.MTGNative", nativeAdInfo.adUnitId, nativeAdInfo.fb_placement_id, nativeAdInfo.adCategory.ToString(), text);
		}
	}

	public void preRequestNativeAd(string adUnitId, string fb_placement_id, string categoryType, MTGTemplate[] supportedTemplates)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			MTGTemplateWrapper mTGTemplateWrapper = new MTGTemplateWrapper();
			mTGTemplateWrapper.objects = supportedTemplates;
			string text = JsonUtility.ToJson(mTGTemplateWrapper);
			_nativePlugin.Call("preloadNative", adUnitId, fb_placement_id, categoryType, text);
		}
	}

	public void requestNativeAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_nativePlugin.Call("loadNative");
		}
	}

	public void registerViewNativeAd(int left, int top, int width, int height, string index_campain)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_nativePlugin.Call("registerView", left, top, width, height, index_campain);
		}
	}

	public void unRegisterViewNativeAd(string index_campain)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_nativePlugin.Call("unRegisterView", index_campain);
		}
	}
}
