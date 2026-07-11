using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class RatBehaviour : MonoBehaviour
{
	public enum RatState
	{
		Idle = 0,
		Running = 1,
		Trapped = 2
	}

	public RatState state;

	public RatPathManager ratPathManager;

	public RatPath currentPath;

	public RatPathPoint currentPoint;

	private RatPathPoint lastPoint;

	public float distanceToCheckWaypoint = 0.1f;

	private float ratHeight;

	public float moveSpeed = 0.75f;

	public float rotateSpeed = 1f;

	public float alertDistance = 5f;

	public bool alertForChangePath;

	public List<string> idleAnimationNames;

	public string runAnimationName;

	public string trappedAnimationName;

	public GameObject gumObject;

	public TakeableObject maskPiece;

	public int currentPathWalkedPointsCount;

	private void Start()
	{
		ratHeight = base.transform.position.y;
		ratPathManager = RatPathManager.instance;
		if (ratPathManager == null || ratPathManager.paths == null || ratPathManager.paths.Count == 0)
		{
			Debug.LogWarning("Rat disabled because no exported rat path is available.", this);
			enabled = false;
			return;
		}
		currentPath = ratPathManager.paths[0];
		currentPoint = currentPath.GetNearestPointInPath(base.transform.position);
		lastPoint = currentPoint;
		Invoke("CheckPiece", 1f);
		StartCoroutine("Behaviour");
	}

	public void CheckPiece()
	{
		if (MaskPuzleBehaviour.instance == null || maskPiece == null)
		{
			return;
		}
		if (MaskPuzleBehaviour.instance.IsMaskPartSet(maskPiece.id))
		{
			maskPiece.gameObject.SetActive(false);
		}
		else
		{
			maskPiece.gameObject.SetActive(true);
		}
	}

	public RatPathPoint GetNextWaypointForCurrentPath()
	{
		return currentPath.GetNext(lastPoint, currentPoint);
	}

	public bool ChangePath()
	{
		if (currentPoint.paths.Count > 1)
		{
			RatPath ratPath = currentPath;
			currentPath = ratPathManager.GetBestPath(currentPoint, currentPath);
			currentPathWalkedPointsCount = 0;
			if (ratPath != currentPath)
			{
				currentPoint = currentPath.GetNearestPointInPath(base.transform.position);
				lastPoint = currentPoint;
				return true;
			}
		}
		return false;
	}

	public void ChangePathRandom()
	{
		currentPath = currentPoint.GetOtherPath(currentPath);
		currentPathWalkedPointsCount = 0;
		currentPoint = currentPath.GetNearestPointInPath(base.transform.position);
		lastPoint = currentPoint;
	}

	public void SetNextPoint()
	{
		RatPathPoint ratPathPoint = currentPoint;
		currentPoint = GetNextWaypointForCurrentPath();
		currentPathWalkedPointsCount++;
		lastPoint = ratPathPoint;
	}

	public void OnBeHitByGum()
	{
		MasterAudio.PlaySound3DFollowTransform("rat_noise", base.transform);
		ChangeState(RatState.Trapped);
	}

	public IEnumerator Behaviour()
	{
		state = RatState.Running;
		while (true)
		{
			switch (state)
			{
			case RatState.Idle:
			{
				float idleTime = Random.Range(3, 6);
				float timer = 0f;
				while (timer < idleTime)
				{
					timer += Time.deltaTime;
					UpdatePlayerAlert();
					if (alertForChangePath)
					{
						yield return new WaitForSeconds(Random.Range(0.5f, 1f));
						ChangeState(RatState.Running);
						break;
					}
					yield return null;
				}
				ChangeState(RatState.Running);
				break;
			}
			case RatState.Running:
			{
				Vector3 moveDirection = (currentPoint.transform.position - base.transform.position).normalized;
				base.transform.position += moveDirection * Time.deltaTime * moveSpeed;
				base.transform.position = new Vector3(base.transform.position.x, ratHeight, base.transform.position.z);
				base.transform.rotation = Quaternion.Lerp(Quaternion.Euler(base.transform.eulerAngles), Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed);
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				UpdatePlayerAlert();
				CheckPathPointEnd();
				break;
			}
			case RatState.Trapped:
				gumObject.SetActive(true);
				yield return new WaitForSeconds(10f);
				gumObject.SetActive(false);
				yield return new WaitForSeconds(0.25f);
				ChangeState(RatState.Running);
				break;
			}
			yield return null;
		}
	}

	public void UpdatePlayerAlert()
	{
		float num = Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - base.transform.position.y);
		if (num <= 1f && Vector3.Distance(PlayersManager.instance.GetCurrentController().transform.position, base.transform.position) < alertDistance)
		{
			alertForChangePath = true;
		}
		else
		{
			alertForChangePath = false;
		}
	}

	public void ChangeState(RatState newState)
	{
		if (newState == state)
		{
			return;
		}
		if (Random.value > 0.8f && Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - base.transform.position.y) < 2f)
		{
			MasterAudio.PlaySound3DFollowTransform("rat_noise", base.transform);
		}
		if (state == RatState.Idle)
		{
			switch (newState)
			{
			case RatState.Running:
				GetComponent<Animation>().CrossFade(runAnimationName, 0.25f);
				break;
			case RatState.Trapped:
				if (!string.IsNullOrEmpty(trappedAnimationName))
				{
					GetComponent<Animation>().CrossFade(trappedAnimationName, 0.25f);
				}
				break;
			}
		}
		else if (state == RatState.Running)
		{
			switch (newState)
			{
			case RatState.Idle:
				GetComponent<Animation>().CrossFade(idleAnimationNames[Random.Range(0, idleAnimationNames.Count)], 0.35f);
				break;
			case RatState.Trapped:
				if (!string.IsNullOrEmpty(trappedAnimationName))
				{
					GetComponent<Animation>().CrossFade(trappedAnimationName, 0.25f);
				}
				break;
			}
		}
		else if (state == RatState.Trapped)
		{
			switch (newState)
			{
			case RatState.Running:
				GetComponent<Animation>().CrossFade(runAnimationName, 0.25f);
				break;
			case RatState.Idle:
				GetComponent<Animation>().CrossFade(idleAnimationNames[Random.Range(0, idleAnimationNames.Count)], 0.35f);
				break;
			}
		}
		state = newState;
	}

	public void CheckPathPointEnd()
	{
		if (Vector3.Distance(base.transform.position, currentPoint.transform.position) <= distanceToCheckWaypoint)
		{
			bool flag = false;
			if (alertForChangePath)
			{
				flag = ChangePath();
			}
			else if (currentPoint.paths.Count > 1 && currentPoint != lastPoint && Random.value > 0.65f && (float)currentPathWalkedPointsCount > (float)currentPath.pathPoints.Count / 2f)
			{
				flag = true;
				ChangePathRandom();
			}
			else if (Random.value > 0.65f)
			{
				ChangeState(RatState.Idle);
			}
			if (!flag)
			{
				SetNextPoint();
			}
		}
	}
}
