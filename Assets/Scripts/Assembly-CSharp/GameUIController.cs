using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
	public static GameUIController instance;

	public GameObject newDayPanel;

	public GameObject gameOverPanel;

	public GameObject gamecompletePanel;

	public Joystick j;

	public TextMeshProUGUI txt;

	public GameObject blackScreen;

	public GameObject exitTxt;

	public GameObject gameOver_menuButton;

	public GameObject gameComplete_menuButton;

	public bool gameOver;

	public bool completed;

	public Slider slider;

	public GameObject actionButton;

	public GameObject centerPoint;

	public Sprite standupSprite;

	public Sprite crawlSprite;

	public GameObject releaseButton;

	public GameObject fastHiddingUI;

	public GameObject standupButton;

	public GameObject pauseButton;

	public GameObject bloodDecals;

	public GameObject joystickUI;

	public bool useJoystick;

	public GameObject interactionEnabledSprite;

	public TextMeshProUGUI donateText;

	public TextMeshProUGUI hideButtonTxt;

	public GameObject startDollButton;

	public GameObject shootButton;

	public TextMeshProUGUI countdownTxt;

	public int currentCountDown = 10;

	public int dollCountDown = 60;

	public GameObject extraLifeWindow;

	public bool watchedVideoToContinue;

	public GameObject rateWindow;

	private void Awake()
	{
		instance = this;
		// A paused time scale can leak into this scene in exported projects.
		// ShowDayText is invoked using scaled time and is responsible for enabling
		// the player and camera, so it must be allowed to run.
		Time.timeScale = 1f;
		StartCoroutine(EnsureGameplayControls());
		useJoystick = false;
		useJoystick = true;
		if (!useJoystick)
		{
			joystickUI.SetActive(false);
		}
	}

	private void Start()
	{
		completed = false;
		gameOver = false;
		if (newDayPanel != null)
		{
			newDayPanel.SetActive(true);
			Image dayImage = newDayPanel.GetComponent<Image>();
			if (dayImage != null)
			{
				dayImage.raycastTarget = true;
			}
		}
		if (txt != null)
		{
			txt.text = string.Empty;
		}
		Invoke("ShowDayText", 1f);
		if (slider != null)
		{
			slider.value = PlayerPrefs.GetFloat("sensivityFactor", 10f);
		}
		if (SimpleSmoothMouseLook.instance != null)
		{
			SimpleSmoothMouseLook.instance.ChangeCameraSensivity(PlayerPrefs.GetFloat("sensivityFactor", 10f));
		}
		if (AnalyticsController.instance != null)
		{
			AnalyticsController.instance.OnStartGame();
		}
		StartMusic();
	}

	private IEnumerator EnsureGameplayControls()
	{
		// Do not depend on Time.timeScale or the day-title animation to make the
		// reconstructed player controllable.
		yield return new WaitForSecondsRealtime(1.25f);
		if (PlayersManager.instance == null)
		{
			yield break;
		}

		MyController player = PlayersManager.instance.GetCurrentController();
		if (player == null)
		{
			yield break;
		}

		if (player.controller == null)
		{
			player.controller = player.GetComponent<CharacterController>();
		}
		if (player.movement == null)
		{
			player.movement = player.GetComponent<PlayerMovement>();
		}
		if (player.controller != null)
		{
			player.controller.enabled = true;
		}
		if (player is PlayerController)
		{
			// StartControlling can be interrupted by missing optional UI references
			// before it restores the normal standing capsule and eye position.
			player.standup = true;
			if (player.controller != null)
			{
				player.controller.center = Vector3.zero;
				player.controller.height = 1.35f;
			}
			if (player.eyesTrans != null)
			{
				player.eyesTrans.localPosition = new Vector3(0f, 0.6f, 0f);
			}
			if (player.feetPosition != null)
			{
				player.feetPosition.localPosition = new Vector3(0f, -0.6f, 0f);
			}
		}
		if (player.movement != null)
		{
			player.movement.enabled = false;
		}
		player.blocked = false;
		if (player.GetComponent<ExportedMovementDriver>() == null)
		{
			player.gameObject.AddComponent<ExportedMovementDriver>();
		}

		if (SimpleSmoothMouseLook.instance != null)
		{
			SimpleSmoothMouseLook.instance.canLook = true;
		}
	}

	public void StartMusic()
	{
	}

	public void ShowGameCompletePanel()
	{
		completed = true;
		AnalyticsEvent.Custom("game_completed", new Dictionary<string, object> { 
		{
			"days",
			ProgressManager.instance.day
		} });
		gamecompletePanel.SetActive(true);
		gamecompletePanel.GetComponent<Image>().DOFade(1f, 1f).OnComplete(ShowFinishText);
	}

	public void ShowFinishText()
	{
		exitTxt.SetActive(true);
		gameComplete_menuButton.SetActive(true);
		Invoke("ShowRateWindow", 3f);
	}

	public void ShowRateWindow()
	{
		if (!PlayerPrefs.HasKey("click_rate"))
		{
			rateWindow.SetActive(true);
		}
	}

	public void ShowGameOver()
	{
		gameOverPanel.SetActive(true);
		gameOverPanel.GetComponent<Image>().DOFade(1f, 1f);
		MusicManager.instance.OnPaused();
		AdsManager.instance.CheckAd();
	}

	public void ShowNewDayPanel()
	{
		txt.text = string.Empty;
		j.Reset();
		if (ProgressManager.instance.day >= 5)
		{
			AnalyticsController.instance.OnGameOver();
			PlayerPrefs.SetInt("gameover", PlayerPrefs.GetInt("gameover", 0) + 1);
			gameOver = true;
			CinematicsManager.instance.StartGameOverCinematic();
		}
		else
		{
			ProgressManager.instance.day++;
			AnalyticsController.instance.OnDied();
			newDayPanel.SetActive(true);
			newDayPanel.GetComponent<Image>().raycastTarget = true;
			newDayPanel.GetComponent<Image>().DOFade(1f, 0.5f).OnComplete(OnStartNewDay);
			CarboardController.instance.ConfigCardboard(ProgressManager.instance.day);
			Invoke("ShowDayText", 3f);
		}
	}

	public void OnStartNewDay()
	{
		AdsManager.instance.CheckAd();
	}

	public void HideBlackScreen()
	{
		blackScreen.SetActive(false);
	}

	public void OnResetGame()
	{
		PlayersManager.instance.GetCurrentController().LeaveHide();
		if (!VariablesGlobales.removeAdsBought && !watchedVideoToContinue && AdsManager.instance.isRewardedAdReady && ProgressManager.instance.day == 5)
		{
			extraLifeWindow.SetActive(true);
		}
		else
		{
			ShowNewDayPanel();
		}
	}

	public void ShowDayText()
	{
		if (ProgressManager.instance.day > 0)
		{
			AnalyticsEvent.Custom("on_died");
		}
		// Player startup must not depend on optional enemy state restored from an
		// exported scene. Start controls first, then reset the nun separately.
		if (PlayersManager.instance != null)
		{
			PlayersManager.instance.StartChild();
		}
		if (ZombieBehaviour.instance != null)
		{
			try
			{
				ZombieBehaviour.instance.ResetNun();
			}
			catch (System.NullReferenceException exception)
			{
				Debug.LogWarning("Nun reset skipped because an exported reference is missing: " + exception.Message);
			}
		}
		if (ProgressManager.instance.day < 5)
		{
			txt.GetComponent<Localize>().SetTerm("day");
			txt.text = txt.text.Replace("{0}", ProgressManager.instance.day.ToString());
		}
		else
		{
			txt.GetComponent<Localize>().SetTerm("last_day");
		}
		txt.DOFade(1f, 5f);
		MasterAudio.PlaySound("day_sound");
		newDayPanel.GetComponent<Image>().DOFade(0f, 0.5f).SetDelay(3f);
		Invoke("DisableRayCast", 1f);
		txt.DOFade(0f, 1f).SetDelay(3.5f).OnComplete(FinishHidingDayPanel);
		AdsManager.instance.FetchAd();
	}

	public void DisableRayCast()
	{
		newDayPanel.GetComponent<Image>().raycastTarget = false;
	}

	public void FinishHidingDayPanel()
	{
		newDayPanel.SetActive(false);
	}

	public void OnClickMenu()
	{
		MasterAudio.PlaySound("button");
		MusicManager.instance.OnContinue();
		j.UnRegisterAxis();
		GameManager.instance.OnLoadScene(1);
	}

	public void OnSensivityChanged()
	{
		if (SimpleSmoothMouseLook.instance != null && slider != null)
		{
			SimpleSmoothMouseLook.instance.ChangeCameraSensivity(slider.value);
		}
	}

	public void OnClickUseDoll()
	{
		if (PlayersManager.instance.GetCurrentController().isHidden)
		{
			PlayersManager.instance.child.objectOnHand.transform.parent = null;
			PlayersManager.instance.child.hasObjectOnHand = false;
			PlayersManager.instance.child.objectOnHand.gameObject.SetActive(false);
			instance.startDollButton.SetActive(false);
			PlayersManager.instance.StartBaby();
		}
		else
		{
			MessagesManager.instance.ShowMessage("must_be_hidden");
		}
	}

	public void OnClickShoot()
	{
		if (PlayersManager.instance.child.objectOnHand.id == 333)
		{
			((GumgunTakeable)PlayersManager.instance.child.objectOnHand).Shoot();
		}
	}

	public void OnClickRelease()
	{
		if (!PlayersManager.instance.GetCurrentController().blocked && !PlayersManager.instance.isBaby && PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.OnReleaseItem();
		}
	}

	public void OnClickAction()
	{
		if (PlayersManager.instance.GetCurrentController().blocked)
		{
			return;
		}
		if (TutorialController.instance.isEnabled)
		{
			TutorialController.instance.OnInteractFinished();
		}
		actionButton.SetActive(false);
		if (PlayersManager.instance.GetCurrentController().current.GetType() == typeof(OpenCloseBehaviour) && ((OpenCloseBehaviour)PlayersManager.instance.GetCurrentController().current).multipleDoors.Length > 0)
		{
			TouchableElement[] multipleDoors = ((OpenCloseBehaviour)PlayersManager.instance.GetCurrentController().current).multipleDoors;
			for (int i = 0; i < multipleDoors.Length; i++)
			{
				OpenCloseBehaviour openCloseBehaviour = (OpenCloseBehaviour)multipleDoors[i];
				openCloseBehaviour.Touched();
			}
		}
		PlayersManager.instance.GetCurrentController().current.Touched();
		PlayersManager.instance.GetCurrentController().canCheckAction = false;
		PlayersManager.instance.GetCurrentController().Invoke("CanCheckAction", 0.5f);
	}

	public void OnClickStandUp()
	{
		if (!PlayersManager.instance.GetCurrentController().blocked && (PlayersManager.instance.GetCurrentController().standup || PlayersManager.instance.GetCurrentController().CanStandup()))
		{
			MasterAudio.PlaySound("hide");
			PlayersManager.instance.GetCurrentController().standup = !PlayersManager.instance.GetCurrentController().standup;
			if (standupButton != null)
			{
				Image standupImage = standupButton.GetComponent<Image>();
				if (standupImage != null) standupImage.sprite = ((!PlayersManager.instance.GetCurrentController().standup) ? crawlSprite : standupSprite);
			}
			if (TutorialController.instance != null && TutorialController.instance.isEnabled)
			{
				TutorialController.instance.OnCrouchControlFinished();
			}
			PlayersManager.instance.GetCurrentController().UpdateStandUp();
			UpdateStandupSprite();
		}
	}

	public void UpdateStandupSprite()
	{
		standupButton.GetComponent<Image>().sprite = ((!PlayersManager.instance.GetCurrentController().standup) ? crawlSprite : standupSprite);
	}

	public void HideFastHiddingUI()
	{
		fastHiddingUI.gameObject.SetActive(false);
	}

	public void ShowFastHiddingUI()
	{
		if (!PlayersManager.instance.child.blocked)
		{
			fastHiddingUI.gameObject.SetActive(true);
			if (PlayersManager.instance.GetCurrentController().isHidden)
			{
				hideButtonTxt.text = LocalizationManager.GetTranslation("unhide");
			}
			else
			{
				hideButtonTxt.text = LocalizationManager.GetTranslation("hide");
			}
		}
	}

	public void SetBloodInCamera(bool s)
	{
		if (VariablesGlobales.bloodEnabled && bloodDecals != null)
		{
			bloodDecals.SetActive(s);
			CanvasGroup canvasGroup = bloodDecals.GetComponent<CanvasGroup>();
			if (canvasGroup != null)
			{
				canvasGroup.alpha = 0f;
				canvasGroup.DOFade(1f, 0.3f);
			}
		}
	}

	public void OnClickFastHidding()
	{
		if (PlayersManager.instance.GetCurrentController().blocked)
		{
			return;
		}
		if (ZombieBehaviour.instance != null && ZombieBehaviour.instance.state == ZombieBehaviour.NunState.ATTACK)
		{
			HideFastHiddingUI();
			return;
		}
		if (PlayersManager.instance.GetCurrentController().isHidden)
		{
			((FastHide)PlayersManager.instance.GetCurrentController().currentHide).OnExitHide();
			if (instance.useJoystick)
			{
				joystickUI.SetActive(true);
			}
			hideButtonTxt.text = LocalizationManager.GetTranslation("hide");
			MasterAudio.PlaySound("unhide");
			return;
		}
		releaseButton.SetActive(false);
		PlayersManager.instance.GetCurrentController().OnFinishPersecution();
		((FastHide)PlayersManager.instance.GetCurrentController().currentHide).OnHide();
		if (instance.useJoystick)
		{
			joystickUI.SetActive(false);
		}
		MasterAudio.PlaySound("hide");
		hideButtonTxt.text = LocalizationManager.GetTranslation("unhide");
	}

	public IEnumerator DollCountDown()
	{
		currentCountDown = dollCountDown;
		countdownTxt.text = currentCountDown.ToString();
		countdownTxt.gameObject.SetActive(true);
		while (currentCountDown > 0)
		{
			yield return new WaitForSeconds(1f);
			currentCountDown--;
			countdownTxt.text = currentCountDown.ToString();
		}
		countdownTxt.gameObject.SetActive(false);
		PlayersManager.instance.ExplodeDoll();
	}

	public void StopCountDown()
	{
		StopCoroutine("DollCountDown");
		countdownTxt.gameObject.SetActive(false);
	}
}
