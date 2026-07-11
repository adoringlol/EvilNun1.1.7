using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using Pathfinding;
using UnityEngine;

public class OpenCloseBehaviour : TouchableElement
{
	public enum openState
	{
		OPENED = 0,
		CLOSED = 1,
		CLOSING = 2,
		OPENING = 3,
		CLOSING_AUTOMATIC = 4,
		CLOSED_BY_KEY = 5,
		CLOSED_BY_MACHINE_CODE = 6,
		CLOSED_FOREVER = 7
	}

	public PositionPlace place;

	public objectType type;

	public openState state;

	public Transform trans;

	public Vector3 rotationOpened;

	public Vector3 rotationClosed;

	public float openedHeight;

	public float closedHeight;

	public bool needObjectToOpen;

	public int neededObjectID;

	public string needObjectKeyMessage;

	public openState startingState;

	public bool canStartClosedByKey;

	[SoundGroup]
	public string audioOpen;

	[SoundGroup]
	public string audioClose;

	public bool closeSoundOnStart;

	public float actionTime = 0.5f;

	public float closeTime = 0.5f;

	public float minHearDistance = 2f;

	public float maxHearDistance = 10f;

	public Transform[] openPositionForEnemie;

	public Vector2 cutRectangle;

	public TouchableElement[] linkedDoors;

	public TouchableElement[] multipleDoors;

	public OcclusionPortal portal;

	public List<Collider> colliders;

	public AnimationCurve openCurve;

	public AnimationCurve closeCurve;

	public Collider[] moreColliders;

	public bool initialized;

	public Vector3 noiseLocalPosition;

