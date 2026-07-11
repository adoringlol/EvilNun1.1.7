using System.Collections;
using DG.Tweening;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Audio;

public class MyController : MonoBehaviour
{
	public static PlayerController instance;

	public PlayerMovement movement;

	public LayerMask touchableLayer;

	public TouchableElement current;

	public TakeableObject objectOnHand;

	public bool hasTouchableInFront;

	public TextMeshProUGUI touchingItemNameTxt;

	public TextMeshProUGUI actionNameTxt;

	public bool hasObjectOnHand;

	public bool canCheckAction;

	public Transform eyesTrans;

	public bool standup = true;

	public Transform objectPosition;

	public Transform feetPosition;

	public bool isHidden;

	public HiddenElement currentHide;

	public LayerMask elevatorLayer;

	public Transform cam;

	public CharacterController controller;

	public float lastVelocity;

	public float handObjectFollowSpeed = 12f;

	public Vector3 objectPositionOffset = new Vector3(0.5f, 0f, 0.5f);

	public float followObjectLocalMinX;

	public float followObjectLocalMaxX;

	public Transform playerStartPosition;

	public OpenCloseBehaviour startRoomDoor;

	public AudioSource source;

	public bool knowsNunFollowsHim;

	public bool canPlayTensionSound;

	public GameObject hat;

	public GameObject febatistaDoll;

	public GameObject feromonasItem;

	public GameObject johnWolfItem;

	public GameObject chivoItem;

	public GameObject vividPlaysItem;

	public GameObject genuItem;

	public GameObject windyItem;

	public GameObject kubzscoutsItem;

	public GameObject denisItem;

	public GameObject zbingzItem;

	public GameObject spidergamingItem;

	public GameObject funbabeItem;

	public GameObject leogamesItem;

	public GameObject pomahItem;

	public GameObject coryItem;

	public GameObject degoItem;

	public GameObject miawaugItem;

	public GameObject charliecharlieItem;

	public GameObject momoItem;

	public GameObject germansnakeItem;

	public bool dead;

	public AudioMixerSnapshot normal;

	public AudioMixerSnapshot hidden;

	public LayerMask techo;

	public AudioClip persecutionClip;

	public AudioClip heartClip;

	public LayerMask canSeeNunFromHide;

	public bool blocked;

	private void Awake()
	{
		canCheckAction = true;
		controller = GetComponent<CharacterController>();
	}

	private void Start()
	{
		VariablesGlobales.plays++;
		PlayerPrefs.SetInt("plays", VariablesGlobales.plays);
		AnalyticsEvent.Custom("start_game");
	}

	public void IsHidden()
	{
		isHidden = true;
		StartCoroutine("Breathing");
		hidden.TransitionTo(0.2f);
		PlayersManager.instance.GetCurrentController().OnFinishPersecution();
	}

