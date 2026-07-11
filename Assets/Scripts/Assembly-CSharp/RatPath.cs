using System.Collections.Generic;
using UnityEngine;

public class RatPath : MonoBehaviour
{
	public List<RatPathPoint> pathPoints;

	public RatPathPoint centerRef;

	[HideInInspector]
	public Vector3 middlePoint;

	public Color pathColor;

	private void Awake()
	{
		Vector3 zero = Vector3.zero;
		foreach (RatPathPoint pathPoint in pathPoints)
		{
			zero += pathPoint.transform.position;
		}
		middlePoint = zero / pathPoints.Count;
	}

	public RatPathPoint GetNearestPointInPath(Vector3 p)
	{
		float num = 99999f;
		RatPathPoint result = null;
		foreach (RatPathPoint pathPoint in pathPoints)
		{
			if (Vector3.Distance(pathPoint.transform.position, p) < num)
			{
				num = Vector3.Distance(pathPoint.transform.position, p);
				result = pathPoint;
			}
		}
		return result;
	}

	public RatPathPoint GetNext(RatPathPoint last, RatPathPoint current)
	{
		int num = pathPoints.IndexOf(current);
		if (last == current)
		{
			if (num == 0)
			{
				if (Random.value > 0.5f)
				{
					return pathPoints[num + 1];
				}
				return pathPoints[pathPoints.Count - 1];
			}
			if (num == pathPoints.Count - 1)
			{
				if (Random.value > 0.5f)
				{
					return pathPoints[0];
				}
				return pathPoints[num - 1];
			}
			if (Random.value > 0.5f)
			{
				return pathPoints[num - 1];
			}
			return pathPoints[num + 1];
		}
		int num2 = pathPoints.IndexOf(last);
		bool flag = false;
		if ((num > 0 && num2 < num) || (num == 0 && num2 == pathPoints.Count - 1))
		{
			flag = true;
		}
		if (flag)
		{
			if (num < pathPoints.Count - 1)
			{
				return pathPoints[num + 1];
			}
			return pathPoints[0];
		}
		if (num == 0)
		{
			return pathPoints[pathPoints.Count - 1];
		}
		return pathPoints[num - 1];
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = pathColor;
		if (pathPoints == null)
		{
			return;
		}
		Gizmos.DrawSphere(centerRef.transform.position, 0.1f);
		foreach (RatPathPoint pathPoint in pathPoints)
		{
			if (pathPoint != null)
			{
				Gizmos.DrawSphere(pathPoint.transform.position, 0.05f);
			}
		}
	}
}
