using UnityEngine;

public class PositionBehaviour : MonoBehaviour
{
	public PositionPlace place;

	public PositionType type;

	public bool isCenter;

	public bool needSchoolOpened;

	private void Start()
	{
		if (PositionsController.instance != null) PositionsController.instance.AddPosition(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position, Vector3.one * 0.3f);
	}
}
