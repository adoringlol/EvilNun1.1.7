using System.Collections.Generic;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
	public static DoorsManager instance;

	public List<OpenCloseBehaviour> doors;

	private void Awake()
	{
		instance = this;
		if (doors == null) doors = new List<OpenCloseBehaviour>();
	}

	public void AddDoor(OpenCloseBehaviour door)
	{
		if (door != null && !doors.Contains(door)) doors.Add(door);
	}

	public List<OpenCloseBehaviour> GetClosedDoorsNearTo(Vector3 v, float dist)
	{
		List<OpenCloseBehaviour> list = new List<OpenCloseBehaviour>();
		foreach (OpenCloseBehaviour door in doors)
		{
			if (door != null && door.trans != null && Vector3.Distance(door.trans.position, v) < dist && (door.state == OpenCloseBehaviour.openState.CLOSED || door.state == OpenCloseBehaviour.openState.CLOSED_BY_KEY))
			{
				list.Add(door);
			}
		}
		return list;
	}

	public List<OpenCloseBehaviour> GetOpenedDoorsNearTo(Vector3 v, float dist)
	{
		List<OpenCloseBehaviour> list = new List<OpenCloseBehaviour>();
		foreach (OpenCloseBehaviour door in doors)
		{
			if (door != null && door.trans != null && Vector3.Distance(door.trans.position, v) < dist && door.state == OpenCloseBehaviour.openState.OPENED)
			{
				list.Add(door);
			}
		}
		return list;
	}
}
