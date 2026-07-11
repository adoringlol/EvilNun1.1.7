using System;
using System.Collections.Generic;
using UnityEngine;

public class Mintegral
{
	public const string ADUNIT_NOT_FOUND_MSG = "AdUnit {0} not found: no plugin was initialized";

	public static IntPtr nativeMobManager;

	public static MTGNativeInfo nativeMobInfo;

	private static Dictionary<string, MintegralAndroidAppWall> _appWallPluginsDict = new Dictionary<string, MintegralAndroidAppWall>();

	private static Dictionary<string, MintegralAndroidInterActive> _interActivePluginsDict = new Dictionary<string, MintegralAndroidInterActive>();

	private static Dictionary<string, MintegralAndroidInterstitialVideo> _interstitialVideoPluginsDict = new Dictionary<string, MintegralAndroidInterstitialVideo>();

	private static Dictionary<string, MintegralAndroidInterstitial> _interstitialPluginsDict = new Dictionary<string, MintegralAndroidInterstitial>();

	private static Dictionary<string, MintegralAndroidRewardedVideo> _rewardedVideoPluginsDict = new Dictionary<string, MintegralAndroidRewardedVideo>();

	private static Dictionary<string, MintegralAndroidOfferWall> _offerWallPluginsDict = new Dictionary<string, MintegralAndroidOfferWall>();

	private static Dictionary<string, MintegralAndroidNative> _nativePluginsDict = new Dictionary<string, MintegralAndroidNative>();

	public static void loadInterActivePluginsForAdUnits(MTGInterActiveInfo[] interActiveAdInfos)
	{
		for (int i = 0; i < interActiveAdInfos.Length; i++)
		{
			MTGInterActiveInfo info = interActiveAdInfos[i];
			if (!_interActivePluginsDict.ContainsKey(info.adUnitId))
			{
				_interActivePluginsDict.Add(info.adUnitId, new MintegralAndroidInterActive(info));
			}
		}
	}

	public static void loadInterstitialVideoPluginsForAdUnits(MTGInterstitialVideoInfo[] interstitialVideoAdInfos)
	{
		for (int i = 0; i < interstitialVideoAdInfos.Length; i++)
		{
			MTGInterstitialVideoInfo info = interstitialVideoAdInfos[i];
			if (!_interstitialVideoPluginsDict.ContainsKey(info.adUnitId))
			{
				_interstitialVideoPluginsDict.Add(info.adUnitId, new MintegralAndroidInterstitialVideo(info));
			}
		}
	}

	public static void loadInterstitialPluginsForAdUnits(MTGInterstitialInfo[] interstitialAdInfos)
	{
		for (int i = 0; i < interstitialAdInfos.Length; i++)
		{
			MTGInterstitialInfo info = interstitialAdInfos[i];
			if (!_interstitialPluginsDict.ContainsKey(info.adUnitId))
			{
				_interstitialPluginsDict.Add(info.adUnitId, new MintegralAndroidInterstitial(info));
			}
		}
	}

