using System.Collections.Generic;
using UnityEngine;

public class RatPathManager : MonoBehaviour
{
	public static RatPathManager instance;

	public List<RatPath> paths;

	public RatBehaviour mRat;

	private void Awake()
	{
		instance = this;
		if (paths == null) paths = new List<RatPath>();
	}

	private void Start()
	{
	}

	public RatPath GetBestPath(RatPathPoint point, RatPath path)
	{
		RatPath result = path;
		float num = Vector3.Distance(path.centerRef.transform.position, PlayersManager.instance.GetCurrentController().transform.position);
		foreach (RatPath path2 in point.paths)
		{
			float num2 = Vector3.Distance(path2.centerRef.transform.position, PlayersManager.instance.GetCurrentController().transform.position);
			if (path2 != path && num2 > num)
			{
				num = num2;
				result = path2;
			}
		}
		return result;
	}

	public RatPath GetRandomPath()
	{
		return paths.Count == 0 ? null : paths[Random.Range(0, paths.Count)];
	}
}
