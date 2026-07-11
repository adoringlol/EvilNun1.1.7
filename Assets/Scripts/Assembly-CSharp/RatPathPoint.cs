using System.Collections.Generic;
using UnityEngine;

public class RatPathPoint : MonoBehaviour
{
	public List<RatPath> paths;

	public RatPath GetOtherPath(RatPath p)
	{
		if (paths == null || paths.Count == 0) return null;
		if (paths.Count == 1)
		{
			return paths[0];
		}
		List<RatPath> list = new List<RatPath>();
		foreach (RatPath path in paths)
		{
			if (path != p)
			{
				list.Add(path);
			}
		}
		return list.Count == 0 ? p : list[Random.Range(0, list.Count)];
	}
}
