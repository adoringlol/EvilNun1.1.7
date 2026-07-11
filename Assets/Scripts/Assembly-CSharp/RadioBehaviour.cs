using DarkTonic.MasterAudio;
using UnityEngine;

public class RadioBehaviour : TouchableElement
{
	public int tries;

	public Transform noisePosition;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (MasterAudio.IsSoundGroupPlaying("radio"))
		{
			MasterAudio.StopSoundGroupOfTransform(base.transform, "radio");
			return;
		}
		tries++;
		if (tries <= 3)
		{
			MasterAudio.PlaySound3DAtTransform("radio", base.transform);
			ZombieBehaviour.instance.AddNoise(noisePosition.position, 20f, "radio");
		}
		else
		{
			MessagesManager.instance.ShowMessage("radio_dont_work");
		}
	}
}
