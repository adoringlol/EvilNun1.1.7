using DarkTonic.MasterAudio;
using UnityEngine;

public class TrapDoorBehaviour : OpenCloseBehaviour
{
	public bool used;

	public Vector3 originalPosition;

	public Vector3 euler;

	public BoxCollider trigger;

	public GameObject nunEntranceTrigger;

	public EasyModeColliderBehaviour seeCollider;

	public override void Initialize()
	{
		trans = base.transform;
		used = false;
		originalPosition = base.transform.position;
		euler = base.transform.eulerAngles;
		GetComponent<Rigidbody>().isKinematic = true;
		interactuable = true;
		if (trigger != null)
		{
			trigger.enabled = false;
		}
		if (nunEntranceTrigger != null)
		{
			nunEntranceTrigger.SetActive(false);
		}
		if ((bool)seeCollider)
		{
			seeCollider.DoEnable();
		}
	}

	public new void Reset()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<BoxCollider>().isTrigger = false;
		base.transform.position = originalPosition;
		base.transform.eulerAngles = euler;
		used = false;
		interactuable = true;
		if (trigger != null)
		{
			trigger.enabled = false;
		}
		if (nunEntranceTrigger != null)
		{
			nunEntranceTrigger.SetActive(false);
		}
		if ((bool)seeCollider)
		{
			seeCollider.DoEnable();
		}
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!used && state != openState.CLOSED_BY_KEY)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().AddTorque(-base.transform.right * 15f, ForceMode.Impulse);
			MasterAudio.PlaySound3DAtTransform("fall_metal_trapdoor_touch", base.transform);
		}
		else
		{
			MessagesManager.instance.ShowMessage("blocked");
		}
	}

	private void OnCollisionStay(Collision col)
	{
		Vector3 rhs = base.transform.TransformDirection(Vector3.up);
		float num = Vector3.Dot(Vector3.up, rhs);
		if (num < 0.02f && !used)
		{
			used = true;
			interactuable = false;
			if (seeCollider != null)
			{
				seeCollider.DoDisable();
			}
			if (nunEntranceTrigger != null)
			{
				nunEntranceTrigger.SetActive(true);
			}
			MasterAudio.PlaySound3DAtVector3(audioOpen, base.transform.position);
			GetComponent<Rigidbody>().isKinematic = true;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.OnDidNoiseWithObject(base.transform.position, 25f);
			}
			if (trigger != null)
			{
				trigger.enabled = true;
			}
		}
	}

	private void OnTriggerStay(Collider col)
	{
		if (VariablesGlobales.difficultyMode != 0 && col.tag == "Nun" && !ZombieBehaviour.instance.canSeePlayer && ZombieBehaviour.instance.animState != ZombieBehaviour.ZombieAnimation.CRAWL && used && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.FOLLOWING_PLAYER && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.FOLLOWING_LAST_SEEN_POSITION)
		{
			Reset();
		}
	}
}
