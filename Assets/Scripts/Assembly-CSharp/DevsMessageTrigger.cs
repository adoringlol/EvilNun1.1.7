using UnityEngine;

public class DevsMessageTrigger : MonoBehaviour
{
	public bool used;

	private void OnTriggerEnter(Collider col)
	{
		if (!used && col.tag == "Player")
		{
			used = true;
			MessagesManager.instance.ShowMessage("devs_working");
		}
	}
}
