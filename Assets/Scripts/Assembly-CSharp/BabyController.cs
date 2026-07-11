using DarkTonic.MasterAudio;
using UnityEngine;

public class BabyController : MyController
{
	public float dollSpeed = 1f;

	public GameObject explosionEffect;

	public GameObject doll;

	public GameObject rope;

	public GameObject dynamite;

	[SoundGroup]
	public string explosion;

	public override void DoUpdate()
	{
		if (!blocked)
		{
			Vector3 normalized = Camera.main.transform.forward.normalized;
			controller.Move(base.transform.forward * Time.deltaTime * dollSpeed);
			lastVelocity = controller.velocity.magnitude;
			base.DoUpdate();
		}
	}

	public override void StartControlling()
	{
		GameUIController.instance.SetBloodInCamera(false);
		CameraMoveAnimation.instance.StopAllAnimations();
		GameUIController.instance.pauseButton.SetActive(true);
		GameUIController.instance.standupButton.SetActive(false);
		GameUIController.instance.fastHiddingUI.SetActive(false);
		if (GameUIController.instance.useJoystick)
		{
			GameUIController.instance.joystickUI.SetActive(false);
		}
		UpdateStandUp(true);
		base.transform.position = playerStartPosition.position;
		GetComponent<CharacterController>().enabled = true;
		movement.enabled = true;
		base.transform.forward = playerStartPosition.forward;
		currentHide = null;
		isHidden = false;
		SimpleSmoothMouseLook.instance.StartCamera();
		knowsNunFollowsHim = false;
		canPlayTensionSound = true;
		blocked = false;
		GameUIController.instance.StartCoroutine("DollCountDown");
		MasterAudio.PlaySound("doll_music_countdown");
		base.StartControlling();
	}

	public override void StopControlling()
	{
		SimpleSmoothMouseLook.instance.canLook = false;
		MasterAudio.StopAllOfSound("doll_music_countdown");
		movement.enabled = false;
		BlockPlayer();
		base.gameObject.SetActive(false);
		base.StopControlling();
	}

	public override void BlockPlayer()
	{
		movement.enabled = false;
		blocked = true;
		hasTouchableInFront = false;
		if (GameUIController.instance.interactionEnabledSprite.activeSelf)
		{
			GameUIController.instance.interactionEnabledSprite.SetActive(false);
		}
		GameUIController.instance.releaseButton.SetActive(false);
		GameUIController.instance.standupButton.SetActive(false);
		GameUIController.instance.actionButton.SetActive(false);
		GameUIController.instance.fastHiddingUI.SetActive(false);
		GameUIController.instance.j.Reset();
		if (GameUIController.instance.useJoystick)
		{
			GameUIController.instance.joystickUI.SetActive(false);
		}
		base.BlockPlayer();
	}

	public void DismountDoll()
	{
		if (rope != null)
		{
			rope.transform.position = base.transform.position;
			rope.gameObject.SetActive(true);
			rope.GetComponent<Rigidbody>().isKinematic = false;
		}
		if (doll != null)
		{
			doll.transform.position = base.transform.position;
			doll.gameObject.SetActive(true);
			doll.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void Explode()
	{
		if (ZombieBehaviour.instance != null && ZombieBehaviour.instance.GetDistanceBetween(base.transform.position, ZombieBehaviour.instance.transform.position) < 2.5f)
		{
			ZombieBehaviour.instance.StartRagDoll(base.transform.position);
		}
		explosionEffect.transform.position = base.transform.position + Vector3.up * 0.5f;
		explosionEffect.SetActive(true);
		explosionEffect.GetComponent<ParticleSystem>().Play(true);
		MasterAudio.PlaySound3DAtVector3(explosion, base.transform.position);
		StopControlling();
		DismountDoll();
		PlayersManager.instance.WaitAndBackToChild();
	}
}
