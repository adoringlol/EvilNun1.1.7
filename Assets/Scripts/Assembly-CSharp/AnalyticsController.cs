using System;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class AnalyticsController : MonoBehaviour
{
	public static AnalyticsController instance;

	private DateTime startTime;

	public const string signature = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8fb+ZsjmFayWbqoaMcxveQkBUkyFmw55mh3pIBqsjrjHRzsbJSwLonHPbbHjPKia9Jh+TMu1ePjge06iRXDFfJgm733BfxOovbVGrWEiub0+d2oBaoyJTWUnuN/c4/l8yofOJi7ETZM2hwhq4ehKEx6i2ofFBD1DDEwNjC03HDiSJ56EGQuQXSTrN7Uwd+Pr1KPRARJA46XFcg61IJDr/9gpjWXAXRLHD4yILgg6l7SOnuPBigsQdCrFo+zmQqbspnVCb8yN6wAFn1CYKKvBP0JNN+YCXhPOZb2ZHP+SUMYLESmbWyRb/oG9+Z1+6237j9sLEuf1Ys6GZ4DPdMDVlwIDAQAB";

	private bool isNewUser;

	private int playsAmount;

	private int solvePuzzlesAmount;

	private int duration
	{
		get
		{
			return Mathf.RoundToInt((float)(UnbiasedTime.Instance.Now() - startTime).TotalSeconds);
		}
	}

	private string GameModeString
	{
		get
		{
			string result = string.Empty;
			if (VariablesGlobales.difficultyMode == 0)
			{
				result = "easy";
			}
			else if (VariablesGlobales.difficultyMode == 1)
			{
				result = "normal";
			}
			else if (VariablesGlobales.difficultyMode == 2)
			{
				result = "hard";
			}
			else if (VariablesGlobales.difficultyMode == 3)
			{
				result = "ghost";
			}
			return result;
		}
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		isNewUser = !PlayerPrefs.HasKey("existingUser");
		PlayerPrefs.SetInt("existingUser", 1);
	}

	private void Start()
	{
		Debug.Log("Initialize analytics");
		GameAnalytics.Initialize();
		if (isNewUser)
		{
			OnInitializeCompleteGameTimes();
		}
	}

	public void OnMintegralUserInitialized(string country)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "Mintegral", country));
	}

	public void OnFinishGDPR(bool result)
	{
		if (isNewUser)
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "window", "00-gdpr", result));
		}
	}

	public void OnShowMainMenu()
	{
		Debug.Log("On Show Main Menu");
		if (isNewUser)
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "window", "10-main"));
		}
	}

	public void OnShowMatchMenu()
	{
		if (isNewUser)
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "window", "20-match"));
		}
	}

	public void OnStartGame()
	{
		startTime = UnbiasedTime.Instance.Now();
		playsAmount++;
		solvePuzzlesAmount = 0;
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "30-start", "game-mode", GameModeString));
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "30-start", "blood", VariablesGlobales.bloodEnabled));
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "30-start", "tutorial", VariablesGlobales.tutorialEnabled));
		PlayerPrefs.SetInt("matchStartTimes", PlayerPrefs.GetInt("matchStartTimes", 0) + 1);
		string text = string.Empty;
		int num = PlayerPrefs.GetInt("matchStartTimes");
		if (num <= 5)
		{
			text = num.ToString();
		}
		else if (num <= 10)
		{
			text = "5-";
		}
		else if (num <= 20)
		{
			text = "10-";
		}
		else if (num <= 30)
		{
			text = "20-";
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "30-start", "times", text));
	}

	public void OnStartTutorial()
	{
		if (!PlayerPrefs.HasKey("tutorial"))
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "first_tutorial", "40-start"));
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "tutorial", "40-start"));
	}

	public void OnCompleteTutorialStep(int step)
	{
		string arg = string.Empty;
		switch (step)
		{
		case 0:
			arg = "50-camera-control";
			break;
		case 1:
			arg = "60-movement-control";
			break;
		case 2:
			arg = "70-crouch-control";
			break;
		case 3:
			arg = "80-interact-control";
			break;
		}
		if (!PlayerPrefs.HasKey("tutorial"))
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "first-tutorial", "progress", arg));
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "tutorial", "progress", arg));
	}

	public void OnCompleteAllTutorial()
	{
		if (!PlayerPrefs.HasKey("tutorial"))
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "first_tutorial", "90-complete"));
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "tutorial", "90-complete"));
	}

	public void OnLeaveFirstRoom()
	{
		if (!PlayerPrefs.HasKey("tutorial"))
		{
			GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "100-first-leave-start-room", "tutorial", VariablesGlobales.tutorialEnabled));
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "100-leave-start-room", "tutorial", VariablesGlobales.tutorialEnabled));
	}

	public void OnOpenMuseumDoor()
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "95-open-museum-door", "tutorial", VariablesGlobales.tutorialEnabled));
	}

	public void OnDied()
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "game-die", GameModeString));
	}

	public void OnInitializeCompleteGameTimes()
	{
		PlayerPrefs.SetInt("completedTimes", 0);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "500-complete", "count", "0"));
	}

	public void OnCompleteGame(int days)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "500-complete", "mode", GameModeString));
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "500-complete", "days"), days);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "500-complete", "duration"), duration);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "500-complete", "tutorial", VariablesGlobales.tutorialEnabled));
		PlayerPrefs.SetInt("completedTimes", PlayerPrefs.GetInt("completedTimes", 0) + 1);
		string text = string.Empty;
		int num = PlayerPrefs.GetInt("completedTimes");
		if (num <= 5)
		{
			text = num.ToString();
		}
		else if (num <= 10)
		{
			text = "5-";
		}
		else if (num <= 20)
		{
			text = "10-";
		}
		else if (num <= 30)
		{
			text = "20-";
		}
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "500-complete", "count", text));
	}

	public void OnGameOver()
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "490-gameover", "mode", GameModeString));
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "490-gameover", "duration"), duration);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "490-gameover", "tutorial", VariablesGlobales.tutorialEnabled));
	}

	public void OnGameClose(int day)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "480-leave", "day"), day);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "480-leave", "duration"), duration);
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "480-leave", "tutorial", VariablesGlobales.tutorialEnabled));
	}

	public void OnOpenGameFromNotification(string eventName, string placement, int pictures_unlocked)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "notifications", placement, eventName, pictures_unlocked));
	}

	public void OnRemoveAds(Product p)
	{
		string cartType = string.Empty;
		if (SceneManager.GetActiveScene().buildIndex == 1)
		{
			cartType = "game-menu";
		}
		else if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			cartType = "match-menu";
		}
		else if (SceneManager.GetActiveScene().buildIndex == 3)
		{
			cartType = "pause";
		}
		GameAnalytics.NewBusinessEventGooglePlay(p.metadata.isoCurrencyCode, (int)(p.metadata.localizedPrice * 100m), "Remove Ads", p.definition.id, cartType, p.receipt, "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8fb+ZsjmFayWbqoaMcxveQkBUkyFmw55mh3pIBqsjrjHRzsbJSwLonHPbbHjPKia9Jh+TMu1ePjge06iRXDFfJgm733BfxOovbVGrWEiub0+d2oBaoyJTWUnuN/c4/l8yofOJi7ETZM2hwhq4ehKEx6i2ofFBD1DDEwNjC03HDiSJ56EGQuQXSTrN7Uwd+Pr1KPRARJA46XFcg61IJDr/9gpjWXAXRLHD4yILgg6l7SOnuPBigsQdCrFo+zmQqbspnVCb8yN6wAFn1CYKKvBP0JNN+YCXhPOZb2ZHP+SUMYLESmbWyRb/oG9+Z1+6237j9sLEuf1Ys6GZ4DPdMDVlwIDAQAB");
	}

	public void OnStartAd(string type, string network, string placement)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}:{4}", "ads", network, type, placement + "-" + GameModeString, "start"));
	}

	public void OnCompleteAd(string type, string network, string placement)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}:{4}", "ads", network, type, placement + "-" + GameModeString, "complete"));
	}

	public void OnClickAd(string type, string network, string placement)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}:{4}", "ads", network, type, placement + "-" + GameModeString, "click"));
	}

	public void OnClickRateGame(string type)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}", "rate", type));
	}

	public void OnSolvePuzzle(string puzzleName)
	{
		solvePuzzlesAmount++;
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}:{3}", "match", "puzzles", GameModeString, puzzleName));
	}

	public void OnTakeItem(string itemName)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "take-item", itemName));
	}

	public void OnActivateEasterEgg(string easterEgg)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "match", "easter-egg", easterEgg.ToLower()));
	}

	public void OnUnlockNewPicture(int pictureAmount)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "progress", "pictures", pictureAmount.ToString()));
	}

	public void OnNewMaskPiecePlaced(int maskPieceAmount)
	{
		GameAnalytics.NewDesignEvent(string.Format("{0}:{1}:{2}", "progress", "mask-pieces", maskPieceAmount.ToString()));
	}
}
