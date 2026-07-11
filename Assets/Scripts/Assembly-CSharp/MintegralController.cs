using UnityEngine;

public class MintegralController : MonoBehaviour
{
	public static MintegralController instance;

	public const string androidAppId = "106264";

	public const string iOSAppId = "106264";

	public const string androidApiKey = "4c02e1ebfae5acb8ac6446ddf5b97f0d";

	public const string iOSApiKey = "4c02e1ebfae5acb8ac6446ddf5b97f0d";

	private static string AUTHORITY_KEY_ALL_INFO = "authority_all_info";

	private static string AUTHORITY_KEY_GENERAL_DATA = "authority_general_data";

	private static string AUTHORITY_KEY_DEVICE_ID = "authority_device_id";

	private static string AUTHORITY_KEY_GPS = "authority_gps";

	private static string AUTHORITY_KEY_IMEI_MAC = "authority_imei_mac";

	private static string AUTHORITY_KEY_ANDROID_ID = "authority_android_id";

	private static string AUTHORITY_KEY_APP_LIST = "authority_applist";

	private static string AUTHORITY_KEY_APP_DOWNLOAD = "authority_app_download";

	private static string AUTHORITY_KEY_APP_PROGRESS = "authority_app_progress";

	private static string IS_SWITCH_ON = "ON";

	private static string IS_SWITCH_OFF = "OFF";

	public const string videoInterstitialId = "56880";

	public const string staticInterstitialId = "57611";

	public const string rewardedVideoId = "57812";

	public bool videoInterstitialReady;

	public bool staticInterstitialReady;

	public bool videoRewardedReady;

	public bool adReady
	{
		get
		{
			return videoInterstitialReady || staticInterstitialReady;
		}
	}

	public bool rewardedAdReady
	{
		get
		{
			return videoRewardedReady;
		}
	}

	private void OnEnable()
	{
		MintegralManager.onInterstitialVideoLoadedEvent += onInterstitialVideoLoadedEvent;
		MintegralManager.onInterstitialVideoFailedEvent += onInterstitialVideoFailedEvent;
		MintegralManager.onInterstitialVideoShownEvent += onInterstitialVideoShownEvent;
		MintegralManager.onInterstitialVideoShownFailedEvent += onInterstitialVideoShownFailedEvent;
		MintegralManager.onInterstitialVideoClickedEvent += onInterstitialVideoClickedEvent;
		MintegralManager.onInterstitialVideoDismissedEvent += onInterstitialVideoDismissedEvent;
		MintegralManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MintegralManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
		MintegralManager.onInterstitialShownEvent += onInterstitialShownEvent;
		MintegralManager.onInterstitialShownFailedEvent += onInterstitialShownFailedEvent;
		MintegralManager.onInterstitialClickedEvent += onInterstitialClickedEvent;
		MintegralManager.onInterstitialDismissedEvent += onInterstitialDismissedEvent;
	}

	private void OnDisable()
	{
		MintegralManager.onInterstitialVideoLoadedEvent -= onInterstitialVideoLoadedEvent;
		MintegralManager.onInterstitialVideoFailedEvent -= onInterstitialVideoFailedEvent;
		MintegralManager.onInterstitialVideoShownEvent -= onInterstitialVideoShownEvent;
		MintegralManager.onInterstitialVideoShownFailedEvent -= onInterstitialVideoShownFailedEvent;
		MintegralManager.onInterstitialVideoClickedEvent -= onInterstitialVideoClickedEvent;
		MintegralManager.onInterstitialVideoDismissedEvent -= onInterstitialVideoDismissedEvent;
		MintegralManager.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
		MintegralManager.onInterstitialFailedEvent -= onInterstitialFailedEvent;
		MintegralManager.onInterstitialShownEvent -= onInterstitialShownEvent;
		MintegralManager.onInterstitialShownFailedEvent -= onInterstitialShownFailedEvent;
		MintegralManager.onInterstitialClickedEvent -= onInterstitialClickedEvent;
		MintegralManager.onInterstitialDismissedEvent -= onInterstitialDismissedEvent;
	}

	private void mtgLog(string log)
	{
		Debug.LogError("Mintegral: " + log + "\n------------------------------");
	}

	private void onInterActiveLoadedEvent(string adUnitId)
	{
		mtgLog("onInterActiveLoadedEvent: " + adUnitId);
	}

	private void onInterActiveFailedEvent(string errorMsg)
	{
		mtgLog("onInterActiveFailedEvent: " + errorMsg);
	}

	private void onInterActiveShownEvent(string errorMsg)
	{
		mtgLog("onInterActiveShownEvent: " + errorMsg);
	}

	private void onInterActiveShownFailedEvent(string adUnitId)
	{
		mtgLog("onInterActiveShownFailedEvent: " + adUnitId);
	}

	private void onInterActiveClickedEvent(string adUnitId)
	{
		mtgLog("onInterActiveClickedEvent: " + adUnitId);
	}

	private void onInterActiveDismissedEvent(string errorMsg)
	{
		mtgLog("onInterActiveDismissedEvent: " + errorMsg);
	}

	private void onInterstitialVideoLoadedEvent(string adUnitId)
	{
		mtgLog("onInterstitialVideoLoadedEvent: " + adUnitId);
		videoInterstitialReady = true;
	}

	private void onInterstitialVideoFailedEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoFailedEvent: " + errorMsg);
		videoInterstitialReady = false;
	}

	private void onInterstitialVideoShownEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoShownEvent: " + errorMsg);
		videoInterstitialReady = false;
		AnalyticsController.instance.OnCompleteAd("video", "mintegral", AdsManager.lastPlacement);
		Mintegral.requestInterstitialVideoAd("56880");
	}

	private void onInterstitialVideoShownFailedEvent(string adUnitId)
	{
		mtgLog("onInterstitialVideoShownFailedEvent: " + adUnitId);
		videoInterstitialReady = false;
	}

	private void onInterstitialVideoClickedEvent(string adUnitId)
	{
		mtgLog("onInterstitialVideoClickedEvent: " + adUnitId);
		AnalyticsController.instance.OnClickAd("video", "mintegral", AdsManager.lastPlacement);
	}

	private void onInterstitialVideoDismissedEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoDismissedEvent: " + errorMsg);
		videoInterstitialReady = false;
		Mintegral.requestInterstitialVideoAd("56880");
	}

	private void onInterstitialLoadedEvent()
	{
		mtgLog("onInterstitialLoadedEvent");
		staticInterstitialReady = true;
	}

	private void onInterstitialFailedEvent(string errorMsg)
	{
		mtgLog("onInterstitialFailedEvent: " + errorMsg);
		staticInterstitialReady = false;
	}

	private void onInterstitialShownEvent()
	{
		mtgLog("onInterstitialShownEvent");
		staticInterstitialReady = false;
		AnalyticsController.instance.OnCompleteAd("interstitial", "mintegral", AdsManager.lastPlacement);
		Mintegral.requestInterstitialAd("57611");
	}

	private void onInterstitialShownFailedEvent(string adUnitId)
	{
		mtgLog("onInterstitialShownFailedEvent: " + adUnitId);
		staticInterstitialReady = false;
	}

	private void onInterstitialClickedEvent()
	{
		mtgLog("onInterstitialClickedEvent");
		AnalyticsController.instance.OnClickAd("interstitial", "mintegral", AdsManager.lastPlacement);
	}

	private void onInterstitialDismissedEvent()
	{
		mtgLog("onInterstitialDismissedEvent");
		staticInterstitialReady = false;
		Mintegral.requestInterstitialAd("57611");
	}

	private void onRewardedVideoLoadedEvent(string adUnitId)
	{
		mtgLog("onRewardedVideoLoadedEvent: " + adUnitId);
	}

	private void onRewardedVideoFailedEvent(string errorMsg)
	{
		mtgLog("onRewardedVideoFailedEvent: " + errorMsg);
	}

	private void onRewardedVideoShownFailedEvent(string adUnitId)
	{
		mtgLog("onRewardedVideoShownFailedEvent: " + adUnitId);
	}

	private void onRewardedVideoShownEvent()
	{
		mtgLog("onRewardedVideoShownEvent");
	}

	private void onRewardedVideoClickedEvent(string errorMsg)
	{
		mtgLog("onRewardedVideoClickedEvent: " + errorMsg);
	}

	private void onRewardedVideoClosedEvent(MintegralManager.MTGRewardData rewardData)
	{
		if (rewardData.converted)
		{
			mtgLog("onRewardedVideoClosedEvent: " + rewardData.ToString());
		}
		else
		{
			mtgLog("onRewardedVideoClosedEvent: No Reward");
		}
	}

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		// PC build: Mintegral SDK is Android-native (AndroidJNI) and would crash
		// on Standalone. It's never used since AdsManager keeps isMintegralUser false.
	}

	private void onShowUserInfoTipsEvent(string msg)
	{
		mtgLog("onShowUserInfoTipsEvent: " + msg);
	}

	public void InitializeAds()
	{
		MTGInterstitialVideoInfo[] array = new MTGInterstitialVideoInfo[1];
		MTGInterstitialVideoInfo mTGInterstitialVideoInfo = default(MTGInterstitialVideoInfo);
		mTGInterstitialVideoInfo.adUnitId = "56880";
		array[0] = mTGInterstitialVideoInfo;
		Mintegral.loadInterstitialVideoPluginsForAdUnits(array);
		MTGInterstitialInfo[] array2 = new MTGInterstitialInfo[1];
		MTGInterstitialInfo mTGInterstitialInfo = default(MTGInterstitialInfo);
		mTGInterstitialInfo.adUnitId = "57611";
		mTGInterstitialInfo.adCategory = MTGAdCategory.MTGAD_CATEGORY_ALL;
		array2[0] = mTGInterstitialInfo;
		Mintegral.loadInterstitialPluginsForAdUnits(array2);
		Mintegral.requestInterstitialVideoAd("56880");
		Mintegral.requestInterstitialAd("57611");
		Mintegral.requestRewardedVideo("57812");
	}

	public void ShowTestAd()
	{
		if (adReady)
		{
			ShowAd();
		}
	}

	public void ShowTestRewardedAd()
	{
		if (rewardedAdReady)
		{
			ShowRewardedAd();
		}
	}

	public void ShowAd()
	{
		if (videoInterstitialReady)
		{
			AnalyticsController.instance.OnStartAd("video", "mintegral", AdsManager.lastPlacement);
			Mintegral.showInterstitialVideoAd("56880");
		}
		else if (staticInterstitialReady)
		{
			AnalyticsController.instance.OnStartAd("interstitial", "mintegral", AdsManager.lastPlacement);
			Mintegral.showInterstitialAd("57611");
		}
	}

	public void ShowRewardedAd()
	{
		if (rewardedAdReady)
		{
			AnalyticsController.instance.OnStartAd("rewarded-video", "mintegral", AdsManager.lastPlacement);
			Mintegral.showInterstitialVideoAd("57812");
		}
	}
}
