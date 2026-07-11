using DarkTonic.MasterAudio;
using UnityEngine;

public class NunAnimation : MonoBehaviour
{
	[SoundGroup]
	public string stepSound;

	[SoundGroup]
	public string smashSound;

	[SoundGroup]
	public string crawlStep;

	public void Step()
	{
		if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - ZombieBehaviour.instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DAtVector3(stepSound, ZombieBehaviour.instance.eyesTrans.position);
		}
	}

	public void CrawlStep()
	{
		if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - ZombieBehaviour.instance.transform.position.y) < 1.5f)
		{
			MasterAudio.PlaySound3DAtVector3(crawlStep, ZombieBehaviour.instance.eyesTrans.position);
		}
	}

	public void Smash()
	{
		MasterAudio.PlaySound3DAtVector3(smashSound, ZombieBehaviour.instance.eyesTrans.position);
		ZombieBehaviour.instance.DestroyTarget();
	}

	public void CameraSmash()
	{
		CameraMoveAnimation.instance.PlayDiedAnimation();
		GameUIController.instance.SetBloodInCamera(true);
	}

	public void HitPlayerCrawling()
	{
		GameUIController.instance.SetBloodInCamera(true);
	}
}
