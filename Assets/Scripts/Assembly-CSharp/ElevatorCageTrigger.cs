using System.Collections.Generic;
using UnityEngine;

public class ElevatorCageTrigger : MonoBehaviour
{
	public List<Collider> currentItems;

	public static ElevatorCageTrigger instance;

	private void Awake()
	{
		instance = this;
		currentItems = new List<Collider>();
	}

	private void OnTriggerStay(Collider col)
	{
		if (!(col.tag == "Takeable") || currentItems.Contains(col))
		{
			return;
		}
		currentItems.Add(col);
		col.gameObject.transform.parent = base.transform;
		if ((bool)col.GetComponent<TakeableObject>())
		{
			if (col.GetComponent<TakeableObject>().usefullItem)
			{
				MessagesManager.instance.ShowMessage("elevator_item_advice");
			}
			else
			{
				MessagesManager.instance.ShowMessage("elevator_items_amount", currentItems.Count.ToString());
			}
		}
		ElevatorBehaviour.instance.UpdateWeight(currentItems);
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Takeable" && currentItems.Contains(col))
		{
			currentItems.Remove(col);
			col.gameObject.transform.parent = null;
			MessagesManager.instance.ShowMessage("elevator_items_amount", currentItems.Count.ToString());
			ElevatorBehaviour.instance.UpdateWeight(currentItems);
		}
	}

	public void RemoveItem(Collider col)
	{
		if (currentItems.Contains(col))
		{
			currentItems.Remove(col);
			ElevatorBehaviour.instance.UpdateWeight(currentItems);
		}
	}

	public bool IsMaterialForDoll()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (Collider currentItem in currentItems)
		{
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 500)
			{
				flag = true;
			}
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 501)
			{
				flag2 = true;
			}
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 502)
			{
				flag3 = true;
			}
		}
		RemoveDollItems((flag && flag2 && flag3) ? true : false);
		if (flag && flag2 && flag3)
		{
			Debug.Log("ha detectado muñeca y dinamita, crear muñeca bomba");
			return true;
		}
		return false;
	}

	public bool IsMaterialForGun()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (Collider currentItem in currentItems)
		{
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 777)
			{
				flag = true;
			}
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 666)
			{
				flag3 = true;
			}
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>() && currentItem.gameObject.GetComponent<TakeableObject>().id == 444)
			{
				flag2 = true;
			}
		}
		RemoveGunItems((flag && flag2 && flag3) ? true : false);
		if (flag && flag2 && flag3)
		{
			Debug.Log("ha detectado materiales para crear pistola");
			return true;
		}
		return false;
	}

	public void RemoveDollItems(bool destroyDollItems)
	{
		bool flag = false;
		for (int num = currentItems.Count - 1; num >= 0; num--)
		{
			Collider collider = currentItems[num];
			if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 500)
			{
				if (destroyDollItems)
				{
					currentItems.RemoveAt(num);
					collider.transform.parent = null;
					collider.gameObject.SetActive(false);
					((BabyController)PlayersManager.instance.doll).doll = collider.gameObject;
				}
			}
			else if (!collider.gameObject.GetComponent<TakeableObject>() || collider.gameObject.GetComponent<TakeableObject>().id != 1000)
			{
				if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 501)
				{
					if (!flag && destroyDollItems)
					{
						flag = true;
						currentItems.RemoveAt(num);
						collider.transform.parent = null;
						collider.gameObject.SetActive(false);
						((BabyController)PlayersManager.instance.doll).dynamite = collider.gameObject;
					}
				}
				else if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 502 && destroyDollItems)
				{
					currentItems.RemoveAt(num);
					collider.transform.parent = null;
					collider.gameObject.SetActive(false);
					((BabyController)PlayersManager.instance.doll).rope = collider.gameObject;
				}
			}
		}
	}

	public void RemoveGunItems(bool destroyItems)
	{
		for (int num = currentItems.Count - 1; num >= 0; num--)
		{
			Collider collider = currentItems[num];
			if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 666)
			{
				if (destroyItems)
				{
					currentItems.RemoveAt(num);
					collider.transform.parent = null;
					collider.gameObject.SetActive(false);
				}
			}
			else if (!collider.gameObject.GetComponent<TakeableObject>() || collider.gameObject.GetComponent<TakeableObject>().id != 333)
			{
				if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 777)
				{
					if (destroyItems)
					{
						currentItems.RemoveAt(num);
						collider.transform.parent = null;
						collider.gameObject.SetActive(false);
					}
				}
				else if ((bool)collider.gameObject.GetComponent<TakeableObject>() && collider.gameObject.GetComponent<TakeableObject>().id == 444 && destroyItems)
				{
					currentItems.RemoveAt(num);
					collider.transform.parent = null;
					collider.gameObject.SetActive(false);
				}
			}
		}
	}
}
