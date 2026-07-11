using DarkTonic.MasterAudio;

public class MusicSlider : CustomSlider
{
	public override void OnVolumeChanged()
	{
		base.OnVolumeChanged();
		MasterAudio.PlaylistMasterVolume = mSlider.value;
	}
}
