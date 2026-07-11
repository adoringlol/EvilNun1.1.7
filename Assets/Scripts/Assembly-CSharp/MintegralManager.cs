using System;
using System.Collections.Generic;
using MintegralInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class MintegralManager : MonoBehaviour
{
	public class MTGRewardData
	{
		public string rewardName;

		public float rewardAmount;

		public bool converted;

		public MTGRewardData(bool isRewardVideo, string json)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return;
			}
			if (dictionary.ContainsKey("converted"))
			{
				if (dictionary["converted"].ToString() == "1")
				{
					converted = true;
				}
				else
				{
					converted = false;
				}
			}
			if (dictionary.ContainsKey("rewardName"))
			{
				rewardName = dictionary["rewardName"].ToString();
			}
			if (dictionary.ContainsKey("rewardAmount"))
			{
				rewardAmount = float.Parse(dictionary["rewardAmount"].ToString());
			}
		}

		public MTGRewardData(Dictionary<string, object> rewardDict)
		{
			converted = true;
			if (rewardDict.ContainsKey("rewardName"))
			{
				rewardName = rewardDict["rewardName"].ToString();
			}
			if (rewardDict.ContainsKey("rewardAmount"))
			{
				rewardAmount = float.Parse(rewardDict["rewardAmount"].ToString());
			}
		}

		public override string ToString()
		{
			if (converted)
			{
				return string.Format("rewardName: {0}, rewardAmount: {1}", rewardName, rewardAmount);
			}
			return "convert false,reward  is null";
		}
	}

	public static event Action<string> onInterActiveLoadedEvent;

	public static event Action<string> onInterActiveFailedEvent;

	public static event Action<string> onInterActiveShownEvent;

	public static event Action<string> onInterActiveShownFailedEvent;

	public static event Action<string> onInterActiveClickedEvent;

	public static event Action<string> onInterActiveDismissedEvent;

	public static event Action<string> onInterstitialVideoLoadedEvent;

	public static event Action<string> onInterstitialVideoFailedEvent;

	public static event Action<string> onInterstitialVideoShownEvent;

	public static event Action<string> onInterstitialVideoShownFailedEvent;

	public static event Action<string> onInterstitialVideoClickedEvent;

	public static event Action<string> onInterstitialVideoDismissedEvent;

	public static event Action onInterstitialLoadedEvent;

	public static event Action<string> onInterstitialFailedEvent;

	public static event Action onInterstitialShownEvent;

	public static event Action<string> onInterstitialShownFailedEvent;

	public static event Action onInterstitialClickedEvent;

	public static event Action onInterstitialDismissedEvent;

	public static event Action<string> onRewardedVideoLoadedEvent;

	public static event Action<string> onRewardedVideoFailedEvent;

	public static event Action onRewardedVideoShownEvent;

	public static event Action<string> onRewardedVideoShownFailedEvent;

	public static event Action<string> onRewardedVideoClickedEvent;

	public static event Action<MTGRewardData> onRewardedVideoClosedEvent;

	public static event Action onOfferWallLoadedEvent;

	public static event Action<string> onOfferWallFailedEvent;

	public static event Action onOfferWallDidClickEvent;

	public static event Action onOfferWallShownEvent;

	public static event Action<string> onOfferWallShownFailedEvent;

	public static event Action onOfferWallClosedEvent;

	public static event Action<MTGRewardData[]> onOfferWallEarnedImmediatelyEvent;

	public static event Action<MTGRewardData[]> onOfferWallNotifyCreditsEarnedAfterQueryEvent;

	public static event Action<string> onNativeLoadedEvent;

	public static event Action<string> onNativeFailedEvent;

	public static event Action<string> onNativeDidClickEvent;

	public static event Action<string> onNativeLoggingImpressionEvent;

	public static event Action<string> onNativeRedirectionStartEvent;

	public static event Action<string> onNativeRedirectionFinishedEvent;

	public static event Action<string> onShowUserInfoTipsEvent;

	static MintegralManager()
	{
		Type typeFromHandle = typeof(MintegralManager);
		try
		{
			MonoBehaviour monoBehaviour = UnityEngine.Object.FindObjectOfType(typeFromHandle) as MonoBehaviour;
			if (!(monoBehaviour != null))
			{
				GameObject gameObject = new GameObject(typeFromHandle.ToString());
				gameObject.AddComponent(typeFromHandle);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}
		catch (UnityException)
		{
			Debug.LogWarning(string.Concat("It looks like you have the ", typeFromHandle, " on a GameObject in your scene. Please remove the script from your scene."));
		}
	}

	private void onInterActiveLoaded(string json)
	{
		if (MintegralManager.onInterActiveLoadedEvent != null)
		{
			MintegralManager.onInterActiveLoadedEvent(json);
		}
	}

	private void onInterActiveFailed(string errorMsg)
	{
		if (MintegralManager.onInterActiveFailedEvent != null)
		{
			MintegralManager.onInterActiveFailedEvent(errorMsg);
		}
	}

	private void onInterActiveShown(string json)
	{
		if (MintegralManager.onInterActiveShownEvent != null)
		{
			MintegralManager.onInterActiveShownEvent(json);
		}
	}

	private void onInterActiveShownFailed(string errorMsg)
	{
		if (MintegralManager.onInterActiveShownFailedEvent != null)
		{
			MintegralManager.onInterActiveShownFailedEvent(errorMsg);
		}
	}

	private void onInterActiveClicked(string json)
	{
		if (MintegralManager.onInterActiveClickedEvent != null)
		{
			MintegralManager.onInterActiveClickedEvent(json);
		}
	}

	private void onInterActiveDismissed(string json)
	{
		if (MintegralManager.onInterActiveDismissedEvent != null)
		{
			MintegralManager.onInterActiveDismissedEvent(json);
		}
	}

	private void onInterstitialVideoLoaded(string json)
	{
		if (MintegralManager.onInterstitialVideoLoadedEvent != null)
		{
			MintegralManager.onInterstitialVideoLoadedEvent(json);
		}
	}

	private void onInterstitialVideoFailed(string errorMsg)
	{
		if (MintegralManager.onInterstitialVideoFailedEvent != null)
		{
			MintegralManager.onInterstitialVideoFailedEvent(errorMsg);
		}
	}

	private void onInterstitialVideoShown(string json)
	{
		if (MintegralManager.onInterstitialVideoShownEvent != null)
		{
			MintegralManager.onInterstitialVideoShownEvent(json);
		}
	}

	private void onInterstitialVideoShownFailed(string errorMsg)
	{
		if (MintegralManager.onInterstitialVideoShownFailedEvent != null)
		{
			MintegralManager.onInterstitialVideoShownFailedEvent(errorMsg);
		}
	}

	private void onInterstitialVideoClicked(string json)
	{
		if (MintegralManager.onInterstitialVideoClickedEvent != null)
		{
			MintegralManager.onInterstitialVideoClickedEvent(json);
		}
	}

	private void onInterstitialVideoDismissed(string json)
	{
		if (MintegralManager.onInterstitialVideoDismissedEvent != null)
		{
			MintegralManager.onInterstitialVideoDismissedEvent(json);
		}
	}

	private void onInterstitialLoaded(string json)
	{
		if (MintegralManager.onInterstitialLoadedEvent != null)
		{
			MintegralManager.onInterstitialLoadedEvent();
		}
	}

	private void onInterstitialFailed(string errorMsg)
	{
		if (MintegralManager.onInterstitialFailedEvent != null)
		{
			MintegralManager.onInterstitialFailedEvent(errorMsg);
		}
	}

	private void onInterstitialShown(string json)
	{
		if (MintegralManager.onInterstitialShownEvent != null)
		{
			MintegralManager.onInterstitialShownEvent();
		}
	}

	private void onInterstitialShownFailed(string errorMsg)
	{
		if (MintegralManager.onInterstitialShownFailedEvent != null)
		{
			MintegralManager.onInterstitialShownFailedEvent(errorMsg);
		}
	}

	private void onInterstitialClicked(string json)
	{
		if (MintegralManager.onInterstitialClickedEvent != null)
		{
			MintegralManager.onInterstitialClickedEvent();
		}
	}

	private void onInterstitialDismissed(string json)
	{
		if (MintegralManager.onInterstitialDismissedEvent != null)
		{
			MintegralManager.onInterstitialDismissedEvent();
		}
	}

	private void onRewardedVideoLoaded(string json)
	{
		if (MintegralManager.onRewardedVideoLoadedEvent != null)
		{
			MintegralManager.onRewardedVideoLoadedEvent(json);
		}
	}

	private void onRewardedVideoFailed(string errorMsg)
	{
		if (MintegralManager.onRewardedVideoFailedEvent != null)
		{
			MintegralManager.onRewardedVideoFailedEvent(errorMsg);
		}
	}

	private void onRewardedVideoShown(string json)
	{
		if (MintegralManager.onRewardedVideoShownEvent != null)
		{
			MintegralManager.onRewardedVideoShownEvent();
		}
	}

	private void onRewardedVideoShownFailed(string errorMsg)
	{
		if (MintegralManager.onRewardedVideoShownFailedEvent != null)
		{
			MintegralManager.onRewardedVideoShownFailedEvent(errorMsg);
		}
	}

	private void onRewardedVideoClosed(string json)
	{
		if (MintegralManager.onRewardedVideoClosedEvent != null)
		{
			MintegralManager.onRewardedVideoClosedEvent(new MTGRewardData(true, json));
		}
	}

	private void onRewardedVideoClicked(string json)
	{
		if (MintegralManager.onRewardedVideoClickedEvent != null)
		{
			MintegralManager.onRewardedVideoClickedEvent(json);
		}
	}

	private void onOfferWallLoaded(string json)
	{
		if (MintegralManager.onOfferWallLoadedEvent != null)
		{
			MintegralManager.onOfferWallLoadedEvent();
		}
	}

	private void onOfferWallFailed(string errorMsg)
	{
		if (MintegralManager.onOfferWallFailedEvent != null)
		{
			MintegralManager.onOfferWallFailedEvent(errorMsg);
		}
	}

	private void onOfferWallDidClick(string json)
	{
		if (MintegralManager.onOfferWallDidClickEvent != null)
		{
			MintegralManager.onOfferWallDidClickEvent();
		}
	}

	private void onOfferWallShown(string json)
	{
		if (MintegralManager.onOfferWallShownEvent != null)
		{
			MintegralManager.onOfferWallShownEvent();
		}
	}

	private void onOfferWallShownFailed(string errorMsg)
	{
		if (MintegralManager.onOfferWallShownFailedEvent != null)
		{
			MintegralManager.onOfferWallShownFailedEvent(errorMsg);
		}
	}

	private void onOfferWallClosed(string json)
	{
		if (MintegralManager.onOfferWallClosedEvent != null)
		{
			MintegralManager.onOfferWallClosedEvent();
		}
	}

	private void onOfferWallEarnedImmediately(string json)
	{
		if (MintegralManager.onOfferWallEarnedImmediatelyEvent == null)
		{
			return;
		}
		if (json != null)
		{
			List<object> list = Json.Deserialize(json) as List<object>;
			MTGRewardData[] array = new MTGRewardData[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				if (dictionary != null)
				{
					MTGRewardData mTGRewardData = new MTGRewardData(dictionary);
					array[i] = mTGRewardData;
				}
			}
			MintegralManager.onOfferWallEarnedImmediatelyEvent(array);
		}
		else
		{
			MintegralManager.onOfferWallEarnedImmediatelyEvent(null);
		}
	}

	private void onOfferWallNotifyCreditsEarnedAfterQuery(string json)
	{
		if (MintegralManager.onOfferWallNotifyCreditsEarnedAfterQueryEvent == null)
		{
			return;
		}
		if (json != null)
		{
			List<object> list = Json.Deserialize(json) as List<object>;
			MTGRewardData[] array = new MTGRewardData[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
				if (dictionary != null)
				{
					MTGRewardData mTGRewardData = new MTGRewardData(dictionary);
					array[i] = mTGRewardData;
				}
			}
			MintegralManager.onOfferWallNotifyCreditsEarnedAfterQueryEvent(array);
		}
		else
		{
			MintegralManager.onOfferWallNotifyCreditsEarnedAfterQueryEvent(null);
		}
	}

	private void onNativeLoaded(string json)
	{
		if (MintegralManager.onNativeLoadedEvent != null)
		{
			MintegralManager.onNativeLoadedEvent(json);
		}
	}

	private void onNativeFailed(string errorMsg)
	{
		if (MintegralManager.onNativeFailedEvent != null)
		{
			MintegralManager.onNativeFailedEvent(errorMsg);
		}
	}

	private void onNativeDidClick(string json)
	{
		if (MintegralManager.onNativeDidClickEvent != null)
		{
			MintegralManager.onNativeDidClickEvent(json);
		}
	}

	private void onNativeLoggingImpression(string json)
	{
		if (MintegralManager.onNativeLoggingImpressionEvent != null)
		{
			MintegralManager.onNativeLoggingImpressionEvent(json);
		}
	}

	private void onNativeRedirectionStart(string json)
	{
		if (MintegralManager.onNativeRedirectionStartEvent != null)
		{
			MintegralManager.onNativeRedirectionStartEvent(json);
		}
	}

	private void onNativeRedirectionFinished(string json)
	{
		if (MintegralManager.onNativeRedirectionFinishedEvent != null)
		{
			MintegralManager.onNativeRedirectionFinishedEvent(json);
		}
	}

	private void onShowUserInfoTips(string json)
	{
		if (MintegralManager.onShowUserInfoTipsEvent != null)
		{
			MintegralManager.onShowUserInfoTipsEvent(json);
		}
	}
}
