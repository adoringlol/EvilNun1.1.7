using System.Collections;
using DarkTonic.MasterAudio;
using I2.Loc;
using UnityEngine;

public class PlayerController : MyController
{
	private float lastTime;

	public GameObject epicLogo;

	public override void DoUpdate()
	{
		if (blocked)
		{
			return;
		}
		Vector3 normalized = Camera.main.transform.forward.normalized;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, normalized, out hitInfo, VariablesGlobales.distanceToInteract, touchableLayer))
		{
			if (hitInfo.collider.gameObject.tag == "Touchable" || hitInfo.collider.gameObject.tag == "Takeable" || hitInfo.collider.gameObject.tag == "Epic")
			{
				if (TutorialController.instance.isEnabled)
				{
					if (TutorialController.instance.tutorialStep < 3)
					{
						return;
					}
					TutorialController.instance.OnInteractStarted();
				}
				if ((bool)hitInfo.collider.transform.GetComponent<TouchableElement>() && hitInfo.collider.transform.GetComponent<TouchableElement>().interactuable)
				{
					current = hitInfo.collider.transform.GetComponent<TouchableElement>();
					hasTouchableInFront = true;
					if (hitInfo.collider.transform.GetComponent<TouchableElement>().specialLocalization)
					{
						touchingItemNameTxt.GetComponent<Localize>().SetTerm(current.actionName);
						touchingItemNameTxt.GetComponent<Localize>().OnLocalize(true);
						touchingItemNameTxt.text = touchingItemNameTxt.text.Replace("{0}", (!VariablesGlobales.ownName) ? VariablesGlobales.randomName : VariablesGlobales.playerOwnName);
					}
					else
					{
						touchingItemNameTxt.GetComponent<Localize>().SetTerm(current.labelName);
						touchingItemNameTxt.GetComponent<Localize>().OnLocalize(true);
					}
					if (!GameUIController.instance.interactionEnabledSprite.activeSelf)
					{
						GameUIController.instance.interactionEnabledSprite.SetActive(true);
					}
					actionNameTxt.GetComponent<Localize>().SetTerm(current.actionName);
					actionNameTxt.GetComponent<Localize>().OnLocalize(true);
				}
				else
				{
					if (hitInfo.collider.gameObject.tag == "Epic" && !epicLogo.activeSelf && ProgressManager.instance.day == 1)
					{
						lastTime += Time.deltaTime;
						if (lastTime > 10f)
						{
							lastTime = 0f;
							epicLogo.SetActive(true);
							MasterAudio.PlaySound("ascensor_button");
						}
					}
					else
					{
						lastTime = 0f;
					}
					hasTouchableInFront = false;
					if (GameUIController.instance.interactionEnabledSprite.activeSelf)
					{
						GameUIController.instance.interactionEnabledSprite.SetActive(false);
					}
				}
			}
			else
			{
				hasTouchableInFront = false;
				if (GameUIController.instance != null && GameUIController.instance.interactionEnabledSprite != null && GameUIController.instance.interactionEnabledSprite.activeSelf)
				{
					GameUIController.instance.interactionEnabledSprite.SetActive(false);
				}
			}
		}
		else
		{
			hasTouchableInFront = false;
			if (GameUIController.instance != null && GameUIController.instance.interactionEnabledSprite != null && GameUIController.instance.interactionEnabledSprite.activeSelf)
			{
				GameUIController.instance.interactionEnabledSprite.SetActive(false);
			}
		}
		if (hasTouchableInFront)
		{
			if (GameUIController.instance != null && GameUIController.instance.actionButton != null && canCheckAction)
			{
				GameUIController.instance.actionButton.SetActive(true);
			}
			else if (GameUIController.instance != null && GameUIController.instance.actionButton != null)
			{
				GameUIController.instance.actionButton.SetActive(false);
			}
		}
		else if (GameUIController.instance != null && GameUIController.instance.actionButton != null)
		{
			GameUIController.instance.actionButton.SetActive(false);
		}
		if (!isHidden || !(currentHide != null) || !(currentHide.door == null))
		{
			if (controller.velocity.magnitude >= 1f && lastVelocity == 0f)
			{
				OnStartWalking();
			}
			else if (lastVelocity >= 1f && controller.velocity.magnitude >= 1f)
			{
				IsWalking();
			}
		}
		if (lastVelocity >= 1f && controller.velocity.magnitude < 1f)
		{
			OnStopWalking();
		}
		lastVelocity = controller.velocity.magnitude;
		base.DoUpdate();
	}

	public override void UpdateStandUp()
	{
		if (standup)
		{
			controller.center = new Vector3(0f, 0f, 0f);
			controller.height = 1.35f;
			eyesTrans.localPosition = new Vector3(0f, 0.6f, 0f);
			cam.transform.localPosition = eyesTrans.transform.localPosition;
			feetPosition.localPosition = new Vector3(0f, -0.6f, 0f);
			base.transform.position += Vector3.up * 0.6f;
		}
		else
		{
			eyesTrans.localPosition = new Vector3(0f, 0.1f, 0f);
			cam.transform.localPosition = eyesTrans.transform.localPosition;
			controller.center = new Vector3(0f, 0f, 0f);
			controller.height = 0.4f;
			feetPosition.localPosition = new Vector3(0f, -0.15f, 0f);
			base.transform.position -= Vector3.up * 0.6f;
		}
	}

	public IEnumerator FootSteps()
	{
		while (true)
		{
			if (controller.isGrounded && controller.velocity.magnitude > 0.5f)
			{
				if (standup)
				{
					MasterAudio.PlaySound3DAtVector3("player_step", base.transform.position - Vector3.up * controller.height / 2f);
					yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));
				}
				else
				{
					MasterAudio.PlaySound3DAtVector3("player_crawl", base.transform.position - Vector3.up * controller.height / 2f);
					yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));
				}
			}
			else
			{
				yield return new WaitForSeconds(0.2f);
			}
		}
	}

	public new void UpdateStandUp(bool set)
	{
		if (set)
		{
			feetPosition.localPosition = new Vector3(0f, -0.6f, 0f);
			controller.center = new Vector3(0f, 0f, 0f);
			controller.height = 1.35f;
			eyesTrans.localPosition = new Vector3(0f, 0.6f, 0f);
			cam.transform.localPosition = eyesTrans.transform.localPosition;
		}
		else
		{
			feetPosition.localPosition = new Vector3(0f, -0.15f, 0f);
			controller.center = new Vector3(0f, 0f, 0f);
			controller.height = 0.4f;
			eyesTrans.localPosition = new Vector3(0f, 0.1f, 0f);
			cam.transform.localPosition = eyesTrans.transform.localPosition;
		}
		standup = set;
		GameUIController.instance.UpdateStandupSprite();
	}

	public override void StartControlling()
	{
		StopCoroutine("FootSteps");
		StartCoroutine("FootSteps");
		dead = false;
		if (GameUIController.instance != null) GameUIController.instance.SetBloodInCamera(false);
		if (CameraMoveAnimation.instance != null) CameraMoveAnimation.instance.StopAllAnimations();
		bool tutorialEnabled = TutorialController.instance != null && TutorialController.instance.isEnabled;
		if (!tutorialEnabled && GameUIController.instance != null && GameUIController.instance.pauseButton != null)
		{
			GameUIController.instance.pauseButton.SetActive(true);
		}
		if (!tutorialEnabled && GameUIController.instance != null && GameUIController.instance.standupButton != null)
		{
			GameUIController.instance.standupButton.SetActive(true);
		}
		if (GameUIController.instance != null && GameUIController.instance.fastHiddingUI != null) GameUIController.instance.fastHiddingUI.SetActive(false);
		if (GameUIController.instance != null && GameUIController.instance.centerPoint != null) GameUIController.instance.centerPoint.SetActive(true);
		if (GameUIController.instance != null && GameUIController.instance.useJoystick && !tutorialEnabled && GameUIController.instance.joystickUI != null)
		{
			GameUIController.instance.joystickUI.SetActive(true);
		}
		if (startRoomDoor != null) startRoomDoor.startingState = OpenCloseBehaviour.openState.CLOSED;
		if (playerStartPosition != null) base.transform.position = playerStartPosition.position;
		NoiseBehaviour[] array = Object.FindObjectsOfType<NoiseBehaviour>();
		NoiseBehaviour[] array2 = array;
		foreach (NoiseBehaviour noiseBehaviour in array2)
		{
			noiseBehaviour.Reset();
		}
		if (startRoomDoor != null && startRoomDoor.initialized)
		{
			startRoomDoor.Reset();
		}
		UpdateStandUp(true);
		if (controller == null) controller = GetComponent<CharacterController>();
		if (controller != null) controller.enabled = true;
		PlayerMovement playerMovement = GetComponent<PlayerMovement>();
		if (playerMovement != null) playerMovement.enabled = GetComponent<ExportedMovementDriver>() == null;
		if (playerStartPosition != null) base.transform.forward = playerStartPosition.forward;
		currentHide = null;
		isHidden = false;
		if (SimpleSmoothMouseLook.instance != null) SimpleSmoothMouseLook.instance.StartCamera();
		knowsNunFollowsHim = false;
		canPlayTensionSound = true;
		blocked = false;
		if (!tutorialEnabled)
		{
			Invoke("StartMessage", 2f);
		}
		base.StartControlling();
	}

	public override void BackToControlling()
	{
		CameraMoveAnimation.instance.StopAllAnimations();
		GameUIController.instance.pauseButton.SetActive(true);
		if (isHidden && currentHide.door == null)
		{
			GameUIController.instance.fastHiddingUI.SetActive(true);
			GameUIController.instance.hideButtonTxt.text = LocalizationManager.GetTranslation("unhide");
			((FastHide)currentHide).OnHide();
		}
		else
		{
			if (GameUIController.instance.useJoystick)
			{
				GameUIController.instance.joystickUI.SetActive(true);
			}
			GameUIController.instance.standupButton.SetActive(true);
			controller.enabled = true;
			PlayerMovement playerMovement = GetComponent<PlayerMovement>();
			// On PC the ExportedMovementDriver drives movement; enabling PlayerMovement
			// too would double-apply input. Match StartControlling.
			if (playerMovement != null) playerMovement.enabled = GetComponent<ExportedMovementDriver>() == null;
			SimpleSmoothMouseLook.instance.OnFinishUsingDoll();
		}
		blocked = false;
		base.BackToControlling();
	}

	public override void StopControlling()
	{
		movement.enabled = false;
		blocked = true;
		hasTouchableInFront = false;
		if (GameUIController.instance.interactionEnabledSprite.activeSelf)
		{
			GameUIController.instance.interactionEnabledSprite.SetActive(false);
		}
		GameUIController.instance.releaseButton.SetActive(false);
		GameUIController.instance.standupButton.SetActive(false);
		GameUIController.instance.actionButton.SetActive(false);
		GameUIController.instance.fastHiddingUI.SetActive(false);
		GameUIController.instance.j.Reset();
		if (GameUIController.instance.useJoystick)
		{
			GameUIController.instance.joystickUI.SetActive(false);
		}
		base.StopControlling();
	}

	public override void BlockPlayer()
	{
		GetComponent<PlayerMovement>().enabled = false;
		blocked = true;
		hasTouchableInFront = false;
		if (GameUIController.instance.interactionEnabledSprite.activeSelf)
		{
			GameUIController.instance.interactionEnabledSprite.SetActive(false);
		}
		GameUIController.instance.startDollButton.SetActive(false);
		GameUIController.instance.shootButton.SetActive(false);
		GameUIController.instance.releaseButton.SetActive(false);
		GameUIController.instance.standupButton.SetActive(false);
		GameUIController.instance.pauseButton.SetActive(false);
		GameUIController.instance.actionButton.SetActive(false);
		GameUIController.instance.fastHiddingUI.SetActive(false);
		GameUIController.instance.j.Reset();
		if (GameUIController.instance.useJoystick)
		{
			GameUIController.instance.joystickUI.SetActive(false);
		}
		GameUIController.instance.centerPoint.SetActive(false);
		base.BlockPlayer();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!hit.collider.CompareTag("Touchable"))
		{
			Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
			if (!(attachedRigidbody != null) || attachedRigidbody.isKinematic)
			{
				return;
			}
			if (hit.collider.CompareTag("Pushable"))
			{
				if (hit.collider.attachedRigidbody.velocity.magnitude < 0.3f)
				{
					attachedRigidbody.AddForceAtPosition(hit.controller.transform.forward.normalized * 10f, hit.point, ForceMode.Force);
					hit.collider.GetComponent<PushBehaviour>().ContactWithPlayer();
				}
			}
			else if (hit.collider.attachedRigidbody.velocity.magnitude < 0.3f)
			{
				attachedRigidbody.AddForceAtPosition(hit.controller.transform.forward.normalized * 10f, hit.point, ForceMode.Force);
			}
		}
		else if (!(hit.controller != null))
		{
		}
	}
}
