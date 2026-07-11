using UnityEngine;

public class WindowMessageTrigger : MonoBehaviour
{
	public bool used;

	private void OnTriggerEnter(Collider col)
	{
		if (!used && col.tag == "Player")
		{
			used = true;
			MessagesManager.instance.ShowMessage("window_tip");
		}
	}
}
