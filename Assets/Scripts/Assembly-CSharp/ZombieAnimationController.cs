using Pathfinding;
using UnityEngine;

public class ZombieAnimationController : MonoBehaviour
{
	public void DoStop()
	{
		base.transform.parent.GetComponent<AIPath>().maxSpeed = 0.45f;
	}

	public void DoContinue()
	{
		base.transform.parent.GetComponent<AIPath>().maxSpeed = 1.05f;
	}
}
