using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class SoundSlider : CustomSlider
{
	public override void OnVolumeChanged()
	{
		base.OnVolumeChanged();
		MasterAudio.MasterVolumeLevel = mSlider.value;
		List<SoundGroupVariation> allPlayingVariationsInBus = MasterAudio.GetAllPlayingVariationsInBus("AMBIENT");
		foreach (SoundGroupVariation item in allPlayingVariationsInBus)
		{
			float b = 0.5f;
			item.AdjustVolume(Mathf.Lerp(0f, b, MasterAudio.MasterVolumeLevel) * 10f);
		}
	}

	public override void OnSaveChanges()
	{
		base.OnSaveChanges();
	}
}
