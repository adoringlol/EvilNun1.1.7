using System.Collections.Generic;
using UnityEngine;

public class QualityMenuManager : MonoBehaviour
{
	public static QualityMenuManager instance;

	public List<QualityItem> qualities;

	public QualityItem selectedQuality;

	public int defaultQuality = 1;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		OnInitialize();
	}

	public void OnInitialize()
	{
		if (qualities == null || qualities.Count == 0)
		{
			return;
		}
		if (VariablesGlobales.qualityMode != -1)
		{
			selectedQuality = GetQuality(VariablesGlobales.qualityMode);
			if (selectedQuality != null && selectedQuality.item != null) OnSwitchQuality(selectedQuality.item, true);
		}
		else
		{
			selectedQuality = GetQuality(defaultQuality);
			if (selectedQuality != null && selectedQuality.item != null) OnSwitchQuality(selectedQuality.item, true);
		}
	}

	public QualityItem GetQuality(int id)
	{
		return qualities.Find((QualityItem l) => l != null && l.id == id);
	}

	public QualityItem GetQuality(CustomCheckBoxBehaviour q)
	{
		return qualities.Find((QualityItem l) => l != null && l.item == q);
	}

	public void OnSwitchQuality(CustomCheckBoxBehaviour target, bool state)
	{
		foreach (QualityItem quality in qualities)
		{
			if (quality != null && (quality.item != target || !state))
			{
				if (quality.item != null) quality.item.OnDeselect();
			}
		}
		selectedQuality = null;
		if (state)
		{
			selectedQuality = GetQuality(target);
		}
		if (selectedQuality == null)
		{
			selectedQuality = GetQuality(VariablesGlobales.qualityMode);
		}
		if (selectedQuality != null && selectedQuality.item != null)
		{
			selectedQuality.item.OnSelect();
		}
	}
}
