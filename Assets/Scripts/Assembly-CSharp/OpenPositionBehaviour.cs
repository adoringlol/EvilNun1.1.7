using UnityEngine;

public class OpenPositionBehaviour : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position + Vector3.up * 0.1f, 0.05f);
		Gizmos.DrawRay(base.transform.position + Vector3.up * 0.1f, base.transform.forward);
	}
}
