using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using Pathfinding;
using TMPro;
using UnityEngine;

public class ZombieBehaviour : AIPath
{
	public enum NunState
	{
		NONE = 0,
		IDLE = 1,
		LOOKING_AROUND = 2,
		SEEN_PLAYER = 3,
		FOLLOWING_PLAYER = 4,
		GO_TO_DOOR = 5,
		GO_TO_HIDE = 6,
		GO_TO_PRAY = 7,
		PRAYING = 8,
		OPENING_DOOR = 9,
		FOLLOWING_LAST_SEEN_POSITION = 10,
		FOLLOWING_LAST_HEARD_POSITION = 11,
		RANDOM_WAY = 12,
		ATTACK = 13,
		HEAR_NOISE = 14,
		DEAD = 15,
		LOOKING_DOLL = 16,
		ON_GUM_HEAD = 17,
		ON_GUM_FEET = 18
	}

	public enum NunTarget
	{
		NONE = 0,
		PLAYER = 1,
		RANDOM = 2,
		DOOR = 3,
		ROOM = 4,
		NOISE = 5,
		LAST_SEEN_PLAYER_POSITION = 6,
		PRAY = 7
	}

	public enum ZombieAnimation
	{
		NONE = 0,
		IDLE = 1,
		RUN = 2,
		WALK = 3,
		ATTACK = 4,
		CRAWL = 5,
		OPENING = 6,
		LOOKING = 7
	}

	public static ZombieBehaviour instance;

	public objectType lastNoiseType;

	public ZombieAnimation animState;

	public ZombieAnimation desiredAnimState;

	public NunState _state;

	public NunState secondaryState;

	public NunTarget _targetType;

	public NunState beforeState;

	public NunState beforePlayerDiedState;

	public Vector3 targetPosition;

	public Vector3 openDoorPosition;

	public Vector3 lastNoisePosition;

	public Vector3 gizmoPosition;

	public GameObject recoverNunEffect;

	public GameObject devilNote;

	public GameObject hammer;

	public GameObject gumFeet;

	public GameObject gumHead;

	public GameObject gumBody;

	public TextMeshProUGUI devilNoteTxt;

	public RoomCodeBehaviour codeBehaviour;

	public Animation anim;

	public Transform eyesTrans;

	public Transform currentRandomPoint;

	public Transform feetPosition;

	public Transform explosionPointTest;

	public Transform nunModel;

	public Transform yaw;

	public Transform currentHidePositionToDiscover;

	public Transform prayPosition;

	public OpenCloseBehaviour doorTarget;

	public LayerMask layersToCheckCrawlEntrance;

	public LayerMask layersToCheckifSeePlayer;

	public LayerMask layersToCheckCrawling;

	public LayerMask layersToCheckDoors;

	public LayerMask layersToCheckWhenDizzy;

	[SerializeField]
	public List<RagDollElement> ragdollJoints;

	public HeadLookController lookController;

	public TrapDoorBehaviour initialTrapDoor;

	public TakeableObject glasses;

	public ZombieAnimation beforeExplodeANim;

	public TakeableObject strongboxKey;

	public float distanceToPlayer;

	public float lastPathDistanceToPlayer;

	public float timerLostSeenPlayer;

	public float currentNoiseDistance;

	public float noiseDistanceToNunAtNoiseMoment;

	public float distanceToSeePlayer = 10f;

	public bool destroyOnAttack;

	public bool canSeePlayer;

	public bool sawPlayerHiding;

	public bool needMove;

	public bool blocked;

	public bool wearingGlasses = true;

	public bool shouldMove;

	public bool prayed;

	public bool standup;

	public bool isInFov;

	public bool cantAttack;

	private readonly float timeRemoveFeetGum = 20f;

	private readonly float timeRemoveHeadGum = 20f;

	private readonly float timeRemoveBodyGum = 20f;

	public bool ghostMode = true;

	public NunTarget targetType
	{
		get
		{
			return _targetType;
		}
		set
		{
			_targetType = value;
		}
	}