	public void LeaveHide()
	{
		MasterAudio.StopSoundGroupOfTransform(base.transform, "breath");
		normal.TransitionTo(0.2f);
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.OnExitHide();
		}
	}

	public virtual void BackToControlling()
	{
	}

	public virtual void StartControlling()
	{
	}

	public virtual void StopControlling()
	{
	}

	private void Update()
	{
		DoUpdate();
	}

	public void NunForgotten()
	{
		canPlayTensionSound = true;
	}

	public virtual void DoUpdate()
	{
		if (blocked)
		{
			return;
		}
		if (hasObjectOnHand && objectOnHand != null)
		{
			objectOnHand.transform.forward = SimpleSmoothMouseLook.instance.transform.TransformDirection(objectOnHand.forwardAxis);
			Vector3 position = SimpleSmoothMouseLook.instance.itemPosition.position;
			objectOnHand.transform.position = position + objectOnHand.takenHeightOffset * Vector3.up;
		}
		if (ZombieBehaviour.instance == null || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.ATTACK || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.NONE || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.DEAD)
		{
			return;
		}
		if (ZombieBehaviour.instance.canSeePlayer && ZombieBehaviour.instance.IsInPlayersFOV() && !ZombieBehaviour.instance.IsInFOV())
		{
			MasterAudio.PlaySound("heart");
		}
		else
		{
			if ((!ZombieBehaviour.instance.canSeePlayer || !ZombieBehaviour.instance.IsInFOV() || !ZombieBehaviour.instance.IsInPlayersFOV() || ZombieBehaviour.instance.state != ZombieBehaviour.NunState.SEEN_PLAYER) && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.FOLLOWING_PLAYER)
			{
				return;
			}
			if (canPlayTensionSound)
			{
				if (ZombieBehaviour.instance.GetDistanceBetween(base.transform.position, ZombieBehaviour.instance.transform.position) < 5f && ZombieBehaviour.instance.GetDistanceBetween(base.transform.position, ZombieBehaviour.instance.transform.position) > 2f)
				{
					MasterAudio.PlaySound3DAtVector3("nun_scream", ZombieBehaviour.instance.eyesTrans.transform.position);
					canPlayTensionSound = false;
					CancelInvoke("NunForgotten");
					Invoke("NunForgotten", 6f);
				}
				else
				{
					MasterAudio.PlaySound("tension");
					canPlayTensionSound = false;
					CancelInvoke("NunForgotten");
					Invoke("NunForgotten", 6f);
				}
			}
			else
			{
				canPlayTensionSound = false;
				CancelInvoke("NunForgotten");
				Invoke("NunForgotten", 3f);
			}
			MasterAudio.StopAllOfSound("heart");
			if (!isHidden)
			{
				MasterAudio.PlaySound("persecution", 1f, null, 0.5f);
			}
		}
	}

	public void OnStartWalking()
	{
		CameraMoveAnimation.instance.CameraStepAnimation();
	}

	public void OnStopWalking()
	{
		CameraMoveAnimation.instance.CameraIdleAnimation();
	}

	public void IsWalking()
	{
		CameraMoveAnimation.instance.CameraStepAnimation();
	}

	public void TakeItem(TakeableObject obj)
	{
		if (hasObjectOnHand)
		{
			if (objectOnHand.id == 333 && obj.id == 3331)
			{
				((GumgunTakeable)objectOnHand).ChargeGum(obj.GetComponent<ShootBehaviour>());
				MasterAudio.PlaySound("grab");
				return;
			}
			objectOnHand.OnReleaseItem();
		}
		hasObjectOnHand = true;
		objectOnHand = obj;
		obj.transform.parent = objectPosition;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.position += obj.takenHeightOffset * Vector3.up;
		obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
		objectOnHand.transform.forward = SimpleSmoothMouseLook.instance.transform.TransformDirection(objectOnHand.forwardAxis);
		if (!isHidden)
		{
			GameUIController.instance.releaseButton.SetActive(true);
		}
		ElevatorCageTrigger.instance.RemoveItem(obj.col);
		MasterAudio.PlaySound("grab");
	}

	public bool CanStandup()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, base.transform.up, out hitInfo, 1f, techo))
		{
			return false;
		}
		return true;
	}

	public virtual void UpdateStandUp()
	{
	}

	public virtual void UpdateStandUp(bool set)
	{
	}

	public void StopPlayingClip()
	{
		source.DOKill();
		source.DOFade(0f, 1f).OnComplete(StopSource);
	}

	public void StopSource()
	{
		source.Stop();
	}

	public void CanCheckAction()
	{
		canCheckAction = true;
	}

	public virtual void BlockPlayer()
	{
	}

	public void OnFinishPersecution()
	{
		MasterAudio.StopAllOfSound("persecution");
		MasterAudio.StopAllOfSound("breath");
	}

	public IEnumerator Breathing()
	{
		yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
		while (isHidden)
		{
			MasterAudio.PlaySound3DAtVector3("breath", base.transform.position);
			yield return new WaitForSeconds(Random.Range(1.5f, 2f));
		}
	}

	public void ThrowHat()
	{
		if (CarboardController.instance.isItown && !hat.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			hat.transform.position = position;
			hat.SetActive(true);
			hat.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowChivoHat()
	{
		if (CarboardController.instance.chivoLife && !chivoItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			chivoItem.transform.position = position;
			chivoItem.SetActive(true);
			chivoItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowVividPlaysHat()
	{
		if (CarboardController.instance.vividPlays && !vividPlaysItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			vividPlaysItem.transform.position = position;
			vividPlaysItem.SetActive(true);
			vividPlaysItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowKubzscoutsItem()
	{
		if (CarboardController.instance.kubzscouts && !kubzscoutsItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			kubzscoutsItem.transform.position = position;
			kubzscoutsItem.SetActive(true);
			kubzscoutsItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowGenuItem()
	{
		if (CarboardController.instance.isGenu && !genuItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			genuItem.transform.position = position;
			genuItem.SetActive(true);
			genuItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowDenisItem()
	{
		if (VariablesGlobales.ownName && VariablesGlobales.playerOwnName.ToLower().Equals("denis") && !denisItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			denisItem.transform.position = position;
			denisItem.SetActive(true);
			denisItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowZbingzItem()
	{
		if (VariablesGlobales.ownName && VariablesGlobales.playerOwnName.ToLower().Equals("zbing z.") && !zbingzItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			zbingzItem.transform.position = position;
			zbingzItem.SetActive(true);
			zbingzItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowGermanSnakeItem()
	{
		if (CarboardController.instance.isGermanSnake && !germansnakeItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			germansnakeItem.transform.position = position;
			germansnakeItem.SetActive(true);
			germansnakeItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowFebatistaDoll()
	{
		if (CarboardController.instance.febatista && !febatistaDoll.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			febatistaDoll.transform.position = position;
			febatistaDoll.SetActive(true);
			febatistaDoll.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowFeromonasItem()
	{
		if (CarboardController.instance.isFeromonas && !feromonasItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			feromonasItem.transform.position = position;
			feromonasItem.SetActive(true);
			feromonasItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void ThrowJohnWolfeItem()
	{
		if (CarboardController.instance.isJohnWolfe && !johnWolfItem.activeSelf)
		{
			Vector3 position = SimpleSmoothMouseLook.instance.transform.position + Vector3.up * 1.2f + SimpleSmoothMouseLook.instance.transform.forward.normalized * 0.2f;
			johnWolfItem.transform.position = position;
			johnWolfItem.SetActive(true);
			johnWolfItem.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void StartMessage()
	{
		MessagesManager.instance.ShowMessage("initial_message");
	}

	public void FallHole()
	{
		MasterAudio.PlaySound("tension_" + Random.Range(1, 3));
		SimpleSmoothMouseLook.instance.OnFallInHole();
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.Release();
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
		}
		if (ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.BlockNun();
		}
		BlockPlayer();
		CameraMoveAnimation.instance.PlayFallHoleAnimation();
		OnFinishPersecution();
		PlayersManager.instance.GetCurrentController().GetComponent<PlayerMovement>().enabled = false;
	}
}
