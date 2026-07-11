using System;
using UnityEngine;

[Serializable]
public class RecipePaper
{
	public bool enabled;

	public int itemID;

	public int requiredLights;

	public GameObject paper;

	public GameObject takeablePaper;

	public string recipeSaveName;

	public void EnableRecipe()
	{
		enabled = true;
		paper.SetActive(true);
		PlayerPrefs.SetInt(recipeSaveName, 1);
		ItemsManager.instance.EnableRecipeItems(itemID);
	}

	public void CheckRecipe()
	{
		if (PlayerPrefs.HasKey(recipeSaveName))
		{
			enabled = true;
			paper.SetActive(true);
			if (takeablePaper != null)
			{
				takeablePaper.SetActive(false);
			}
		}
	}
}
