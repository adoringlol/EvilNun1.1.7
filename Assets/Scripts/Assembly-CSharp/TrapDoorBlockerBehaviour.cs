using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class TrapDoorBlockerBehaviour : TouchableElement
{
	public List<Transform> clavos;

	public bool used;

	public GameObject nunEntranceTrigger;

	public EasyModeColliderBehaviour seeCollider;

	public Vector3 torque;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (clavos.Count == 0 && !used)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().AddTorque(-base.transform.forward * 15f, ForceMode.Impulse);
		}
		else
		{
			MessagesManager.instance.ShowMessage("cant_remove");
		}
	}

	public void OnRemovedClavo(Transform clavo)
	{
		clavos.Remove(clavo);
		if (clavos.Count == 0)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().AddTorque(torque * 15f, ForceMode.Impulse);
			if (nunEntranceTrigger != null)
			{
				nunEntranceTrigger.SetActive(true);
			}
			if ((bool)seeCollider)
			{
				seeCollider.DoDisable();
			}
		}
	}

	private void OnCollisionStay(Collision col)
	{
		if (!used)
		{
			Vector3 forward = base.transform.forward;
			float num = Vector3.Dot(Vector3.up, forward);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		MasterAudio.PlaySound3DAtVector3("fall_wood", base.transform.position);
	}
}
