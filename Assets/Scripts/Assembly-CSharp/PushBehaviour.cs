using DarkTonic.MasterAudio;
using UnityEngine;

public class PushBehaviour : MonoBehaviour
{
	public bool pushed;

	public float force = 5f;

	private Rigidbody rb;

	private Collider lastCol;

	public AudioClip high;

	public AudioClip low;

	public bool canSound;

	public float dragNoiseAmount = 2f;

	public float fallNoiseAmount = 5f;

	public bool doNoise;

	[SoundGroup]
	public string pushSound;

	[SoundGroup]
	public string fallSound;

	private void Awake()
	{
		canSound = true;
		rb = GetComponent<Rigidbody>();
	}

	public void ContactWithPlayer()
	{
		CancelInvoke("CantDoNoise");
		doNoise = true;
		Invoke("CantDoNoise", 1f);
	}

	public void CantDoNoise()
	{
		doNoise = false;
	}

	public void ContactWithNun()
	{
		doNoise = false;
	}

	private void OnCollisionStay()
	{
		if (rb.velocity.magnitude > 1f && !rb.isKinematic)
		{
			if (doNoise && ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position, 3f, string.Empty);
			}
			if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - base.transform.position.y) < 1.5f)
			{
				MasterAudio.PlaySound3DAtVector3(fallSound, base.transform.position);
			}
		}
		else if (rb.velocity.magnitude > 0.5f && !rb.isKinematic)
		{
			if (doNoise && ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position, 1f, string.Empty);
			}
			if (Mathf.Abs(PlayersManager.instance.GetCurrentController().transform.position.y - base.transform.position.y) < 1.5f)
			{
				MasterAudio.PlaySound3DAtVector3(pushSound, base.transform.position);
			}
		}
	}
}
