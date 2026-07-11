using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
	public Slider mSlider;

	[Header("Nombre de la propiedad a guardar en pprefs")]
	public string pprefName;

	public CanvasGroup mGraphics;

	public float defaultValue = 1f;

	public bool isEnabled = true;

	public virtual void Initialize()
	{
		if (mSlider == null) mSlider = GetComponent<Slider>();
		if (mSlider == null) return;
		if (PlayerPrefs.HasKey(pprefName))
		{
			mSlider.value = PlayerPrefs.GetFloat(pprefName);
		}
		else
		{
			mSlider.value = defaultValue;
		}
		OnValueChanged();
		UpdateGraphics();
	}

	public virtual void OnMaximumValueReached()
	{
		isEnabled = true;
	}

	public virtual void OnMinimumValueReached()
	{
		isEnabled = false;
	}

	public void OnValueChanged()
	{
		if (mSlider == null) mSlider = GetComponent<Slider>();
		if (mSlider == null) return;
		if (mSlider.value == mSlider.minValue)
		{
			OnMinimumValueReached();
		}
		else if (mSlider.value == mSlider.maxValue)
		{
			OnMaximumValueReached();
		}
		else if (mSlider.value > 0f)
		{
			isEnabled = true;
		}
		OnVolumeChanged();
		UpdateGraphics();
	}

	public virtual void OnVolumeChanged()
	{
	}

	public virtual void OnSaveChanges()
	{
		if (mSlider == null) return;
		PlayerPrefs.SetFloat(pprefName, mSlider.value);
	}

	public void UpdateGraphics()
	{
		if (mGraphics != null) mGraphics.alpha = ((!isEnabled) ? 0.5f : 1f);
	}
}
