using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsContainerBehaviour : MonoBehaviour
{
	public List<ItemPositionBehaviour> positions;

	private void Awake()
	{
		positions = GetComponentsInChildren<ItemPositionBehaviour>().ToList();
	}

	private void Start()
	{
		if (ItemsManager.instance != null && !ItemsManager.instance.containers.Contains(this)) ItemsManager.instance.containers.Add(this);
	}

	public bool ContainsItem(ItemPositionData item)
	{
		foreach (ItemPositionBehaviour position in positions)
		{
			if (position.type == item.type)
			{
				return true;
			}
		}
		return false;
	}

	public Transform GetPositionForItem(ItemPositionData item)
	{
		foreach (ItemPositionBehaviour position in positions)
		{
			if (position.type == item.type)
			{
				return position.transform;
			}
		}
		return null;
	}
}