	public NunState state
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state != value)
			{
				beforeState = _state;
			}
			_state = value;
		}
	}

	private new void Awake()
	{
		instance = this;
		ghostMode = VariablesGlobales.difficultyMode == 3;
		controller = GetComponent<CharacterController>();
		lookController = GetComponent<HeadLookController>();
		UpdateRagDollInfo();
	}

	public void UpdateRagDollInfo()
	{
		foreach (RagDollElement ragdollJoint in ragdollJoints)
		{
			ragdollJoint.localeuler = ragdollJoint.rb.transform.localEulerAngles;
			ragdollJoint.localpos = ragdollJoint.rb.transform.localPosition;
		}
	}

	public void OnHeadShot()
	{
		MasterAudio.StopAllSoundsOfTransform(base.transform);
		canMove = false;
		base.transform.DOKill();
		CancelInvoke();
		gumHead.SetActive(true);
		secondaryState = NunState.ON_GUM_HEAD;
		MasterAudio.PlaySound3DAtVector3("hit_nun", base.transform.position);
		StopAllRoutinesUnlessIA();
		StopCoroutine("DoingDizzy");
		StartCoroutine("DoingDizzy");
	}

	public void OnShotBody()
	{
		MasterAudio.StopAllSoundsOfTransform(base.transform);
		gumBody.SetActive(true);
		MasterAudio.PlaySound3DAtVector3("hit_nun", base.transform.position);
		StartCoroutine("RemoveGumBody");
	}

	public IEnumerator RemoveGumBody()
	{
		yield return new WaitForSeconds(timeRemoveBodyGum);
		gumBody.SetActive(false);
	}

	public void OnFeetContactGum()
	{
		if (state != NunState.DEAD && !blocked)
		{
			MasterAudio.StopAllSoundsOfTransform(base.transform);
			canMove = false;
			base.transform.DOKill();
			CancelInvoke();
			anim.CrossFade("gum_feet");
			animState = ZombieAnimation.IDLE;
			gumFeet.SetActive(true);
			gumFeet.transform.position = GetFeetPosition() - Vector3.up * 0.03f;
			state = NunState.ON_GUM_FEET;
			MasterAudio.PlaySound3DAtVector3("hit_nun", base.transform.position);
			StopAllRoutinesUnlessIA();
			StopCoroutine("GumOnFeetRoutine");
			StartCoroutine("GumOnFeetRoutine");
		}
	}

	public void StopAllRoutinesUnlessIA()
	{
		base.transform.DOKill();
		CancelInvoke();
		if (animState == ZombieAnimation.CRAWL)
		{
			Invoke("CheckCrawling", 2f);
		}
		if (state == NunState.OPENING_DOOR)
		{
			rotationSpeed = 300f;
		}
		StopCoroutine("StartLookingAround");
		StopCoroutine("OnSeenPlayer");
		StopCoroutine("OpeningDoor");
		StopCoroutine("HeardNoiseRoutine");
		StopCoroutine("StartIdle");
		StopCoroutine("StartPraying");
		StopCoroutine("StartCrawl");
		StopCoroutine("EndCrawl");
	}

	public void StartRagDoll(Vector3 explosionPoint)
	{
		MasterAudio.StopAllSoundsOfTransform(base.transform);
		beforeExplodeANim = animState;
		StopCoroutine("GumOnFeetRoutine");
		gumFeet.SetActive(false);
		canMove = false;
		StopAllCoroutines();
		CancelInvoke();
		BlockNun();
		state = NunState.DEAD;
		foreach (RagDollElement ragdollJoint in ragdollJoints)
		{
			ragdollJoint.rb.isKinematic = false;
		}
		anim.Stop();
		if (wearingGlasses)
		{
			wearingGlasses = false;
			glasses.transform.parent = null;
			glasses.rb.isKinematic = false;
			glasses.interactuable = true;
			glasses.col.enabled = true;
			glasses.rb.AddExplosionForce(500f, explosionPoint, 2f, 3f);
		}
		controller.enabled = false;
		foreach (RagDollElement ragdollJoint2 in ragdollJoints)
		{
			ragdollJoint2.rb.isKinematic = false;
			ragdollJoint2.rb.AddExplosionForce(2000f, explosionPoint, 5f, 3f);
		}
		MessagesManager.instance.ShowMessage("nun_exploded", "60");
		StartCoroutine("FinishRagDoll");
	}

	public IEnumerator FinishRagDoll()
	{
		yield return new WaitForSeconds(3f);
		if (((NunKeyBehaviour)strongboxKey).hasNun)
		{
			strongboxKey.transform.parent = null;
			strongboxKey.rb.isKinematic = false;
			strongboxKey.interactuable = true;
			((NunKeyBehaviour)strongboxKey).KeyReleased();
		}
		yield return new WaitForSeconds(57f);
		recoverNunEffect.transform.position = ragdollJoints[10].rb.transform.position - Vector3.up * 0.1f;
		recoverNunEffect.SetActive(true);
		yield return new WaitForSeconds(2f);
		nunModel.transform.parent = base.transform;
		controller.enabled = true;
		gumBody.SetActive(false);
		gumFeet.SetActive(false);
		gumHead.SetActive(false);
		base.transform.position = ragdollJoints[10].rb.transform.position + Vector3.up * controller.height / 2f;
		foreach (RagDollElement ragdollJoint in ragdollJoints)
		{
			ragdollJoint.rb.transform.localEulerAngles = ragdollJoint.localeuler;
			ragdollJoint.rb.transform.localPosition = ragdollJoint.localpos;
			ragdollJoint.rb.isKinematic = true;
		}
		if (beforeExplodeANim == ZombieAnimation.CRAWL)
		{
			anim.Play("crawl");
			nunModel.transform.localPosition = new Vector3(nunModel.localPosition.x, -0.3f, nunModel.localPosition.z);
			animState = ZombieAnimation.CRAWL;
			CancelInvoke("CheckCrawling");
			Invoke("CheckCrawling", 1f);
		}
		else
		{
			anim.Play("idle1");
			nunModel.transform.localPosition = new Vector3(nunModel.localPosition.x, -0.86f, nunModel.localPosition.z);
			animState = ZombieAnimation.IDLE;
		}
		yield return new WaitForSeconds(1f);
		RecoverNun();
		yield return new WaitForSeconds(1f);
		recoverNunEffect.SetActive(false);
	}

	public IEnumerator GumOnFeetRoutine()
	{
		maxSpeed = 0f;
		yield return new WaitForSeconds(timeRemoveFeetGum);
		RemoveFeetGum();
	}

	public IEnumerator StartLookingAround()
	{
		state = NunState.LOOKING_AROUND;
		if (Random.value > 0.5f && !MasterAudio.IsSoundGroupPlaying("tarareo") && !MasterAudio.IsSoundGroupPlaying("susurrar") && !MasterAudio.IsSoundGroupPlaying("whisper") && Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DFollowTransform("whisper", base.transform);
		}
		shouldMove = false;
		needMove = false;
		if (animState != ZombieAnimation.CRAWL)
		{
			anim.CrossFade("look_around");
			yield return new WaitForSeconds(anim["look_around"].length);
		}
		StartCoroutine("StartIdle");
	}

	public float GetDistanceBetween(Vector3 f, Vector3 t)
	{
		if (Mathf.Abs(f.y - t.y) > 1.5f)
		{
			return Vector3.Distance(f, t) + 5f;
		}
		t.y = 0f;
		f.y = 0f;
		return Vector3.Distance(f, t);
	}

	public IEnumerator DoingDizzy()
	{
		float dizzyTimer = 0f;
		float timerToRemoveGum = timeRemoveHeadGum;
		while (timerToRemoveGum > 0f)
		{
			if (state != NunState.ON_GUM_FEET && state != NunState.DEAD && !blocked)
			{
				if (animState != ZombieAnimation.RUN && animState != ZombieAnimation.CRAWL)
				{
					if (CanStandup())
					{
						UpdateStandUp(true);
						anim.CrossFade("run_with_gum");
						animState = ZombieAnimation.RUN;
						NormalizeSpeed();
					}
					else
					{
						UpdateStandUp(false);
						anim.CrossFade("crawl");
						animState = ZombieAnimation.CRAWL;
						NormalizeSpeed();
					}
				}
				Vector3 desiredMove = base.transform.forward.normalized;
				RaycastHit hitInfo;
				Physics.SphereCast(base.transform.position, controller.radius, Vector3.down, out hitInfo, controller.height / 2f, -1, QueryTriggerInteraction.Ignore);
				desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);
				Vector3 m_MoveDir = Vector3.zero;
				m_MoveDir.x = desiredMove.x * maxSpeed;
				m_MoveDir.z = desiredMove.z * maxSpeed;
				if (controller.isGrounded)
				{
					m_MoveDir.y = -9f;
				}
				else
				{
					m_MoveDir += Physics.gravity * 9f * Time.fixedDeltaTime;
				}
				controller.Move(m_MoveDir * Time.fixedDeltaTime);
				dizzyTimer += Time.deltaTime;
				if (dizzyTimer > 0.3f)
				{
					dizzyTimer = 0f;
					Vector3 to = base.transform.position + base.transform.forward;
					Vector3 from = base.transform.position - Vector3.up * 0.2f;
					Vector3 dir = to - from;
					RaycastHit hit;
					if (Physics.Raycast(from, dir, out hit, 0.8f, layersToCheckWhenDizzy))
					{
						if (animState == ZombieAnimation.RUN)
						{
							anim.CrossFade("hit_wall");
							animState = ZombieAnimation.IDLE;
						}
						yield return new WaitForSeconds(anim["hit_wall"].length);
						Vector3 newDir = base.transform.forward;
						float minAngle = Random.Range(-180, 180);
						if (Mathf.Abs(minAngle) < 60f)
						{
							minAngle = ((!(Random.value > 0.5f)) ? (-60) : 60);
						}
						newDir.y += minAngle;
						base.transform.DORotate(newDir, 0.5f);
						yield return new WaitForSeconds(0.5f);
						timerToRemoveGum -= 0.5f;
					}
				}
			}
			timerToRemoveGum -= Time.deltaTime;
			yield return null;
		}
		gumHead.gameObject.SetActive(false);
		if (state != NunState.ON_GUM_FEET && !blocked && state != NunState.DEAD)
		{
			if (CanStandup())
			{
				UpdateStandUp(true);
				animState = ZombieAnimation.NONE;
			}
			else
			{
				UpdateStandUp(false);
				anim.CrossFade("crawl");
				animState = ZombieAnimation.CRAWL;
			}
			canMove = true;
			StopAllRoutinesUnlessIA();
			StartCoroutine("StartIdle");
		}
		secondaryState = NunState.NONE;
	}

	public IEnumerator OpeningDoor()
	{
		shouldMove = false;
		needMove = false;
		state = NunState.OPENING_DOOR;
		desiredAnimState = animState;
		Transform enterPosition = doorTarget.GetNearestEnterPosition(GetFeetPosition());
		Transform oppositePosition = doorTarget.GetOppositePosition(enterPosition);
		rotationSpeed = 0f;
		yield return base.transform.DORotate(Quaternion.LookRotation(enterPosition.forward).eulerAngles, 0.3f).WaitForCompletion();
		rotationSpeed = 300f;
		Invoke("FixOpeningDoor", 10f);
		bool trapPlayer = false;
		if (PlayersManager.instance.GetCurrentController().isHidden && PlayersManager.instance.GetCurrentController().currentHide.door == doorTarget)
		{
			trapPlayer = true;
		}
		if (animState != ZombieAnimation.CRAWL)
		{
			anim.CrossFade("idle1");
			animState = ZombieAnimation.IDLE;
			if (trapPlayer)
			{
				yield return null;
			}
			else if (targetType == NunTarget.PLAYER)
			{
				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
		}
		if (doorTarget != null)
		{
			if (doorTarget.state == OpenCloseBehaviour.openState.CLOSED_BY_KEY)
			{
				if (animState != ZombieAnimation.CRAWL)
				{
					anim.CrossFade("open_inside");
					animState = ZombieAnimation.OPENING;
					yield return new WaitForSeconds(anim["open_inside"].length / 2f);
					doorTarget.FlickDoor();
					yield return new WaitForSeconds(anim["open_inside"].length / 2f);
					doorTarget.trans.DOKill();
					while (doorTarget.state == OpenCloseBehaviour.openState.CLOSED_BY_KEY)
					{
						yield return null;
					}
					doorTarget.Touched(false, trapPlayer);
				}
			}
			else if (doorTarget.state == OpenCloseBehaviour.openState.CLOSED)
			{
				anim.CrossFade("open");
				// The "open" clip is absent from the nun's Animation in this export, so
				// anim["open"] is null and CrossFade no-ops. Guard the length access to
				// avoid the NullReferenceException logged in OpeningDoor.
				yield return new WaitForSeconds((anim["open"] != null) ? anim["open"].length : 0.5f);
				if (doorTarget.multipleDoors.Length > 0)
				{
					TouchableElement[] multipleDoors = doorTarget.multipleDoors;
					for (int i = 0; i < multipleDoors.Length; i++)
					{
						OpenCloseBehaviour openCloseBehaviour = (OpenCloseBehaviour)multipleDoors[i];
						openCloseBehaviour.Touched(false, trapPlayer);
					}
				}
				doorTarget.Touched(false, trapPlayer);
			}
			else if (doorTarget.state == OpenCloseBehaviour.openState.CLOSED_FOREVER)
			{
				StopAllRoutinesUnlessIA();
				StartCoroutine("StartIdle");
				yield break;
			}
		}
		if (trapPlayer && !ghostMode)
		{
			AttackPlayer();
		}
		else if (targetType == NunTarget.LAST_SEEN_PLAYER_POSITION || targetType == NunTarget.PLAYER)
		{
			CancelInvoke("FixOpeningDoor");
			StartFollowingLastSeenPosition();
		}
		else if (targetType == NunTarget.NOISE)
		{
			CancelInvoke("FixOpeningDoor");
			StartFollowingHeardPosition();
		}
		else if (targetType == NunTarget.RANDOM)
		{
			if (oppositePosition != null && Random.value > 0.5f)
			{
				Vector3 op = oppositePosition.position;
				op.y = base.transform.position.y;
				yield return new WaitForSeconds(0.3f);
				anim.CrossFade("walk");
				animState = ZombieAnimation.WALK;
				yield return base.transform.DOMove(op, Vector3.Distance(openDoorPosition, oppositePosition.position) * 1f).WaitForCompletion();
				anim.CrossFade("idle1");
				animState = ZombieAnimation.IDLE;
				yield return new WaitForSeconds(0.5f);
				if (doorTarget.state != OpenCloseBehaviour.openState.OPENED)
				{
					yield break;
				}
				yield return base.transform.DORotate(Quaternion.LookRotation(oppositePosition.forward).eulerAngles, 0.3f).WaitForCompletion();
				anim.CrossFade("open");
				// The "open" clip is absent from the nun's Animation in this export, so
				// anim["open"] is null and CrossFade no-ops. Guard the length access to
				// avoid the NullReferenceException logged in OpeningDoor.
				yield return new WaitForSeconds((anim["open"] != null) ? anim["open"].length : 0.5f);
				if (doorTarget.multipleDoors.Length > 0)
				{
					TouchableElement[] multipleDoors2 = doorTarget.multipleDoors;
					for (int j = 0; j < multipleDoors2.Length; j++)
					{
						OpenCloseBehaviour openCloseBehaviour2 = (OpenCloseBehaviour)multipleDoors2[j];
						openCloseBehaviour2.Touched(false, trapPlayer);
					}
				}
				doorTarget.Touched(false);
				yield return new WaitForSeconds(0.3f);
				ContinueRandomWay();
				CancelInvoke("FixOpeningDoor");
			}
			else
			{
				CancelInvoke("FixOpeningDoor");
				ContinueRandomWay();
			}
		}
		else
		{
			CancelInvoke("FixOpeningDoor");
			ContinueRandomWay();
		}
	}

	public IEnumerator HeardNoiseRoutine()
	{
		base.transform.DOKill();
		state = NunState.HEAR_NOISE;
		targetType = NunTarget.NOISE;
		shouldMove = false;
		needMove = false;
		if (noiseDistanceToNunAtNoiseMoment < 5f)
		{
			if (!IsInFOV())
			{
				if (animState != ZombieAnimation.CRAWL)
				{
					anim.CrossFade("idle1");
					animState = ZombieAnimation.IDLE;
				}
				yield return new WaitForSeconds(1f);
			}
			else
			{
				yield return null;
			}
		}
		Vector3 positionNor = lastNoisePosition;
		positionNor.y = base.transform.position.y;
		base.transform.DORotate(Quaternion.LookRotation(positionNor - base.transform.position).eulerAngles, 0.2f);
		yield return new WaitForSeconds(0.3f);
		StartFollowingHeardPosition();
	}

	public IEnumerator StartIdle()
	{
		shouldMove = false;
		needMove = false;
		state = NunState.IDLE;
		float waitTime = 0.3f;
		if (animState != ZombieAnimation.CRAWL && animState != ZombieAnimation.IDLE)
		{
			waitTime = CrossFadeIdle();
			animState = ZombieAnimation.IDLE;
		}
		if (targetType == NunTarget.RANDOM && !MasterAudio.IsSoundGroupPlaying("tarareo") && !MasterAudio.IsSoundGroupPlaying("susurrar") && !MasterAudio.IsSoundGroupPlaying("whisper"))
		{
			if (Random.value > 0.75f)
			{
				if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
				{
					MasterAudio.PlaySound3DFollowTransform("tarareo", eyesTrans);
				}
			}
			else if (Random.value > 0.75f && Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
			{
				MasterAudio.PlaySound3DFollowTransform("susurrar", eyesTrans);
			}
		}
		yield return new WaitForSeconds(waitTime);
		if (Random.value > 0.7f)
		{
			StartCoroutine("StartIdle");
		}
		else if (Random.value < (float)SkeletonBehaviour.instance.items * 0.1f)
		{
			GoToNearestRoomFromPlayer();
		}
		else if (Random.value > 0.7f)
		{
			GoToComunZone(false);
		}
		else
		{
			GoToComunZone(true);
		}
	}

	[ContextMenu("PlayTarareoForTesting")]
	public void PlayTarareoForTesting()
	{
		if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DFollowTransform("tarareo", eyesTrans);
		}
	}

	public IEnumerator StartPraying()
	{
		shouldMove = false;
		needMove = false;
		state = NunState.PRAYING;
		canMove = false;
		anim.CrossFade("start_pray");
		anim.CrossFadeQueued("idle_pray");
		Vector3 p = prayPosition.position;
		p.y = base.transform.position.y;
		ShortcutExtensions.DORotate(endValue: Quaternion.LookRotation(prayPosition.forward).eulerAngles, target: base.transform, duration: 0.3f);
		yield return new WaitForSeconds(1f);
		MasterAudio.StopAllSoundsOfTransform(base.transform);
		if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DAtTransform("pray", base.transform);
		}
		MessagesManager.instance.ShowMessage("nun_start_pray", "30");
		devilNote.SetActive(true);
		prayed = true;
		string ncode = Random.Range(0, 9).ToString() + Random.Range(0, 9) + Random.Range(0, 9) + Random.Range(0, 9);
		codeBehaviour.SetCode(ncode);
		devilNoteTxt.text = ncode;
		yield return new WaitForSeconds(32f);
		canMove = true;
		StartCoroutine("StartIdle");
	}

	public IEnumerator StartCrawl()
	{
		shouldMove = false;
		needMove = false;
		CrossFadeIdle();
		animState = ZombieAnimation.CRAWL;
		yield return new WaitForSeconds(0.3f);
		anim.CrossFade("crawl");
		nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.3f, nunModel.localPosition.z);
		UpdateStandUp(false);
		yield return new WaitForSeconds(0.7f);
		CancelInvoke("CheckCrawling");
		Invoke("CheckCrawling", 2f);
		needMove = true;
	}

	public IEnumerator EndCrawl()
	{
		UpdateStandUp(true);
		animState = ZombieAnimation.NONE;
		StartCoroutine("StartIdle");
		yield return null;
	}

	public IEnumerator Attacking()
	{
		CancelInvoke();
		canMove = false;
		if (WindowBehaviour.instance.isInside)
		{
			WindowBehaviour.instance.Touched();
		}
		PlayersManager.instance.GetCurrentController().OnFinishPersecution();
		MasterAudio.PlaySound("jump_scare");
		MasterAudio.PlaySound("scream");
		MessagesManager.instance.HideMessage();
		shouldMove = false;
		needMove = false;
		PlayersManager.instance.GetCurrentController().dead = true;
		PlayersManager.instance.GetCurrentController().BlockPlayer();
		PlayersManager.instance.GetCurrentController().GetComponent<PlayerMovement>().enabled = false;
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.ReleaseOnDied();
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
		}
		SimpleSmoothMouseLook.instance.StartLookingNun();
		if (animState != ZombieAnimation.CRAWL)
		{
			anim.CrossFade("idle1");
			nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.85f, nunModel.localPosition.z);
		}
		Vector3 d = PlayersManager.instance.GetCurrentController().transform.position;
		d.y = base.transform.position.y;
		base.transform.LookAt(d);
		yield return new WaitForSeconds(0.5f);
		if (PlayersManager.instance.GetCurrentController().isHidden && PlayersManager.instance.GetCurrentController().currentHide.door == null)
		{
			destroyOnAttack = true;
			Vector3 worldPosition = ((FastHide)PlayersManager.instance.GetCurrentController().currentHide).camPosition.position;
			worldPosition.y = base.transform.position.y;
			base.transform.LookAt(worldPosition);
		}
		if (animState != ZombieAnimation.CRAWL)
		{
			anim.CrossFade("attack");
			yield return new WaitForSeconds(anim["attack"].length);
		}
		else
		{
			anim.CrossFade("crawl_attack");
			yield return new WaitForSeconds(anim["crawl_attack"].length);
		}
		if (animState != ZombieAnimation.CRAWL)
		{
			anim.CrossFade("idle1");
		}
		else
		{
			anim.CrossFade("crawl_idle");
		}
		if (!destroyOnAttack)
		{
			if (animState != ZombieAnimation.CRAWL)
			{
				anim.CrossFade("idle1");
				yield return new WaitForSeconds(1f);
				Invoke("Reset", 2f);
			}
			else
			{
				anim.CrossFade("crawl_idle");
				Invoke("Reset", 0.2f);
			}
		}
	}

	public IEnumerator OnSeenPlayer()
	{
		StopPlayingClip();
		StopAllRoutinesUnlessIA();
		shouldMove = false;
		needMove = false;
		base.transform.DOKill();
		state = NunState.SEEN_PLAYER;
		if (distanceToPlayer < 1f && canSeePlayer)
		{
			AttackPlayer();
			yield break;
		}
		if (Time.time - timerLostSeenPlayer > 4f)
		{
			if (IsInPlayersFOV() && animState != ZombieAnimation.CRAWL)
			{
				anim.CrossFade("idle1");
				animState = ZombieAnimation.IDLE;
				yield return new WaitForSeconds(0.5f);
			}
			needMove = true;
		}
		yield return null;
		timerLostSeenPlayer = Time.time;
		StartFollowingPlayer();
	}

	public IEnumerator NoiseBehaviour()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			currentNoiseDistance -= 0.5f;
			currentNoiseDistance = Mathf.Clamp(currentNoiseDistance, 0f, 50f);
		}
	}

	public void RecoverFromRagDollNow()
	{
		if (((NunKeyBehaviour)strongboxKey).hasNun)
		{
			strongboxKey.transform.parent = null;
			strongboxKey.rb.isKinematic = false;
			strongboxKey.interactuable = true;
			((NunKeyBehaviour)strongboxKey).KeyReleased();
		}
		nunModel.transform.parent = base.transform;
		controller.enabled = true;
		foreach (RagDollElement ragdollJoint in ragdollJoints)
		{
			ragdollJoint.rb.transform.localEulerAngles = ragdollJoint.localeuler;
			ragdollJoint.rb.transform.localPosition = ragdollJoint.localpos;
			ragdollJoint.rb.isKinematic = true;
		}
		nunModel.transform.localPosition = new Vector3(nunModel.localPosition.x, -0.86f, nunModel.localPosition.z);
	}

	private new void Start()
	{
		Invoke("ResetNun", 0.1f);
		if (ghostMode)
		{
			AdsManager.instance.OnStartGhostMode();
		}
	}

	public void TryToSetTargetPosition(Vector3 p)
	{
		if (IsWay(GetFeetPosition(), p))
		{
			base.destination = p;
			return;
		}
		Vector3 nearestPointToTarget = GetNearestPointToTarget(p);
		if (nearestPointToTarget != Vector3.zero)
		{
			if (IsWay(GetFeetPosition(), p))
			{
				base.destination = p;
			}
			else
			{
				StartCoroutine("StartIdle");
			}
			return;
		}
		gizmoPosition = p;
		if (state != NunState.IDLE)
		{
			StartCoroutine("StartIdle");
		}
	}

	public Vector3 GetNearestPointToTarget(Vector3 p)
	{
		NNInfo nearest = AstarPath.active.GetNearest(p);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				return Vector3.zero;
			}
			if (Vector3.Distance(nearest.position, p) < 0.6f)
			{
				return nearest.position;
			}
			return Vector3.zero;
		}
		return Vector3.zero;
	}

	public IEnumerator DoingIA()
	{
		yield return new WaitForSeconds(0.1f);
		while (true)
		{
			if (secondaryState != NunState.ON_GUM_HEAD && !TutorialController.instance.isEnabled)
			{
				IA();
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void GoToComunZone(bool random)
	{
		PositionData positionData = null;
		positionData = (random ? PositionsController.instance.GetRandomComunZoneTo(base.transform.position) : PositionsController.instance.GetComunZoneTo(base.transform.position));
		Transform transform = positionData.points[Random.Range(0, positionData.points.Count)];
		NNInfo nearest = AstarPath.active.GetNearest(transform.position);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				GoToComunZone(true);
				return;
			}
			StartRandomWay();
			currentRandomPoint = transform;
			TryToSetTargetPosition(currentRandomPoint.position);
		}
	}

	public void StartRandomWay()
	{
		StopAllRoutinesUnlessIA();
		if (!MasterAudio.IsSoundGroupPlaying("tarareo") && !MasterAudio.IsSoundGroupPlaying("susurrar") && !MasterAudio.IsSoundGroupPlaying("whisper") && Random.value > 0.75f && Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DFollowTransform("tarareo", base.transform);
		}
		StartWalking();
		targetType = NunTarget.RANDOM;
		state = NunState.RANDOM_WAY;
	}

	public void ContinueRandomWay()
	{
		StopAllRoutinesUnlessIA();
		if (!MasterAudio.IsSoundGroupPlaying("tarareo") && !MasterAudio.IsSoundGroupPlaying("susurrar") && !MasterAudio.IsSoundGroupPlaying("whisper") && Random.value > 0.75f && Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DFollowTransform("tarareo", base.transform);
		}
		StartWalking();
		targetType = NunTarget.RANDOM;
		state = NunState.RANDOM_WAY;
	}

	public void GotoNearRoom()
	{
		PositionData nearRoomTo = PositionsController.instance.GetNearRoomTo(lastNoisePosition);
		if (nearRoomTo.place == PositionPlace.START_ROOM && initialTrapDoor.used)
		{
			currentRandomPoint = initialTrapDoor.trans;
			TryToSetTargetPosition(currentRandomPoint.position);
			StartRandomWay();
			return;
		}
		Transform transform = nearRoomTo.points[Random.Range(0, nearRoomTo.points.Count)];
		NNInfo nearest = AstarPath.active.GetNearest(transform.position);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				GoToComunZone(true);
				return;
			}
			StartRandomWay();
			currentRandomPoint = transform;
			TryToSetTargetPosition(currentRandomPoint.position);
		}
	}

	public void GoToNearestRoomToNun()
	{
		PositionData nearRoomTo = PositionsController.instance.GetNearRoomTo(base.transform.position);
		if (nearRoomTo.place == PositionPlace.START_ROOM && Random.value > 0.5f)
		{
			GoToNearestRoomToNun();
			return;
		}
		if (nearRoomTo.place == PositionPlace.START_ROOM && initialTrapDoor.used)
		{
			currentRandomPoint = initialTrapDoor.trans;
			TryToSetTargetPosition(currentRandomPoint.position);
			StartRandomWay();
			return;
		}
		Transform transform = nearRoomTo.points[Random.Range(0, nearRoomTo.points.Count)];
		NNInfo nearest = AstarPath.active.GetNearest(transform.position);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				GoToComunZone(true);
				return;
			}
			StartRandomWay();
			currentRandomPoint = transform;
			TryToSetTargetPosition(currentRandomPoint.position);
		}
	}

	public void GoToNearestRoomFromLastSeenPosition()
	{
		PositionData nearRoomTo = PositionsController.instance.GetNearRoomTo(targetPosition);
		if (nearRoomTo.place == PositionPlace.START_ROOM && initialTrapDoor.used)
		{
			currentRandomPoint = initialTrapDoor.trans;
			TryToSetTargetPosition(currentRandomPoint.position);
			StartRandomWay();
			return;
		}
		Transform transform = nearRoomTo.points[Random.Range(0, nearRoomTo.points.Count)];
		NNInfo nearest = AstarPath.active.GetNearest(transform.position);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				GoToComunZone(true);
				return;
			}
			StartRandomWay();
			currentRandomPoint = transform;
			TryToSetTargetPosition(currentRandomPoint.position);
		}
	}

	public void GoToNearestRoomFromPlayer()
	{
		PositionData nearRoomTo = PositionsController.instance.GetNearRoomTo(PlayersManager.instance.GetCurrentController().transform.position);
		Transform transform = nearRoomTo.points[Random.Range(0, nearRoomTo.points.Count)];
		NNInfo nearest = AstarPath.active.GetNearest(transform.position);
		if (nearest.node != null)
		{
			if (!nearest.node.Walkable)
			{
				GoToComunZone(true);
				return;
			}
			StartRandomWay();
			currentRandomPoint = transform;
			TryToSetTargetPosition(currentRandomPoint.position);
		}
	}

	public void OnUsedDoor(OpenCloseBehaviour item)
	{
		if (state != NunState.ON_GUM_FEET && state != NunState.ON_GUM_HEAD && !blocked && state != NunState.DEAD)
		{
			if (item.type == objectType.DOOR && GetDistanceBetween(base.transform.position, item.trans.position) < 2.5f)
			{
				targetType = NunTarget.LAST_SEEN_PLAYER_POSITION;
				doorTarget = item;
				targetPosition = PlayersManager.instance.GetCurrentController().feetPosition.position;
				StartGoToDoor();
			}
			else
			{
				float dist = Random.Range(item.minHearDistance, item.maxHearDistance);
				AddNoise(item.GetNoisePosition(), dist, string.Empty);
				lastNoiseType = item.type;
			}
		}
	}

	public void OnDidNoiseWithObject(Vector3 n, float dist)
	{
		lastNoiseType = objectType.TAKEABLE;
		if (GetDistanceBetween(base.transform.position, n) > 0.8f)
		{
			AddNoise(n, dist, string.Empty);
		}
	}

	public void AddNoise(Vector3 n, float dist, string noiseName = "")
	{
		if (TutorialController.instance.isEnabled || state == NunState.ON_GUM_FEET || secondaryState == NunState.ON_GUM_HEAD)
		{
			return;
		}
		bool flag = false;
		if (Vector3.Distance(lastNoisePosition, n) > 5f)
		{
			currentNoiseDistance = 0f;
		}
		if (state == NunState.RANDOM_WAY || state == NunState.IDLE || state == NunState.FOLLOWING_LAST_HEARD_POSITION || state == NunState.LOOKING_AROUND || state == NunState.HEAR_NOISE || state == NunState.GO_TO_DOOR || state == NunState.OPENING_DOOR)
		{
			flag = true;
		}
		if (Time.time - timerLostSeenPlayer > 1f && state == NunState.FOLLOWING_LAST_SEEN_POSITION)
		{
			flag = true;
		}
		if (flag)
		{
			if (VariablesGlobales.difficultyMode == 0)
			{
				currentNoiseDistance += dist / 2f;
			}
			else if (VariablesGlobales.difficultyMode == 1)
			{
				currentNoiseDistance += dist;
			}
			else
			{
				currentNoiseDistance += dist * 2f;
			}
			noiseDistanceToNunAtNoiseMoment = GetDistanceBetween(n, base.transform.position);
			lastNoisePosition = n;
			float distanceBetween = GetDistanceBetween(lastNoisePosition, base.transform.position);
			if (currentNoiseDistance > distanceBetween && animState != ZombieAnimation.CRAWL && targetType != NunTarget.NOISE)
			{
				StopAllRoutinesUnlessIA();
				StartCoroutine("HeardNoiseRoutine");
			}
		}
	}

	public void StopPlayingClip()
	{
		MasterAudio.StopSoundGroupOfTransform(base.transform, "tarareo");
		MasterAudio.StopSoundGroupOfTransform(base.transform, "susurrar");
	}

	public bool IsNearDoor()
	{
		return false;
	}

	public void NormalizeSpeed()
	{
		if (state == NunState.ON_GUM_FEET)
		{
			maxSpeed = 0f;
		}
		else if (animState == ZombieAnimation.RUN)
		{
			if (VariablesGlobales.difficultyMode == 1)
			{
				maxSpeed = 2.4f;
			}
			else
			{
				maxSpeed = 2.7f;
			}
		}
		else if (animState == ZombieAnimation.WALK)
		{
			if (VariablesGlobales.difficultyMode == 0 && (state == NunState.FOLLOWING_PLAYER || state == NunState.FOLLOWING_LAST_SEEN_POSITION))
			{
				maxSpeed = 1.8f;
				anim["walk"].speed = 1.3f;
			}
			else
			{
				anim["walk"].speed = 1f;
				maxSpeed = 1.4f;
			}
		}
		else if (animState == ZombieAnimation.CRAWL)
		{
			maxSpeed = 1f;
		}
	}

	public void UpdateSpeed()
	{
		if (!shouldMove || state == NunState.ON_GUM_FEET)
		{
			maxSpeed = 0f;
		}
		else if (!canSeePlayer && GetDistanceBetween(base.transform.position, PlayersManager.instance.GetCurrentController().transform.position) > 8f && state == NunState.FOLLOWING_LAST_HEARD_POSITION)
		{
			maxSpeed = 4f;
		}
		else
		{
			NormalizeSpeed();
		}
	}

	public void BlockNun()
	{
		beforePlayerDiedState = state;
		recoverNunEffect.SetActive(false);
		StopAllCoroutines();
		state = NunState.NONE;
		shouldMove = false;
		needMove = false;
		canMove = false;
	}

	public void IA()
	{
		if (state == NunState.PRAYING || state == NunState.ATTACK || state == NunState.DEAD || blocked)
		{
			return;
		}
		if (PlayersManager.instance == null || PlayersManager.instance.GetCurrentController() == null || PlayersManager.instance.GetCurrentController().feetPosition == null)
		{
			return;
		}
		distanceToPlayer = GetDistanceBetween(PlayersManager.instance.GetCurrentController().feetPosition.position, GetFeetPosition());
		UpdateSpeed();
		if (!canSeePlayer && state != NunState.SEEN_PLAYER && state != NunState.ON_GUM_FEET && state != NunState.ON_GUM_HEAD && state != NunState.OPENING_DOOR && state != NunState.LOOKING_AROUND && state != NunState.LOOKING_DOLL && state != NunState.IDLE)
		{
			CheckDoorInNextNodes();
		}
		if (animState != ZombieAnimation.CRAWL && state != NunState.DEAD && state != NunState.ON_GUM_FEET)
		{
			CheckNeedCrawl(1f);
		}
		RaycastHit hitInfo;
		if (PlayersManager.instance.GetCurrentController().isHidden || ghostMode)
		{
			canSeePlayer = false;
			if (ghostMode && PlayersManager.instance.isBaby)
			{
				bool flag = false;
				Vector3 vector = ((!standup) ? base.transform.position : (base.transform.position + Vector3.up * 0.7f));
				Vector3 direction = PlayersManager.instance.GetCurrentController().transform.position - vector;
				if (Physics.Raycast(vector, direction, out hitInfo, distanceToSeePlayer, layersToCheckifSeePlayer) && hitInfo.collider.tag == "Player")
				{
					flag = true;
				}
				if (state != NunState.LOOKING_DOLL)
				{
					if (GetDistanceBetween(PlayersManager.instance.GetCurrentController().transform.position, base.transform.position) < 5f && flag)
					{
						StartLookingDoll();
					}
				}
				else if (GetDistanceBetween(PlayersManager.instance.GetCurrentController().transform.position, base.transform.position) > 6f || !flag)
				{
					StopLookingDoll();
				}
			}
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Vector3 vector2 = ((!standup) ? base.transform.position : (base.transform.position + Vector3.up * 0.7f));
			zero = ((!PlayersManager.instance.isBaby) ? (PlayersManager.instance.GetCurrentController().eyesTrans.position - Vector3.up * 0.2f - vector2) : (PlayersManager.instance.GetCurrentController().transform.position - vector2));
			if (Physics.Raycast(vector2, zero, out hitInfo, distanceToSeePlayer, layersToCheckifSeePlayer))
			{
				if (hitInfo.collider.tag == "Player")
				{
					canSeePlayer = true;
				}
				else
				{
					canSeePlayer = false;
				}
			}
			else
			{
				canSeePlayer = false;
			}
			if (PlayersManager.instance.isBaby)
			{
				if (state != NunState.LOOKING_DOLL)
				{
					if (GetDistanceBetween(PlayersManager.instance.GetCurrentController().transform.position, base.transform.position) < 5f && canSeePlayer)
					{
						StartLookingDoll();
					}
				}
				else if (GetDistanceBetween(PlayersManager.instance.GetCurrentController().transform.position, base.transform.position) > 6f || !canSeePlayer)
				{
					StopLookingDoll();
				}
				canSeePlayer = false;
			}
		}
		if (!ghostMode && state != NunState.ON_GUM_FEET)
		{
			if (IsInFOV())
			{
				if (canSeePlayer)
				{
					if (state != NunState.SEEN_PLAYER && state != NunState.FOLLOWING_PLAYER)
					{
						if (IsInPlayersFOV() && Time.time - timerLostSeenPlayer > 5f)
						{
							StartCoroutine("OnSeenPlayer");
						}
						else
						{
							StartFollowingPlayer();
						}
					}
				}
				else if (state == NunState.FOLLOWING_PLAYER && Time.time - timerLostSeenPlayer > 2.5f)
				{
					StartFollowingLastSeenPosition();
				}
			}
			else if (distanceToPlayer < 2f && canSeePlayer && !PlayersManager.instance.isBaby && state != NunState.SEEN_PLAYER && state != NunState.FOLLOWING_PLAYER)
			{
				if (IsInPlayersFOV() && Time.time - timerLostSeenPlayer > 5f)
				{
					StartCoroutine("OnSeenPlayer");
				}
				else
				{
					StartFollowingPlayer();
				}
			}
		}
		switch (state)
		{
		case NunState.FOLLOWING_PLAYER:
			TryToSetTargetPosition(PlayersManager.instance.GetCurrentController().feetPosition.position);
			if (!(distanceToPlayer < 5f))
			{
				break;
			}
			if (canSeePlayer)
			{
				if (distanceToPlayer < 1.2f && !ghostMode)
				{
					AttackPlayer();
				}
			}
			else if (PlayersManager.instance.GetCurrentController().isHidden && sawPlayerHiding)
			{
				if (PlayersManager.instance.GetCurrentController().currentHide.door != null)
				{
					doorTarget = PlayersManager.instance.GetCurrentController().currentHide.door;
					state = NunState.GO_TO_DOOR;
				}
				else
				{
					currentHidePositionToDiscover = ((FastHide)PlayersManager.instance.GetCurrentController().currentHide).nunPositionDiscover;
					TryToSetTargetPosition(currentHidePositionToDiscover.position);
					state = NunState.GO_TO_HIDE;
				}
			}
			else if (!PlayersManager.instance.GetCurrentController().standup && distanceToPlayer < 0.7f && !ghostMode)
			{
				AttackPlayer();
			}
			break;
		case NunState.GO_TO_DOOR:
			openDoorPosition = doorTarget.GetNearestEnterPosition(GetFeetPosition()).position;
			TryToSetTargetPosition(openDoorPosition);
			if (GetDistanceBetween(openDoorPosition, GetFeetPosition()) < 0.3f)
			{
				StopAllRoutinesUnlessIA();
				StartCoroutine("OpeningDoor");
			}
			break;
		case NunState.GO_TO_PRAY:
			TryToSetTargetPosition(prayPosition.position);
			if (GetDistanceBetween(prayPosition.position, GetFeetPosition()) < 0.3f)
			{
				StopAllRoutinesUnlessIA();
				StartCoroutine("StartPraying");
			}
			break;
		case NunState.ON_GUM_FEET:
			if (ghostMode)
			{
				break;
			}
			if (canSeePlayer && IsInFOV())
			{
				if (distanceToPlayer < 1.2f)
				{
					AttackPlayer();
				}
			}
			else if (canSeePlayer && distanceToPlayer < 0.8f)
			{
				AttackPlayer();
			}
			break;
		case NunState.GO_TO_HIDE:
			TryToSetTargetPosition(currentHidePositionToDiscover.position);
			if (GetDistanceBetween(currentHidePositionToDiscover.position, GetFeetPosition()) < 0.8f && PlayersManager.instance.GetCurrentController().isHidden)
			{
				AttackPlayer(true);
			}
			break;
		case NunState.FOLLOWING_LAST_SEEN_POSITION:
		{
			TryToSetTargetPosition(targetPosition);
			float distanceBetween = GetDistanceBetween(targetPosition, GetFeetPosition());
			if (PlayersManager.instance.GetCurrentController().isHidden && sawPlayerHiding)
			{
				if (PlayersManager.instance.GetCurrentController().currentHide.door != null)
				{
					doorTarget = PlayersManager.instance.GetCurrentController().currentHide.door;
					state = NunState.GO_TO_DOOR;
				}
				else
				{
					currentHidePositionToDiscover = ((FastHide)PlayersManager.instance.GetCurrentController().currentHide).nunPositionDiscover;
					TryToSetTargetPosition(currentHidePositionToDiscover.position);
					state = NunState.GO_TO_HIDE;
				}
			}
			else
			{
				if (!(distanceBetween < 1f))
				{
					break;
				}
				if (distanceToPlayer < 1.5f)
				{
					if (Time.time - timerLostSeenPlayer < 2f)
					{
						if (PlayersManager.instance.GetCurrentController().isHidden)
						{
							StopAllRoutinesUnlessIA();
							StartCoroutine("StartLookingAround");
						}
						else
						{
							targetPosition = PlayersManager.instance.GetCurrentController().feetPosition.position;
							TryToSetTargetPosition(targetPosition);
						}
					}
					else if (animState != ZombieAnimation.CRAWL)
					{
						StopAllRoutinesUnlessIA();
						StartCoroutine("StartLookingAround");
					}
					else
					{
						GoToNearestRoomFromLastSeenPosition();
					}
				}
				else if (Time.time - timerLostSeenPlayer < 5f)
				{
					GoToNearestRoomFromLastSeenPosition();
				}
				else if (Random.value < 0.7f)
				{
					GoToNearestRoomFromLastSeenPosition();
				}
				else
				{
					StopAllRoutinesUnlessIA();
					StartCoroutine("StartLookingAround");
				}
			}
			break;
		}
		case NunState.FOLLOWING_LAST_HEARD_POSITION:
		{
			TryToSetTargetPosition(lastNoisePosition);
			float distanceBetween2 = GetDistanceBetween(lastNoisePosition, base.transform.position);
			if (PlayersManager.instance.GetCurrentController().isHidden)
			{
				if (distanceBetween2 < 1.2f)
				{
					StopAllRoutinesUnlessIA();
					StartCoroutine("StartLookingAround");
				}
			}
			else if (noiseDistanceToNunAtNoiseMoment < 1f && !PlayersManager.instance.GetCurrentController().standup && distanceToPlayer < 1.2f)
			{
				if (!ghostMode)
				{
					AttackPlayer();
				}
			}
			else if (distanceBetween2 < 1.2f)
			{
				GotoNearRoom();
			}
			break;
		}
		case NunState.RANDOM_WAY:
			TryToSetTargetPosition(currentRandomPoint.transform.position);
			if (animState != ZombieAnimation.CRAWL)
			{
				if (GetDistanceBetween(currentRandomPoint.transform.position, GetFeetPosition()) < 0.5f)
				{
					StartCoroutine("StartIdle");
				}
			}
			else if (GetDistanceBetween(currentRandomPoint.transform.position, GetFeetPosition()) < 1f)
			{
				StartCoroutine("StartIdle");
			}
			break;
		case NunState.IDLE:
			break;
		case NunState.LOOKING_AROUND:
			break;
		case NunState.ATTACK:
			break;
		case NunState.SEEN_PLAYER:
		case NunState.PRAYING:
		case NunState.OPENING_DOOR:
		case NunState.HEAR_NOISE:
		case NunState.DEAD:
		case NunState.LOOKING_DOLL:
		case NunState.ON_GUM_HEAD:
			break;
		}
	}

	public bool IsWay(Vector3 from, Vector3 to)
	{
		if (AstarPath.active != null)
		{
			GraphNode node = AstarPath.active.GetNearest(from, NNConstraint.Default).node;
			GraphNode node2 = AstarPath.active.GetNearest(to, NNConstraint.Default).node;
			if (PathUtilities.IsPathPossible(node, node2))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void StartFollowingHeardPosition()
	{
		if (state != NunState.ON_GUM_FEET && secondaryState != NunState.ON_GUM_HEAD)
		{
			base.transform.DOKill();
			StopPlayingClip();
			state = NunState.FOLLOWING_LAST_HEARD_POSITION;
			targetType = NunTarget.NOISE;
			if (Time.time - timerLostSeenPlayer < 6f || noiseDistanceToNunAtNoiseMoment < 8f || currentNoiseDistance > 15f || desiredAnimState == ZombieAnimation.RUN)
			{
				StartRunning();
			}
			else
			{
				StartWalking();
			}
		}
	}

	public void StartFollowingPlayer()
	{
		if (state != NunState.ON_GUM_FEET && secondaryState != NunState.ON_GUM_HEAD)
		{
			base.transform.DOKill();
			StopPlayingClip();
			StopAllRoutinesUnlessIA();
			state = NunState.FOLLOWING_PLAYER;
			targetPosition = PlayersManager.instance.GetCurrentController().feetPosition.position;
			targetType = NunTarget.PLAYER;
			StartRunning();
		}
	}

	public void StartFollowingLastSeenPosition()
	{
		if (state != NunState.ON_GUM_FEET && secondaryState != NunState.ON_GUM_HEAD)
		{
			base.transform.DOKill();
			StopPlayingClip();
			StopAllRoutinesUnlessIA();
			state = NunState.FOLLOWING_LAST_SEEN_POSITION;
			targetPosition = PlayersManager.instance.GetCurrentController().feetPosition.position;
			targetType = NunTarget.LAST_SEEN_PLAYER_POSITION;
			StartRunning();
		}
	}

	public float CrossFadeIdle()
	{
		string animation = "idle" + Random.Range(1, 4);
		if (animState == ZombieAnimation.CRAWL)
		{
			animation = "crawl_idle";
			nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.3f, nunModel.localPosition.z);
		}
		else
		{
			nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.86f, nunModel.localPosition.z);
		}
		if (animState != ZombieAnimation.IDLE)
		{
			anim.CrossFade(animation);
		}
		return anim[animation].length;
	}

	public void FixOpeningDoor()
	{
		if (state == NunState.OPENING_DOOR)
		{
			StartCoroutine("StartIdle");
		}
	}

	public void CheckDoorInNextNodes()
	{
		if (path == null || path.vectorPath.Count <= 0)
		{
			return;
		}
		Gizmos.color = Color.white;
		int num = ((path.vectorPath.Count >= 3) ? 3 : path.vectorPath.Count);
		for (int i = 1; i < num; i++)
		{
			Vector3 vector = path.vectorPath[i] + Vector3.up * 0.3f;
			Vector3 vector2 = path.vectorPath[i - 1] + Vector3.up * 0.3f;
			Vector3 direction = vector - vector2;
			RaycastHit hitInfo;
			if (Physics.Raycast(vector2, direction, out hitInfo, 2f, layersToCheckDoors) && (bool)hitInfo.collider.gameObject.GetComponent<OpenCloseBehaviour>())
			{
				OpenCloseBehaviour component = hitInfo.collider.gameObject.GetComponent<OpenCloseBehaviour>();
				if ((component.state == OpenCloseBehaviour.openState.CLOSED || component.state == OpenCloseBehaviour.openState.CLOSED_BY_KEY) && component.type == objectType.DOOR)
				{
					doorTarget = component;
					StartGoToDoor();
					break;
				}
			}
		}
	}

	public void StartGoToDoor()
	{
		StopAllRoutinesUnlessIA();
		if (animState != ZombieAnimation.WALK && animState != ZombieAnimation.RUN && animState != ZombieAnimation.CRAWL)
		{
			if (state == NunState.FOLLOWING_LAST_SEEN_POSITION || state == NunState.FOLLOWING_PLAYER || (state == NunState.FOLLOWING_LAST_HEARD_POSITION && Time.time - timerLostSeenPlayer < 5f))
			{
				StartRunning();
			}
			else
			{
				StartWalking();
			}
		}
		needMove = true;
		state = NunState.GO_TO_DOOR;
	}

	public void CheckNeedCrawl(float distance)
	{
		if (animState != ZombieAnimation.CRAWL)
		{
			Vector3 vector = base.transform.position + base.transform.forward;
			Vector3 vector2 = base.transform.position + Vector3.up * 1f;
			Vector3 direction = vector - vector2;
			RaycastHit hitInfo;
			if (Physics.Raycast(vector2, direction, out hitInfo, distance, layersToCheckCrawlEntrance) && hitInfo.collider.tag == "CrawlEntrance")
			{
				StopAllRoutinesUnlessIA();
				StartCoroutine("StartCrawl");
				animState = ZombieAnimation.CRAWL;
			}
		}
	}

	private new void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		if (path != null && path.vectorPath.Count > 0)
		{
			foreach (Vector3 item in path.vectorPath)
			{
				Gizmos.DrawSphere(item, 0.15f);
			}
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(targetPosition, 0.3f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(openDoorPosition, 0.3f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(lastNoisePosition, 0.3f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(gizmoPosition, 0.5f);
	}

	public void UpdateStandUp(bool set)
	{
		if (controller == null) controller = GetComponent<CharacterController>();
		if (controller == null || feetPosition == null) return;
		if (standup != set)
		{
			standup = set;
			if (standup)
			{
				controller.height = 1.6f;
				base.transform.position += Vector3.up * 0.5f;
				feetPosition.localPosition = new Vector3(0f, -0.75f, 0f);
			}
			else
			{
				controller.height = 0.4f;
				base.transform.position -= Vector3.up * 0.5f;
				feetPosition.localPosition = new Vector3(0f, 0f, 0f);
			}
		}
	}

	public bool IsInFOV()
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		if (state == NunState.LOOKING_AROUND || state == NunState.IDLE)
		{
			zero = PlayersManager.instance.GetCurrentController().transform.position - eyesTrans.position;
			num = Vector3.Angle(zero, eyesTrans.forward);
		}
		else
		{
			zero = PlayersManager.instance.GetCurrentController().transform.position - base.transform.position;
			num = Vector3.Angle(zero, base.transform.forward);
		}
		if (num >= -90f && num <= 90f)
		{
			isInFov = true;
			return true;
		}
		isInFov = false;
		return false;
	}

	public bool IsInPlayersFOV()
	{
		Vector3 vector = base.transform.position - PlayersManager.instance.GetCurrentController().transform.position;
		float num = Vector3.Angle(vector, PlayersManager.instance.GetCurrentController().transform.forward);
		if (num >= -70f && num <= 70f)
		{
			return true;
		}
		return false;
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "PrayZone" && !prayed && !canSeePlayer && SkeletonBehaviour.instance.CanPray())
		{
			StartGoToPray();
		}
	}

	public void StartGoToPray()
	{
		StopAllRoutinesUnlessIA();
		StartWalking();
		needMove = true;
		state = NunState.GO_TO_PRAY;
		targetType = NunTarget.PRAY;
	}

	public override void OnTargetReached()
	{
		base.OnTargetReached();
	}

	protected override void OnPathComplete(Path newPath)
	{
		base.OnPathComplete(newPath);
		if (state == NunState.ON_GUM_FEET || secondaryState == NunState.ON_GUM_HEAD || !needMove)
		{
			return;
		}
		shouldMove = true;
		if (animState == ZombieAnimation.CRAWL)
		{
			return;
		}
		if (desiredAnimState == ZombieAnimation.RUN)
		{
			animState = ZombieAnimation.RUN;
			nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.86f, nunModel.localPosition.z);
			if (VariablesGlobales.difficultyMode == 1)
			{
				anim["run"].speed = 1f;
			}
			else
			{
				anim["run"].speed = 1f;
			}
			anim.CrossFade("run");
		}
		else if (desiredAnimState == ZombieAnimation.WALK)
		{
			animState = ZombieAnimation.WALK;
			nunModel.localPosition = new Vector3(nunModel.localPosition.x, -0.86f, nunModel.localPosition.z);
			anim.CrossFade("walk");
		}
	}

	public void CheckCrawling()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, base.transform.up, out hitInfo, 1f, layersToCheckCrawling))
		{
			CancelInvoke("CheckCrawling");
			Invoke("CheckCrawling", 0.5f);
		}
		else
		{
			StopAllRoutinesUnlessIA();
			StartCoroutine("EndCrawl");
		}
	}

	public bool CanStandup()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, base.transform.up, out hitInfo, 1f, layersToCheckCrawling))
		{
			return false;
		}
		return true;
	}

	public void StartLookingDoll()
	{
		StopPlayingClip();
		StopAllRoutinesUnlessIA();
		shouldMove = false;
		needMove = false;
		state = NunState.LOOKING_DOLL;
		float num = 0f;
		if (animState != ZombieAnimation.CRAWL)
		{
			num = CrossFadeIdle();
			animState = ZombieAnimation.IDLE;
		}
	}

	public void StopLookingDoll()
	{
		StopPlayingClip();
		StopAllRoutinesUnlessIA();
		shouldMove = false;
		needMove = false;
		state = NunState.IDLE;
		StartCoroutine("StartIdle");
	}

	public void StartWalking(bool force = false)
	{
		if (animState == ZombieAnimation.CRAWL)
		{
			needMove = true;
			return;
		}
		if (!shouldMove && animState != ZombieAnimation.IDLE)
		{
			anim.CrossFade("idle1");
			animState = ZombieAnimation.IDLE;
		}
		desiredAnimState = ZombieAnimation.WALK;
		needMove = true;
	}

	public void StartRunning(bool force = false)
	{
		if (animState == ZombieAnimation.CRAWL)
		{
			shouldMove = false;
			needMove = true;
			return;
		}
		if (!shouldMove && animState != ZombieAnimation.IDLE)
		{
			anim.CrossFade("idle1");
			animState = ZombieAnimation.IDLE;
		}
		if (VariablesGlobales.difficultyMode == 0)
		{
			desiredAnimState = ZombieAnimation.WALK;
		}
		else
		{
			desiredAnimState = ZombieAnimation.RUN;
		}
		needMove = true;
	}

	public void AttackPlayer(bool destroyHide = false)
	{
		if (state != NunState.ATTACK)
		{
			maxSpeed = 0f;
			state = NunState.ATTACK;
			destroyOnAttack = destroyHide;
			base.transform.DOKill();
			StopAllCoroutines();
			StartCoroutine("Attacking");
		}
	}

	public void DestroyTarget()
	{
		if (destroyOnAttack)
		{
			MasterAudio.PlaySound3DAtVector3("break_furniture", PlayersManager.instance.GetCurrentController().currentHide.transform.position);
			Object.Destroy(PlayersManager.instance.GetCurrentController().currentHide.gameObject, 0.5f);
			PlayersManager.instance.GetCurrentController().currentHide = null;
			Reset();
		}
	}

	public void Reset()
	{
		GameUIController.instance.OnResetGame();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.CompareTag("Touchable"))
		{
			return;
		}
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
				hit.collider.GetComponent<PushBehaviour>().ContactWithNun();
			}
		}
		else if (hit.collider.attachedRigidbody.velocity.magnitude < 0.3f)
		{
			attachedRigidbody.AddForceAtPosition(hit.controller.transform.forward.normalized * 10f, hit.point, ForceMode.Force);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
	}

	public void ResetNun()
	{
		destroyOnAttack = false;
		if (beforePlayerDiedState == NunState.DEAD)
		{
			RecoverFromRagDollNow();
		}
		if (gumFeet != null) gumFeet.SetActive(false);
		if (gumHead != null) gumHead.SetActive(false);
		if (gumBody != null) gumBody.SetActive(false);
		currentNoiseDistance = 0f;
		blocked = false;
		canMove = true;
		if (!cantAttack)
		{
			if (PositionsController.instance != null)
			{
				Transform randomAvailableComunPosition = PositionsController.instance.GetRandomAvailableComunPosition();
				if (randomAvailableComunPosition != null)
				{
					base.transform.position = randomAvailableComunPosition.position + Vector3.up * 0.5f;
				}
			}
		}
		if (initialTrapDoor != null) initialTrapDoor.Reset();
		state = NunState.IDLE;
		animState = ZombieAnimation.IDLE;
		StartCoroutine("StartIdle");
		StartCoroutine("DoingIA");
		StartCoroutine("NoiseBehaviour");
		if (!standup)
		{
			UpdateStandUp(true);
		}
	}

	public void RecoverNun()
	{
		state = NunState.IDLE;
		canMove = true;
		currentNoiseDistance = 0f;
		blocked = false;
		StartCoroutine("StartIdle");
		StartCoroutine("DoingIA");
		StartCoroutine("NoiseBehaviour");
	}

	public void RemoveFeetGum()
	{
		state = NunState.IDLE;
		gumFeet.SetActive(false);
		blocked = false;
		if (secondaryState == NunState.NONE)
		{
			canMove = true;
			StopAllRoutinesUnlessIA();
			StartCoroutine("StartIdle");
		}
	}

	public void AfterNunAttack()
	{
		currentNoiseDistance = 0f;
		blocked = false;
		state = NunState.IDLE;
		animState = ZombieAnimation.IDLE;
		StartCoroutine("StartIdle");
		StartCoroutine("DoingIA");
		StartCoroutine("NoiseBehaviour");
	}
}
