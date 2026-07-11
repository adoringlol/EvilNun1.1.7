using UnityEngine;

public class MintegralEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		MintegralManager.onInterActiveLoadedEvent += onInterActiveLoadedEvent;
		MintegralManager.onInterActiveFailedEvent += onInterActiveFailedEvent;
		MintegralManager.onInterActiveShownEvent += onInterActiveShownEvent;
		MintegralManager.onInterActiveShownFailedEvent += onInterActiveShownFailedEvent;
		MintegralManager.onInterActiveClickedEvent += onInterActiveClickedEvent;
		MintegralManager.onInterActiveDismissedEvent += onInterActiveDismissedEvent;
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
		MintegralManager.onRewardedVideoLoadedEvent += onRewardedVideoLoadedEvent;
		MintegralManager.onRewardedVideoFailedEvent += onRewardedVideoFailedEvent;
		MintegralManager.onRewardedVideoShownFailedEvent += onRewardedVideoShownFailedEvent;
		MintegralManager.onRewardedVideoShownEvent += onRewardedVideoShownEvent;
		MintegralManager.onRewardedVideoClickedEvent += onRewardedVideoClickedEvent;
		MintegralManager.onRewardedVideoClosedEvent += onRewardedVideoClosedEvent;
		MintegralManager.onOfferWallLoadedEvent += onOfferWallLoadedEvent;
		MintegralManager.onOfferWallFailedEvent += onOfferWallFailedEvent;
		MintegralManager.onOfferWallDidClickEvent += onOfferWallDidClickEvent;
		MintegralManager.onOfferWallShownEvent += onOfferWallShownEvent;
		MintegralManager.onOfferWallShownFailedEvent += onOfferWallShownFailedEvent;
		MintegralManager.onOfferWallClosedEvent += onOfferWallClosedEvent;
		MintegralManager.onOfferWallEarnedImmediatelyEvent += onOfferWallEarnedImmediatelyEvent;
		MintegralManager.onOfferWallNotifyCreditsEarnedAfterQueryEvent += onOfferWallNotifyCreditsEarnedAfterQueryEvent;
		MintegralManager.onNativeLoadedEvent += onNativeLoadedEvent;
		MintegralManager.onNativeFailedEvent += onNativeFailedEvent;
		MintegralManager.onNativeDidClickEvent += onNativeDidClickEvent;
		MintegralManager.onNativeLoggingImpressionEvent += onNativeLoggingImpressionEvent;
		MintegralManager.onNativeRedirectionStartEvent += onNativeRedirectionStartEvent;
		MintegralManager.onNativeRedirectionFinishedEvent += onNativeRedirectionFinishedEvent;
		MintegralManager.onShowUserInfoTipsEvent += onShowUserInfoTipsEvent;
	}

	private void OnDisable()
	{
		MintegralManager.onInterActiveLoadedEvent -= onInterActiveLoadedEvent;
		MintegralManager.onInterActiveFailedEvent -= onInterActiveFailedEvent;
		MintegralManager.onInterActiveShownEvent -= onInterActiveShownEvent;
		MintegralManager.onInterActiveShownFailedEvent -= onInterActiveShownFailedEvent;
		MintegralManager.onInterActiveClickedEvent -= onInterActiveClickedEvent;
		MintegralManager.onInterActiveDismissedEvent -= onInterActiveDismissedEvent;
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
		MintegralManager.onRewardedVideoLoadedEvent -= onRewardedVideoLoadedEvent;
		MintegralManager.onRewardedVideoFailedEvent -= onRewardedVideoFailedEvent;
		MintegralManager.onRewardedVideoShownFailedEvent -= onRewardedVideoShownFailedEvent;
		MintegralManager.onRewardedVideoShownEvent -= onRewardedVideoShownEvent;
		MintegralManager.onRewardedVideoClickedEvent -= onRewardedVideoClickedEvent;
		MintegralManager.onRewardedVideoClosedEvent -= onRewardedVideoClosedEvent;
		MintegralManager.onOfferWallLoadedEvent -= onOfferWallLoadedEvent;
		MintegralManager.onOfferWallFailedEvent -= onOfferWallFailedEvent;
		MintegralManager.onOfferWallDidClickEvent -= onOfferWallDidClickEvent;
		MintegralManager.onOfferWallShownEvent -= onOfferWallShownEvent;
		MintegralManager.onOfferWallShownFailedEvent -= onOfferWallShownFailedEvent;
		MintegralManager.onOfferWallClosedEvent -= onOfferWallClosedEvent;
		MintegralManager.onOfferWallEarnedImmediatelyEvent -= onOfferWallEarnedImmediatelyEvent;
		MintegralManager.onOfferWallNotifyCreditsEarnedAfterQueryEvent -= onOfferWallNotifyCreditsEarnedAfterQueryEvent;
		MintegralManager.onNativeLoadedEvent -= onNativeLoadedEvent;
		MintegralManager.onNativeFailedEvent -= onNativeFailedEvent;
		MintegralManager.onNativeDidClickEvent -= onNativeDidClickEvent;
		MintegralManager.onNativeLoggingImpressionEvent -= onNativeLoggingImpressionEvent;
		MintegralManager.onNativeRedirectionStartEvent -= onNativeRedirectionStartEvent;
		MintegralManager.onNativeRedirectionFinishedEvent -= onNativeRedirectionFinishedEvent;
		MintegralManager.onShowUserInfoTipsEvent -= onShowUserInfoTipsEvent;
	}

	private void mtgLog(string log)
	{
		Debug.LogError("Mintegra: " + log + "\n------------------------------");
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
	}

	private void onInterstitialVideoFailedEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoFailedEvent: " + errorMsg);
	}

	private void onInterstitialVideoShownEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoShownEvent: " + errorMsg);
	}

	private void onInterstitialVideoShownFailedEvent(string adUnitId)
	{
		mtgLog("onInterstitialVideoShownFailedEvent: " + adUnitId);
	}

	private void onInterstitialVideoClickedEvent(string adUnitId)
	{
		mtgLog("onInterstitialVideoClickedEvent: " + adUnitId);
	}

	private void onInterstitialVideoDismissedEvent(string errorMsg)
	{
		mtgLog("onInterstitialVideoDismissedEvent: " + errorMsg);
	}

	private void onInterstitialLoadedEvent()
	{
		mtgLog("onInterstitialLoadedEvent");
	}

	private void onInterstitialFailedEvent(string errorMsg)
	{
		mtgLog("onInterstitialFailedEvent: " + errorMsg);
	}

	private void onInterstitialShownEvent()
	{
		mtgLog("onInterstitialShownEvent");
	}

	private void onInterstitialShownFailedEvent(string adUnitId)
	{
		mtgLog("onInterstitialShownFailedEvent: " + adUnitId);
	}

	private void onInterstitialClickedEvent()
	{
		mtgLog("onInterstitialClickedEvent");
	}

	private void onInterstitialDismissedEvent()
	{
		mtgLog("onInterstitialDismissedEvent");
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

	private void onOfferWallLoadedEvent()
	{
		mtgLog("onOfferWallLoadedEvent");
	}

	private void onOfferWallFailedEvent(string errorMsg)
	{
		mtgLog("onOfferWallFailedEvent: " + errorMsg);
	}

	private void onOfferWallDidClickEvent()
	{
		mtgLog("onOfferWallDidClickEvent");
	}

	private void onOfferWallShownEvent()
	{
		mtgLog("onOfferWallShownEvent");
	}

	private void onOfferWallShownFailedEvent(string errorMsg)
	{
		mtgLog("onOfferWallShownFailedEvent: " + errorMsg);
	}

	private void onOfferWallClosedEvent()
	{
		mtgLog("onOfferWallClosedEvent");
	}

	private void onOfferWallEarnedImmediatelyEvent(MintegralManager.MTGRewardData[] rewardDatas)
	{
		mtgLog("onOfferWallEarnedImmediatelyEvent: ");
		foreach (MintegralManager.MTGRewardData mTGRewardData in rewardDatas)
		{
			mtgLog("OfferWall RewardData: " + mTGRewardData.ToString());
		}
	}

	private void onOfferWallNotifyCreditsEarnedAfterQueryEvent(MintegralManager.MTGRewardData[] rewardDatas)
	{
		mtgLog("onOfferWallNotifyCreditsEarnedAfterQueryEvent: ");
		foreach (MintegralManager.MTGRewardData mTGRewardData in rewardDatas)
		{
			mtgLog("OfferWall RewardData: " + mTGRewardData.ToString());
		}
	}

	private void onNativeLoadedEvent(string msg)
	{
		mtgLog("onNativeLoadedEvent: " + msg);
	}

	private void onNativeFailedEvent(string msg)
	{
		mtgLog("onNativeFailedEvent: " + msg);
	}

	private void onNativeDidClickEvent(string msg)
	{
		mtgLog("onNativeDidClickEvent: " + msg);
	}

	private void onNativeLoggingImpressionEvent(string msg)
	{
		mtgLog("onNativeLoggingImpressionEvent: " + msg);
	}

	private void onNativeRedirectionStartEvent(string msg)
	{
		mtgLog("onNativeRedirectionStartEvent: " + msg);
	}

	private void onNativeRedirectionFinishedEvent(string msg)
	{
		mtgLog("onNativeRedirectionFinishedEvent: " + msg);
	}

	private void onShowUserInfoTipsEvent(string msg)
	{
		mtgLog("onShowUserInfoTipsEvent: " + msg);
	}
}
