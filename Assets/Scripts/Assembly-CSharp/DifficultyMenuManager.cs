using System.Collections.Generic;
using UnityEngine;

public class DifficultyMenuManager : MonoBehaviour
{
	public static DifficultyMenuManager instance;

	public List<DifficultyItem> difficulties;

	public DifficultyItem selectedDifficulty;

	public int defaultDifficulty = 1;

	private void Awake()
	{
		instance = this;
		if (difficulties == null) difficulties = new List<DifficultyItem>();
	}

	public void OnInitialize()
	{
		if (difficulties.Count == 0) return;
		foreach (DifficultyItem difficulty in difficulties)
		{
			if (difficulty != null && difficulty.content != null) difficulty.content.SetActive(false);
		}
		if (VariablesGlobales.difficultyMode != -1)
		{
			selectedDifficulty = GetDifficulty(VariablesGlobales.difficultyMode);
			if (selectedDifficulty != null && selectedDifficulty.item != null) OnSwitchDifficulty(selectedDifficulty.item, true);
		}
		else
		{
			selectedDifficulty = GetDifficulty(defaultDifficulty);
			if (selectedDifficulty != null && selectedDifficulty.item != null) OnSwitchDifficulty(selectedDifficulty.item, true);
		}
	}

	public DifficultyItem GetDifficulty(int id)
	{
		return difficulties.Find((DifficultyItem l) => l != null && l.id == id);
	}

	public DifficultyItem GetDifficulty(CustomCheckBoxBehaviour diff)
	{
		return difficulties.Find((DifficultyItem l) => l != null && l.item == diff);
	}

	public void OnSwitchDifficulty(CustomCheckBoxBehaviour target, bool state)
	{
		foreach (DifficultyItem difficulty in difficulties)
		{
			if (difficulty != null && difficulty.item != null && (difficulty.item != target || !state))
			{
				difficulty.item.OnDeselect();
			}
		}
		if (selectedDifficulty != null)
		{
			if (selectedDifficulty.content != null) selectedDifficulty.content.SetActive(false);
		}
		selectedDifficulty = null;
		if (state)
		{
			selectedDifficulty = GetDifficulty(target);
		}
		if (selectedDifficulty == null)
		{
			selectedDifficulty = GetDifficulty(VariablesGlobales.difficultyMode);
		}
		if (selectedDifficulty == null) return;
		if (selectedDifficulty.item != null) selectedDifficulty.item.OnSelect();
		if (selectedDifficulty.content != null) selectedDifficulty.content.SetActive(true);
		UpdateDifficulty();
	}

	public void UpdateDifficulty()
	{
		if (selectedDifficulty != null && selectedDifficulty.content != null && selectedDifficulty.id == 3 && selectedDifficulty.content.transform.childCount > 1)
		{
			if (VariablesGlobales.removeAdsBought)
			{
				selectedDifficulty.content.transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				selectedDifficulty.content.transform.GetChild(1).gameObject.SetActive(true);
			}
		}
	}
}
