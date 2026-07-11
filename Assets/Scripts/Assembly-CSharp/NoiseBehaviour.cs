using DarkTonic.MasterAudio;
using UnityEngine;

public class NoiseBehaviour : MonoBehaviour
{
	public bool used;

	public float noiseDistance = 10f;

	public Vector3 startPos;

	[SoundGroup]
	public string fallSound;

	private void Start()
	{
		startPos = base.transform.position;
	}

	public void Reset()
	{
		if (Random.value > 0.5f)
		{
			used = false;
			GetComponent<Rigidbody>().isKinematic = true;
			base.transform.position = startPos;
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player" && !used)
		{
			used = true;
			GetComponent<Rigidbody>().isKinematic = false;
			Invoke("PlaySound", 0.4f);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (used && col.gameObject.tag != "Player" && GetComponent<Rigidbody>().velocity.y > 0.5f)
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	public void PlaySound()
	{
		if (ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.OnDidNoiseWithObject(base.transform.position, noiseDistance);
		}
		MasterAudio.PlaySound3DAtVector3(fallSound, base.transform.position);
	}
}