	private bool byPlayer_last;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position + noiseLocalPosition, 0.3f);
	}

	public Transform GetNearestEnterPosition(Vector3 pos)
	{
		if (openPositionForEnemie == null || openPositionForEnemie.Length == 0) return trans == null ? base.transform : trans;
		if (openPositionForEnemie.Length == 1)
		{
			return openPositionForEnemie[0];
		}
		float num = Vector3.Distance(openPositionForEnemie[0].position, pos);
		float num2 = Vector3.Distance(openPositionForEnemie[1].position, pos);
		if (num < num2)
		{
			return openPositionForEnemie[0];
		}
		return openPositionForEnemie[1];
	}

	public Transform GetOppositePosition(Transform pos)
	{
		if (openPositionForEnemie == null) return null;
		if (openPositionForEnemie.Length == 2)
		{
			if (openPositionForEnemie[0] == pos)
			{
				return openPositionForEnemie[1];
			}
			return openPositionForEnemie[0];
		}
		return null;
	}

	public Vector3 GetNoisePosition()
	{
		if (openPositionForEnemie == null || openPositionForEnemie.Length == 0)
		{
			return trans.position + trans.forward * 1f;
		}
		Transform enter = GetRandomEnterPosition();
		return enter == null ? base.transform.position : enter.position;
	}

	public Transform GetRandomEnterPosition()
	{
		if (openPositionForEnemie == null || openPositionForEnemie.Length == 0) return null;
		return openPositionForEnemie[Random.Range(0, openPositionForEnemie.Length)];
	}

	public override void Initialize()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;
		if (colliders == null) colliders = new List<Collider>();
		if (linkedDoors == null) linkedDoors = new TouchableElement[0];
		if (multipleDoors == null) multipleDoors = new TouchableElement[0];
		if (moreColliders == null) moreColliders = new Collider[0];
		if (trans == null)
		{
			trans = base.transform;
		}
		if ((bool)GetComponent<NavmeshCut>())
		{
			cutRectangle = GetComponent<NavmeshCut>().rectangleSize;
		}
		if ((bool)GetComponent<OcclusionPortal>())
		{
			portal = GetComponent<OcclusionPortal>();
		}
		if (startingState == openState.CLOSED_FOREVER)
		{
			trans.localEulerAngles = rotationClosed;
		}
		else if (startingState == openState.CLOSED || startingState == openState.CLOSED_BY_KEY)
		{
			trans.localEulerAngles = rotationClosed;
		}
		else
		{
			trans.localEulerAngles = rotationOpened;
		}
		if (startingState == openState.CLOSED)
		{
			if (Random.value > 0.5f && canStartClosedByKey)
			{
				startingState = openState.CLOSED_BY_KEY;
			}
			else
			{
				startingState = openState.CLOSED;
			}
		}
		state = startingState;
		if ((bool)GetComponent<Collider>())
		{
			colliders.Add(GetComponent<Collider>());
		}
		SetColliders(true);
		if (state == openState.CLOSED || state == openState.CLOSED_BY_KEY)
		{
			if ((bool)GetComponent<NavmeshCut>())
			{
				GetComponent<NavmeshCut>().rectangleSize = new Vector2(0f, 0f);
				GetComponent<NavmeshCut>().ForceUpdate();
			}
			if (portal != null)
			{
				portal.open = false;
			}
		}
		if (state == openState.OPENED)
		{
			if (portal != null)
			{
				portal.open = true;
			}
			if ((bool)GetComponent<NavmeshCut>())
			{
				GetComponent<NavmeshCut>().rectangleSize = cutRectangle;
				GetComponent<NavmeshCut>().ForceUpdate();
			}
		}
		if (type == objectType.DOOR || type == objectType.TRAP_DOOR)
		{
			Invoke("InitDelayed", 0.5f);
		}
	}

	public void Reset()
	{
		if (startingState == openState.CLOSED || startingState == openState.CLOSED_BY_KEY)
		{
			trans.localEulerAngles = rotationClosed;
		}
		else
		{
			trans.localEulerAngles = rotationOpened;
		}
		if (startingState == openState.CLOSED)
		{
			if (Random.value > 0.5f && canStartClosedByKey)
			{
				startingState = openState.CLOSED_BY_KEY;
			}
			else
			{
				startingState = openState.CLOSED;
			}
		}
		state = startingState;
		SetColliders(true);
		if ((state == openState.CLOSED || state == openState.CLOSED_BY_KEY) && (bool)GetComponent<NavmeshCut>())
		{
			GetComponent<NavmeshCut>().rectangleSize = new Vector2(0f, 0f);
			GetComponent<NavmeshCut>().ForceUpdate();
		}
	}

	public void InitDelayed()
	{
		if (DoorsManager.instance != null) DoorsManager.instance.AddDoor(this);
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		byPlayer_last = byPlayer;
		if (linkedDoors.Length > 0)
		{
			TouchableElement[] array = linkedDoors;
			foreach (TouchableElement touchableElement in array)
			{
				((PersianaCustomAnimationBehaviour)touchableElement).OpenPersiana();
			}
		}
		if (state == openState.CLOSED_BY_KEY)
		{
			if (needObjectToOpen && PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == neededObjectID)
			{
				state = openState.CLOSED;
				if (PlayersManager.instance.GetCurrentController().objectOnHand.id == 45)
				{
					MasterAudio.PlaySound3DAtVector3AndForget("LockerLocked", base.transform.position + noiseLocalPosition);
					MessagesManager.instance.ShowMessage("use_locker_code");
					PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
				}
				return;
			}
			FlickDoor(byPlayer);
			if (id == 100)
			{
				MessagesManager.instance.ShowMessage("need_code_door");
			}
			else if (needObjectToOpen)
			{
				MessagesManager.instance.ShowMessage(needObjectKeyMessage);
			}
		}
		if (state == openState.CLOSED_FOREVER)
		{
			FlickDoor(byPlayer);
		}
		if (state == openState.OPENED)
		{
			if (!byPlayer)
			{
				SetColliders(false);
			}
			state = openState.CLOSING;
			trans.DOKill();
			float num = actionTime;
			if (!byPlayer)
			{
				if (trapPlayer)
				{
					num = 0.3f;
				}
				else if (ZombieBehaviour.instance.targetType == ZombieBehaviour.NunTarget.PLAYER)
				{
					num = actionTime / 2f;
				}
			}
			switch (type)
			{
			case objectType.DOOR:
				trans.DOLocalRotate(rotationClosed, closeTime).SetEase(closeCurve).OnComplete(OnFinishAction);
				break;
			case objectType.WINDOW:
				trans.DOMoveY(closedHeight, num).OnComplete(OnFinishAction);
				break;
			case objectType.TRAP_DOOR:
				trans.DOLocalRotate(rotationClosed, num).SetEase(closeCurve).OnComplete(OnFinishAction);
				break;
			case objectType.LOCKER:
				trans.DOLocalRotate(rotationClosed, num).SetEase(closeCurve).OnComplete(OnFinishAction);
				if (closeSoundOnStart)
				{
					MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
				}
				break;
			case objectType.PERSIANA:
				trans.DOMoveY(closedHeight, num).SetEase(closeCurve).OnComplete(OnFinishAction);
				MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
				break;
			case objectType.PERSIANA_SMALL:
				trans.DOLocalMoveY(closedHeight, num).SetEase(closeCurve).OnComplete(OnFinishAction);
				MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
				break;
			case objectType.PALANCA:
				trans.DOLocalRotate(rotationClosed, actionTime).SetEase(closeCurve).OnComplete(OnFinishAction);
				break;
			case objectType.DRAWER:
				trans.DOLocalMoveX(closedHeight, num).SetEase(closeCurve).OnComplete(OnFinishAction);
				MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
				break;
			case objectType.PUERTA_HORNO:
				trans.DOLocalRotate(rotationClosed, actionTime).SetEase(closeCurve).OnComplete(OnFinishAction);
				break;
			}
			Invoke("NoiseAtClose", num);
			if (ZombieBehaviour.instance != null && byPlayer)
			{
				ZombieBehaviour.instance.OnUsedDoor(this);
			}
		}
		else if (state == openState.CLOSED)
		{
			trans.DOKill();
			if (!byPlayer)
			{
				SetColliders(false);
			}
			if (portal != null)
			{
				portal.open = true;
			}
			switch (type)
			{
			case objectType.DOOR:
				trans.DOLocalRotate(rotationOpened, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.WINDOW:
				trans.DOMoveY(openedHeight, actionTime).OnComplete(OnFinishAction);
				break;
			case objectType.TRAP_DOOR:
				trans.DOLocalRotate(rotationOpened, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.LOCKER:
				trans.DOLocalRotate(rotationOpened, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.PERSIANA:
				trans.DOMoveY(openedHeight, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.PERSIANA_SMALL:
				trans.DOLocalMoveY(openedHeight, actionTime).SetEase(closeCurve).OnComplete(OnFinishAction);
				MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
				break;
			case objectType.PALANCA:
				trans.DOLocalRotate(rotationOpened, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.DRAWER:
				trans.DOLocalMoveX(openedHeight, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			case objectType.PUERTA_HORNO:
				trans.DOLocalRotate(rotationOpened, actionTime).SetEase(openCurve).OnComplete(OnFinishAction);
				break;
			}
			state = openState.OPENING;
			if (audioOpen != string.Empty)
			{
				MasterAudio.PlaySound3DAtVector3AndForget(audioOpen, base.transform.position + noiseLocalPosition);
			}
			if (ZombieBehaviour.instance != null && byPlayer && (VariablesGlobales.difficultyMode > 0 || type == objectType.DOOR || type == objectType.TRAP_DOOR))
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position + noiseLocalPosition, maxHearDistance, string.Empty);
			}
		}
	}

	public void NoiseAtClose()
	{
		if (ZombieBehaviour.instance != null && byPlayer_last && VariablesGlobales.difficultyMode > 1)
		{
			ZombieBehaviour.instance.AddNoise(base.transform.position, maxHearDistance, string.Empty);
		}
	}

	public void FlickDoor(bool byPlayer = false)
	{
		if (type == objectType.LOCKER)
		{
			MasterAudio.PlaySound3DAtVector3AndForget("LockerLocked", base.transform.position + noiseLocalPosition);
		}
		else
		{
			MasterAudio.PlaySound3DAtVector3AndForget("flick_door", base.transform.position + noiseLocalPosition);
		}
		trans.DOShakeRotation(1.5f, new Vector3(0f, 0.5f, 0f));
		if ((!byPlayer || id != 99) && id != 100)
		{
			if (!byPlayer)
			{
				Invoke("FinishUnlockingDoor", 1f);
			}
			else if (state == openState.CLOSED_FOREVER)
			{
				MessagesManager.instance.ShowMessage("blocked");
			}
			else
			{
				MessagesManager.instance.ShowMessage("key_closed");
			}
		}
	}

	public void FinishUnlockingDoor()
	{
		state = openState.CLOSED;
	}

	[ContextMenu("PlayTestSound")]
	public void PlayTestSound()
	{
		MasterAudio.PlaySound3DAtVector3AndForget(audioOpen, base.transform.position + noiseLocalPosition);
	}

	public virtual void OnFinishAction()
	{
		if (state == openState.OPENING)
		{
			state = openState.OPENED;
			if ((bool)GetComponent<NavmeshCut>())
			{
				GetComponent<NavmeshCut>().rectangleSize = cutRectangle;
				GetComponent<NavmeshCut>().ForceUpdate();
			}
		}
		else if (state == openState.CLOSING)
		{
			if ((bool)GetComponent<NavmeshCut>())
			{
				GetComponent<NavmeshCut>().rectangleSize = new Vector2(0f, 0f);
				GetComponent<NavmeshCut>().ForceUpdate();
			}
			if (portal != null)
			{
				portal.open = false;
			}
			state = openState.CLOSED;
			if (type != objectType.PERSIANA && type != objectType.DRAWER && !closeSoundOnStart && audioClose != string.Empty)
			{
				MasterAudio.PlaySound3DAtVector3AndForget(audioClose, base.transform.position + noiseLocalPosition);
			}
		}
		SetColliders(true);
	}

	public void SetColliders(bool s)
	{
		foreach (Collider collider in colliders)
		{
			collider.isTrigger = !s;
		}
	}
}