	public static void loadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			if (!_rewardedVideoPluginsDict.ContainsKey(text))
			{
				_rewardedVideoPluginsDict.Add(text, new MintegralAndroidRewardedVideo(text));
			}
		}
	}

	public static void loadAppWallPluginsForAdUnits(string[] appWallAdUnitIds)
	{
		foreach (string text in appWallAdUnitIds)
		{
			if (!_appWallPluginsDict.ContainsKey(text))
			{
				_appWallPluginsDict.Add(text, new MintegralAndroidAppWall(text));
			}
		}
	}

	public static void loadOfferWallPluginsForAdUnits(MTGOfferWallInfo[] offerWallAdInfos)
	{
		for (int i = 0; i < offerWallAdInfos.Length; i++)
		{
			MTGOfferWallInfo info = offerWallAdInfos[i];
			if (!_offerWallPluginsDict.ContainsKey(info.adUnitId))
			{
				_offerWallPluginsDict.Add(info.adUnitId, new MintegralAndroidOfferWall(info));
			}
		}
	}

	public static void loadNativePluginsForAdUnits(MTGNativeInfo[] nativeAdInfos)
	{
		for (int i = 0; i < nativeAdInfos.Length; i++)
		{
			MTGNativeInfo nativeAdInfo = nativeAdInfos[i];
			if (!_nativePluginsDict.ContainsKey(nativeAdInfo.adUnitId))
			{
				_nativePluginsDict.Add(nativeAdInfo.adUnitId, new MintegralAndroidNative(nativeAdInfo));
				nativeMobInfo = nativeAdInfo;
			}
		}
	}

	public static void initMTGSDK(string appId, string apiKey)
	{
		Debug.LogError("mintegral: initMTGSDK \n------------------------------");
		MintegralConfig.initialMTGSDK(appId, apiKey);
	}

	public static void showUserPrivateInfoTips()
	{
		MintegralConfig.showUserPrivateInfoTipsConfig();
	}

	public static void setUserPrivateInfoType(string statusKey, string statusType)
	{
		MintegralConfig.setUserPrivateInfoTypeConfig(statusKey, statusType);
	}

	public static int userPrivateInfo(string statusKey)
	{
		return MintegralConfig.userPrivateInfoConfig(statusKey);
	}

	public static void setConsentStatusInfoType(int statusType)
	{
		MintegralConfig.setConsentStatusInfoTypeConfig(statusType);
	}

	public static bool getConsentStatusInfoType()
	{
		return MintegralConfig.getConsentStatusInfoTypeConfig();
	}

	public static void requestInterActiveAd(string adUnitId)
	{
		MintegralAndroidInterActive value;
		if (_interActivePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestInterActiveAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showInterActiveAd(string adUnitId)
	{
		MintegralAndroidInterActive value;
		if (_interActivePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showInterActiveAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static int getInterActiveStatusAd(string adUnitId)
	{
		MintegralAndroidInterActive value;
		if (_interActivePluginsDict.TryGetValue(adUnitId, out value))
		{
			return value.getInterActiveStatusAd();
		}
		return 0;
	}

	public static void requestInterstitialVideoAd(string adUnitId)
	{
		MintegralAndroidInterstitialVideo value;
		if (_interstitialVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestInterstitialVideoAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showInterstitialVideoAd(string adUnitId)
	{
		MintegralAndroidInterstitialVideo value;
		if (_interstitialVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showInterstitialVideoAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void requestInterstitialAd(string adUnitId)
	{
		MintegralAndroidInterstitial value;
		if (_interstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestInterstitialAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showInterstitialAd(string adUnitId)
	{
		MintegralAndroidInterstitial value;
		if (_interstitialPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showInterstitialAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void requestRewardedVideo(string adUnitId)
	{
		MintegralAndroidRewardedVideo value;
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestRewardedVideoAd(adUnitId);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showRewardedVideo(string adUnitId, string rewardId = "rewardid", string userId = "userId")
	{
		if (rewardId.Length == 0 || rewardId == null)
		{
			rewardId = "defaultRewardId";
		}
		MintegralAndroidRewardedVideo value;
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showRewardedVideoAd(adUnitId, rewardId, userId);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static bool isVideoReadyToPlay(string adUnitId)
	{
		bool result = false;
		MintegralAndroidRewardedVideo value;
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			result = value.isVideoReady(adUnitId);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
		return result;
	}

	public static void cleanAllVideoFileCache(string adUnitId)
	{
		MintegralAndroidRewardedVideo value;
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.cleanVideoFileCache();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showAppWallAd(string adUnitId)
	{
		MintegralAndroidAppWall value;
		if (_appWallPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showAppWallAd(adUnitId);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void requestOfferWallAd(string adUnitId)
	{
		MintegralAndroidOfferWall value;
		if (_offerWallPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestOfferWallAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void showOfferWallAd(string adUnitId)
	{
		MintegralAndroidOfferWall value;
		if (_offerWallPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.showOfferWallAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void queryOfferWallRewards(string adUnitId)
	{
		MintegralAndroidOfferWall value;
		if (_offerWallPluginsDict.TryGetValue(adUnitId, out value))
		{
			value.queryOfferWallRewards();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void preRequestNativeAd(string adUnitId, string fb_placement_id, string categoryType, MTGTemplate[] supportedTemplate)
	{
		MintegralAndroidNative value;
		if (_nativePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.preRequestNativeAd(adUnitId, fb_placement_id, categoryType, supportedTemplate);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void requestNativeAd(string adUnitId)
	{
		MintegralAndroidNative value;
		if (_nativePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.requestNativeAd();
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void registerViewNativeAd(string adUnitId, int left, int top, int width, int height, string index_campain)
	{
		MintegralAndroidNative value;
		if (_nativePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.registerViewNativeAd(left, top, width, height, index_campain);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}

	public static void unRegisterViewNativeAd(string adUnitId, string index_campain)
	{
		MintegralAndroidNative value;
		if (_nativePluginsDict.TryGetValue(adUnitId, out value))
		{
			value.unRegisterViewNativeAd(index_campain);
		}
		else
		{
			Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
		}
	}
}
