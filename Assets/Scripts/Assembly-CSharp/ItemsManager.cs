using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
	public static ItemsManager instance;

	public List<ItemPositionData> reorderableItems;

	public List<ItemsContainerBehaviour> containers;

	public List<RecipePaper> recipes;

	private void Awake()
	{
		instance = this;
		containers = new List<ItemsContainerBehaviour>();
	}

	private void Start()
	{
		if (recipes != null) foreach (RecipePaper recipe in recipes)
		{
			if (recipe != null) recipe.CheckRecipe();
		}
		Invoke("ReorderItems", 0.5f);
	}

	public void EnableRecipeItems(int recipeID)
	{
		foreach (ItemPositionData reorderableItem in reorderableItems)
		{
			if (reorderableItem.hasRecipe && reorderableItem.recipeID == recipeID)
			{
				reorderableItem.item.SetActive(true);
			}
		}
	}

	public void ReorderItems()
	{
		if (reorderableItems == null || containers == null || containers.Count == 0)
		{
			Debug.LogWarning("Item reordering skipped because no item containers were exported.", this);
			return;
		}
		// Deterministic placement: scan for the first unused compatible container.
		// The original used a bounded random retry (Random.Range, attempts < Count*2)
		// which could fail to place an item even when a compatible container existed
		// (source of the "No compatible container found for item tijeras" warning).
		List<ItemsContainerBehaviour> list = new List<ItemsContainerBehaviour>();
		foreach (ItemPositionData ipd in reorderableItems)
		{
			if (ipd == null || ipd.item == null) continue;
			bool flag = false;
			foreach (ItemsContainerBehaviour itemsContainerBehaviour in containers)
			{
				if (itemsContainerBehaviour == null || list.Contains(itemsContainerBehaviour) || !itemsContainerBehaviour.ContainsItem(ipd))
				{
					continue;
				}
				Transform positionForItem = itemsContainerBehaviour.GetPositionForItem(ipd);
				if (positionForItem == null) continue;
				ipd.item.transform.parent = itemsContainerBehaviour.transform;
				ipd.item.transform.position = positionForItem.position;
				ipd.item.transform.localEulerAngles = positionForItem.localEulerAngles;
				list.Add(itemsContainerBehaviour);
				flag = true;
				break;
			}
			if (ipd.hasRecipe)
			{
				RecipePaper recipePaper = (recipes != null) ? recipes.Find((RecipePaper r) => r != null && r.itemID == ipd.recipeID) : null;
				ipd.item.SetActive(recipePaper == null || recipePaper.enabled);
			}
			if (!flag) Debug.LogWarning("No compatible container found for item " + ipd.item.name, this);
		}
	}

	public void EnableRecipe(int id)
	{
		if (recipes == null) return;
		RecipePaper recipe = recipes.Find((RecipePaper r) => r != null && r.itemID == id);
		if (recipe != null) recipe.EnableRecipe();
	}
}
