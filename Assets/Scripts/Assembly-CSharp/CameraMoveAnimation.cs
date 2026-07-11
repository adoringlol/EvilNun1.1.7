using DarkTonic.MasterAudio;
using UnityEngine;

public class CameraMoveAnimation : MonoBehaviour
{
	private Animation anim;

	public static CameraMoveAnimation instance;

	public bool canAnimate = true;

	public Transform fallPosition;

	[SoundGroup]
	public string fallBody1;

	[SoundGroup]
	public string fallBody2;

	[SoundGroup]
	public string fallBody3;

	private void Awake()
	{
		anim = base.gameObject.GetComponent<Animation>();
		instance = this;
		canAnimate = true;
	}

	public void CameraStepAnimation()
	{
		if (canAnimate)
		{
			if (anim.IsPlaying("Camera_Idle"))
			{
				anim.CrossFade("Step", 0.25f);
			}
			else if (!anim.IsPlaying("Step"))
			{
				anim.CrossFade("Step", 0.25f);
			}
		}
	}

	public void CameraIdleAnimation()
	{
		base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		if (canAnimate)
		{
			anim.CrossFade("Camera_Idle", 0.25f);
		}
	}

	public void StopAllAnimations()
	{
		base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		anim.Stop();
		canAnimate = false;
	}

	public void ResetAnimation()
	{
		canAnimate = true;
		CameraIdleAnimation();
	}

	public void PlayDiedAnimation()
	{
		if (PlayersManager.instance.GetCurrentController().standup)
		{
			anim.CrossFade("die");
		}
		else
		{
			anim.CrossFade("die_crawl");
		}
	}

	public void PlayFallHoleAnimation()
	{
		base.transform.parent.eulerAngles = new Vector3(0f, base.transform.parent.eulerAngles.y, base.transform.parent.eulerAngles.z);
		base.transform.parent.position = fallPosition.position;
		anim.Play("dieHole");
	}

	public void PlayFallBody1()
	{
		MasterAudio.PlaySound3DAtVector3(fallBody1, base.transform.position);
	}

	public void PlayFallBody2()
	{
		MasterAudio.PlaySound3DAtVector3(fallBody2, base.transform.position);
	}

	public void ThrowHat()
	{
		PlayersManager.instance.GetCurrentController().ThrowHat();
		PlayersManager.instance.GetCurrentController().ThrowFebatistaDoll();
		PlayersManager.instance.GetCurrentController().ThrowFeromonasItem();
		PlayersManager.instance.GetCurrentController().ThrowJohnWolfeItem();
		PlayersManager.instance.GetCurrentController().ThrowChivoHat();
		PlayersManager.instance.GetCurrentController().ThrowVividPlaysHat();
		PlayersManager.instance.GetCurrentController().ThrowGenuItem();
		PlayersManager.instance.GetCurrentController().ThrowKubzscoutsItem();
		PlayersManager.instance.GetCurrentController().ThrowDenisItem();
		PlayersManager.instance.GetCurrentController().ThrowZbingzItem();
		PlayersManager.instance.GetCurrentController().ThrowGermanSnakeItem();
	}

	public void PlayFallBody3()
	{
		MasterAudio.PlaySound3DAtVector3(fallBody3, base.transform.position);
	}

	public void OnFinishFall()
	{
		PlayersManager.instance.GetCurrentController().ThrowHat();
		PlayersManager.instance.GetCurrentController().ThrowFebatistaDoll();
		PlayersManager.instance.GetCurrentController().ThrowFeromonasItem();
		PlayersManager.instance.GetCurrentController().ThrowJohnWolfeItem();
		PlayersManager.instance.GetCurrentController().ThrowChivoHat();
		PlayersManager.instance.GetCurrentController().ThrowVividPlaysHat();
		PlayersManager.instance.GetCurrentController().ThrowGenuItem();
		PlayersManager.instance.GetCurrentController().ThrowKubzscoutsItem();
		PlayersManager.instance.GetCurrentController().ThrowDenisItem();
		PlayersManager.instance.GetCurrentController().ThrowZbingzItem();
		PlayersManager.instance.GetCurrentController().ThrowGermanSnakeItem();
		GameUIController.instance.SetBloodInCamera(true);
		MasterAudio.PlaySound3DAtVector3(fallBody2, base.transform.position);
		GameUIController.instance.Invoke("ShowNewDayPanel", 3f);
	}
}
