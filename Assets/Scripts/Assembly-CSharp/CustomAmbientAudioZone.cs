using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CustomAmbientAudioZone : MonoBehaviour
{
	[HideInInspector]
	public CustomAmbientAudio main;

	private void Start()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<BoxCollider>().isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			main.OnEnterZone();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			main.OnExitZone();
		}
	}
}
