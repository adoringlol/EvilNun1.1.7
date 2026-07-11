using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
	public static TutorialController instance;

	public int tutorialStep;

	private bool cameraControlFinished;

	private bool moveControlFinished;

	private bool crouchControlFinished;

	private bool interactTutorialStarted;

	private bool interactFinished;

	public bool isEnabled;

	public Image cameraInteractionPanel;

	public Image joystickInteractionImage;

	public GameObject joystickPanel;

	public GameObject crouchButton;

	public GameObject actionButton;

	public GameObject pauseButton;

	public GameObject hands_panel;

	[Header("Mostrar logs")]
	public bool enableLogging;

	private void Awake()
	{
		instance = this;
		isEnabled = VariablesGlobales.tutorialEnabled;
	}

	private void Start()
	{
		tutorialStep = 0;
		if (cameraInteractionPanel == null || joystickPanel == null || joystickInteractionImage == null || crouchButton == null)
		{
			Debug.LogWarning("El tutorial no está bien configurado...");
			isEnabled = false;
		}
		if (isEnabled)
		{
			StartCoroutine("TutorialBehaviour");
		}
	}

	public void TutorialInitialization()
	{
		pauseButton.SetActive(false);
		joystickPanel.SetActive(false);
		cameraInteractionPanel.gameObject.SetActive(false);
		crouchButton.SetActive(false);
		SimpleSmoothMouseLook.instance.canLook = false;
		AnalyticsController.instance.OnStartTutorial();
	}

	public IEnumerator TutorialBehaviour()
	{
		TutorialInitialization();
		yield return new WaitForSeconds(3f);
		hands_panel.SetActive(true);
		hands_panel.transform.DOKill();
		hands_panel.GetComponent<Image>().DOFade(0.85f, 2f);
		hands_panel.GetComponent<Image>().DOFade(0f, 3f).SetDelay(3f)
			.OnComplete(HandsCompleted);
		if (enableLogging)
		{
			Debug.Log("Tutorial started");
		}
		yield return StartCoroutine("TutorialCameraControl_1");
		yield return new WaitForSeconds(4f);
		yield return StartCoroutine("TutorialMoveControl_2");
		yield return new WaitForSeconds(4f);
		yield return StartCoroutine("TutorialCrouchControl_3");
		yield return new WaitForSeconds(2f);
		interactFinished = false;
		while (!interactFinished)
		{
			yield return null;
		}
		if (enableLogging)
		{
			Debug.Log("Tutorial finished");
		}
		AnalyticsController.instance.OnCompleteAllTutorial();
		PlayerPrefs.SetInt("tutorial", 0);
		isEnabled = false;
		pauseButton.SetActive(true);
	}

	public void HandsCompleted()
	{
		hands_panel.transform.DOKill();
		hands_panel.SetActive(false);
	}

	public void OnCompleteTutorialStep()
	{
		AnalyticsController.instance.OnCompleteTutorialStep(tutorialStep);
		tutorialStep++;
	}

	public IEnumerator TutorialCameraControl_1()
	{
		MessagesManager.instance.ShowMessage("tutorial_0", 10f);
		cameraInteractionPanel.gameObject.SetActive(true);
		if (enableLogging)
		{
			Debug.Log("Camera control start");
		}
		cameraInteractionPanel.DOFade(0.35f, 1f).SetLoops(-1, LoopType.Yoyo);
		yield return new WaitForSeconds(0.1f);
		SimpleSmoothMouseLook.instance.canLook = true;
		while (!cameraControlFinished)
		{
			yield return null;
		}
		cameraInteractionPanel.DOKill();
		yield return cameraInteractionPanel.DOFade(0f, 0.25f).WaitForCompletion();
		cameraInteractionPanel.gameObject.SetActive(false);
		OnCompleteTutorialStep();
		yield return new WaitForSeconds(0.5f);
		MessagesManager.instance.ShowMessage("tutorial_1", 10f);
		if (enableLogging)
		{
			Debug.Log("Camera control end");
		}
	}

	public void OnCameraControlFinished()
	{
		cameraControlFinished = true;
	}

	public IEnumerator TutorialMoveControl_2()
	{
		MessagesManager.instance.ShowMessage("tutorial_2", 10f);
		joystickPanel.SetActive(true);
		if (enableLogging)
		{
			Debug.Log("Move control start");
		}
		joystickInteractionImage.DOFade(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
		while (!moveControlFinished)
		{
			yield return null;
		}
		joystickInteractionImage.DOKill();
		yield return joystickInteractionImage.DOFade(0.85f, 0.25f).WaitForCompletion();
		OnCompleteTutorialStep();
		yield return new WaitForSeconds(0.5f);
		MessagesManager.instance.ShowMessage("tutorial_3", 10f);
		if (enableLogging)
		{
			Debug.Log("Move control end");
		}
	}

	public void OnMoveControlFinished()
	{
		moveControlFinished = true;
	}

	public IEnumerator TutorialCrouchControl_3()
	{
		MessagesManager.instance.ShowMessage("tutorial_4", 10f);
		crouchButton.SetActive(true);
		if (enableLogging)
		{
			Debug.Log("Crouch control start");
		}
		crouchButton.GetComponent<Image>().DOFade(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
		while (!crouchControlFinished)
		{
			yield return null;
		}
		crouchButton.GetComponent<Image>().DOKill();
		yield return crouchButton.GetComponent<Image>().DOFade(1f, 0.25f).WaitForCompletion();
		OnCompleteTutorialStep();
		yield return new WaitForSeconds(0.5f);
		MessagesManager.instance.ShowMessage("tutorial_5", 10f);
		if (enableLogging)
		{
			Debug.Log("Crouch control end");
		}
	}

	public void OnCrouchControlFinished()
	{
		crouchControlFinished = true;
	}

	public void OnInteractStarted()
	{
		if (!interactTutorialStarted)
		{
			StartCoroutine("TutorialInteractControl_4");
		}
	}

	public IEnumerator TutorialInteractControl_4()
	{
		interactTutorialStarted = true;
		if (enableLogging)
		{
			Debug.Log("Interact control start");
		}
		bool waitingToBeActive = true;
		while (!interactFinished)
		{
			if (waitingToBeActive && GameUIController.instance.actionButton.activeSelf)
			{
				MessagesManager.instance.ShowMessage("tutorial_6");
				waitingToBeActive = false;
				Color color = actionButton.GetComponent<Image>().color;
				color.a = 0.85f;
				actionButton.GetComponent<Image>().color = color;
				actionButton.GetComponent<Image>().DOFade(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
			}
			else if (!waitingToBeActive && !GameUIController.instance.actionButton.activeSelf)
			{
				waitingToBeActive = true;
				actionButton.GetComponent<Image>().DOKill();
			}
			yield return null;
		}
		actionButton.GetComponent<Image>().DOKill();
		yield return actionButton.GetComponent<Image>().DOFade(1f, 0.25f).WaitForCompletion();
		OnCompleteTutorialStep();
		yield return new WaitForSeconds(0.5f);
		MessagesManager.instance.ShowMessage("tutorial_7");
		if (enableLogging)
		{
			Debug.Log("Interact tutorial end");
		}
	}

	public void OnInteractFinished()
	{
		interactFinished = true;
	}
}
