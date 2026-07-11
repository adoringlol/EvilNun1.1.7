using System;
using UnityEngine;

[Serializable]
public class MaskPart
{
	public bool isSet;

	public int maskPartID;

	public GameObject item;

	public string saveName;

	public void CheckPart()
	{
		if (PlayerPrefs.HasKey(saveName))
		{
			item.gameObject.SetActive(true);
			isSet = true;
		}
		else
		{
			item.gameObject.SetActive(false);
			isSet = false;
		}
	}

	public void SetPart()
	{
		PlayerPrefs.SetInt(saveName, 1);
		item.gameObject.SetActive(true);
		isSet = true;
	}
}
