using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MintegralDemoGUI : MonoBehaviour
{
	private int _selectedToggleIndex;

	private string[] _AdUnits;

	private const string InterActive = "InterActive";

	private const string InterstitialVideo = "InterstitialVideo";

	private const string Interstitial = "Interstitial";

	private const string RewardVideo = "RewardVideo";

	private const string OfferWall = "OfferWall";

	private const string AppWall = "AppWall";

	private const string Native = "Native";

	public const string MTGSDKAppIDForiOS = "92763";

	public const string MTGSDKApiKeyForiOS = "936dcbdd57fe235fd7cf61c2e93da3c4";

	public const string MTGSDKAppIDForAndroid = "92762";

	public const string MTGSDKApiKeyForAndroid = "936dcbdd57fe235fd7cf61c2e93da3c4";

	private int GDPR_ON = 1;

	private int GDPR_OFF;

	private string[] _adTypeList = new string[7] { "InterActive", "InterstitialVideo", "Interstitial", "RewardVideo", "OfferWall", "AppWall", "Native" };

	private Dictionary<string, string[]> _adUnitDict = new Dictionary<string, string[]>
	{
		{
			"InterActive",
			new string[1] { "48127" }
		},
		{
			"InterstitialVideo",
			new string[1] { "35811" }
		},
		{
			"Interstitial",
			new string[1] { "21312" }
		},
		{
			"RewardVideo",
			new string[2] { "21310", "30043" }
		},
		{
			"OfferWall",
			new string[1] { "21311" }
		},
		{
			"AppWall",
			new string[1] { "21308" }
		},
		{
			"Native",
			new string[2] { "21306", "1611993839047594_1614040148842963" }
		}
	};

	private static bool IsAdUnitArrayNullOrEmpty(string[] adUnitArray)
	{
		return adUnitArray == null || adUnitArray.Length == 0;
	}

	private void Start()
	{
		initMTGSDK();
		initAllAds();
	}

	private void initMTGSDK()
	{
		Mintegral.setConsentStatusInfoType(GDPR_ON);
		mtgLog("userPrivateInfo ConsentStatus : " + Mintegral.getConsentStatusInfoType());
		Mintegral.initMTGSDK("92762", "936dcbdd57fe235fd7cf61c2e93da3c4");
	}

	private void initAllAds()
	{
		string[] array = ((!_adUnitDict.ContainsKey("InterActive")) ? null : _adUnitDict["InterActive"]);
		MTGInterActiveInfo[] array2 = new MTGInterActiveInfo[array.Length];
		MTGInterActiveInfo mTGInterActiveInfo = default(MTGInterActiveInfo);
		for (int i = 0; i < array.Length; i++)
		{
			string adUnitId = array[i];
			mTGInterActiveInfo.adUnitId = adUnitId;
			array2[i] = mTGInterActiveInfo;
		}
		Mintegral.loadInterActivePluginsForAdUnits(array2);
		string[] array3 = ((!_adUnitDict.ContainsKey("InterstitialVideo")) ? null : _adUnitDict["InterstitialVideo"]);
		MTGInterstitialVideoInfo[] array4 = new MTGInterstitialVideoInfo[array3.Length];
		MTGInterstitialVideoInfo mTGInterstitialVideoInfo = default(MTGInterstitialVideoInfo);
		for (int j = 0; j < array3.Length; j++)
		{
			string adUnitId2 = array3[j];
			mTGInterstitialVideoInfo.adUnitId = adUnitId2;
			array4[j] = mTGInterstitialVideoInfo;
		}
		Mintegral.loadInterstitialVideoPluginsForAdUnits(array4);
		string[] array5 = ((!_adUnitDict.ContainsKey("Interstitial")) ? null : _adUnitDict["Interstitial"]);
		MTGInterstitialInfo[] array6 = new MTGInterstitialInfo[array5.Length];
		MTGInterstitialInfo mTGInterstitialInfo = default(MTGInterstitialInfo);
		for (int k = 0; k < array5.Length; k++)
		{
			string adUnitId3 = array5[k];
			mTGInterstitialInfo.adUnitId = adUnitId3;
			mTGInterstitialInfo.adCategory = MTGAdCategory.MTGAD_CATEGORY_ALL;
			array6[k] = mTGInterstitialInfo;
		}
		Mintegral.loadInterstitialPluginsForAdUnits(array6);
		string[] rewardedVideoAdUnitIds = ((!_adUnitDict.ContainsKey("RewardVideo")) ? null : _adUnitDict["RewardVideo"]);
		Mintegral.loadRewardedVideoPluginsForAdUnits(rewardedVideoAdUnitIds);
		string[] array7 = ((!_adUnitDict.ContainsKey("OfferWall")) ? null : _adUnitDict["OfferWall"]);
		MTGOfferWallInfo[] array8 = new MTGOfferWallInfo[array7.Length];
		MTGOfferWallInfo mTGOfferWallInfo = default(MTGOfferWallInfo);
		MTGOfferWallAlertTips alertTips = default(MTGOfferWallAlertTips);
		for (int l = 0; l < array7.Length; l++)
		{
			string adUnitId4 = array7[l];
			mTGOfferWallInfo.adUnitId = adUnitId4;
			mTGOfferWallInfo.adCategory = MTGAdCategory.MTGAD_CATEGORY_ALL;
			mTGOfferWallInfo.userId = "the userid ";
			alertTips.leftButtonTitle = "close";
			alertTips.rightButtonTitle = "continue";
			alertTips.alertContent = "if close this video, you will not get the reward";
			mTGOfferWallInfo.alertTips = alertTips;
			array8[l] = mTGOfferWallInfo;
		}
		Mintegral.loadOfferWallPluginsForAdUnits(array8);
		string[] appWallAdUnitIds = ((!_adUnitDict.ContainsKey("AppWall")) ? null : _adUnitDict["AppWall"]);
		Mintegral.loadAppWallPluginsForAdUnits(appWallAdUnitIds);
		string[] array9 = ((!_adUnitDict.ContainsKey("Native")) ? null : _adUnitDict["Native"]);
		MTGNativeInfo[] array10 = new MTGNativeInfo[1];
		MTGNativeInfo mTGNativeInfo = default(MTGNativeInfo);
		for (int m = 0; m < 1; m++)
		{
			string adUnitId5 = array9[0];
			string fb_placement_id = array9[1];
			mTGNativeInfo.adUnitId = adUnitId5;
			mTGNativeInfo.fb_placement_id = fb_placement_id;
			mTGNativeInfo.autoCacheImage = false;
			mTGNativeInfo.adCategory = MTGAdCategory.MTGAD_CATEGORY_ALL;
			mTGNativeInfo.supportedTemplate = new MTGTemplate[1]
			{
				new MTGTemplate
				{
					templateType = MTGAdTemplateType.MTGAD_TEMPLATE_BIG_IMAGE,
					adsNum = 3
				}
			};
			array10[0] = mTGNativeInfo;
		}
		Mintegral.loadNativePluginsForAdUnits(array10);
	}

	private void loadAdType(int adTypeIndex, string adUnit)
	{
		mtgLog("requesting " + _adTypeList[adTypeIndex] + " with AdUnit: " + adUnit);
		switch (adTypeIndex)
		{
		case 0:
			Mintegral.requestInterActiveAd(adUnit);
			break;
		case 1:
			Mintegral.requestInterstitialVideoAd(adUnit);
			break;
		case 2:
			Mintegral.requestInterstitialAd(adUnit);
			break;
		case 3:
			Mintegral.requestRewardedVideo(adUnit);
			break;
		case 4:
			Mintegral.requestOfferWallAd(adUnit);
			break;
		}
	}

	private void showAdType(int adTypeIndex, string adUnit)
	{
		mtgLog("showing " + _adTypeList[adTypeIndex] + " with AdUnit: " + adUnit);
		switch (adTypeIndex)
		{
		case 0:
			Mintegral.showInterActiveAd(adUnit);
			mtgLog("status: " + Mintegral.getInterActiveStatusAd(adUnit));
			break;
		case 1:
			Mintegral.showInterstitialVideoAd(adUnit);
			break;
		case 2:
			Mintegral.showInterstitialAd(adUnit);
			break;
		case 3:
			if (Mintegral.isVideoReadyToPlay(adUnit))
			{
				Mintegral.showRewardedVideo(adUnit);
			}
			else
			{
				mtgLog("Reward Unit:" + adUnit + "Not Ready");
			}
			break;
		case 4:
			Mintegral.showOfferWallAd(adUnit);
			break;
		case 5:
			Mintegral.showAppWallAd(adUnit);
			break;
		}
	}

	private void queryOfferWallRewards(string adUnit)
	{
		Mintegral.queryOfferWallRewards(adUnit);
	}

	private void OnGUI()
	{
		GUIStyle style = GUI.skin.GetStyle("label");
		style.fontSize = 30;
		GUIStyle style2 = GUI.skin.GetStyle("button");
		style2.fontSize = 20;
		GUI.skin.button.margin = new RectOffset(0, 0, 10, 0);
		GUI.skin.button.stretchWidth = true;
		GUI.skin.button.fixedHeight = ((Screen.width < 960 && Screen.height < 960) ? 50 : 75);
		int num = 20;
		_selectedToggleIndex = GUI.Toolbar(new Rect(0f, (float)Screen.height - GUI.skin.button.fixedHeight, Screen.width, GUI.skin.button.fixedHeight), _selectedToggleIndex, _adTypeList);
		string text = _adTypeList[_selectedToggleIndex];
		_AdUnits = ((!_adUnitDict.ContainsKey(text)) ? null : _adUnitDict[text]);
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Space(num);
		GUILayout.Label(text + ":");
		if (!IsAdUnitArrayNullOrEmpty(_AdUnits))
		{
			string[] adUnits = _AdUnits;
			foreach (string text2 in adUnits)
			{
				GUILayout.BeginHorizontal();
				if (_selectedToggleIndex == 6)
				{
					SceneManager.LoadScene("NativeAdsScene");
				}
				else
				{
					if (_selectedToggleIndex != 5 && GUILayout.Button("Request: " + text2))
					{
						loadAdType(_selectedToggleIndex, text2);
					}
					if (GUILayout.Button("Show"))
					{
						showAdType(_selectedToggleIndex, text2);
					}
					if (_selectedToggleIndex == 4 && GUILayout.Button("queryRewards"))
					{
						queryOfferWallRewards(text2);
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			GUILayout.Label("No AdUnits for " + text);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void mtgLog(string log)
	{
		Debug.LogError("Mintegral: " + log + "\n------------------------------");
	}
}
