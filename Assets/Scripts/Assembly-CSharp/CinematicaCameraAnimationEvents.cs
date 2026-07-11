using DarkTonic.MasterAudio;
using UnityEngine;

public class CinematicaCameraAnimationEvents : MonoBehaviour
{
	[SoundGroup]
	public string running;

	public void StartRunning()
	{
		MasterAudio.PlaySound(running);
	}
}
