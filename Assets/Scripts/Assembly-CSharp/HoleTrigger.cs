using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			PlayersManager.instance.GetCurrentController().FallHole();
		}
	}
}
