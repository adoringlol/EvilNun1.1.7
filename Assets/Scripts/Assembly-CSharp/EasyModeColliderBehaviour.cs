using Pathfinding;
using UnityEngine;

public class EasyModeColliderBehaviour : MonoBehaviour
{
	public Vector2 rect;

	public bool automatic;

	private void Awake()
	{
		rect = GetComponent<NavmeshCut>().rectangleSize;
	}

	private void Start()
	{
		if (automatic && VariablesGlobales.difficultyMode > 0)
		{
			DoDisable();
		}
	}

	public void DoDisable()
	{
		if ((bool)GetComponent<BoxCollider>())
		{
			GetComponent<BoxCollider>().enabled = false;
		}
		if ((bool)GetComponent<NavmeshCut>())
		{
			GetComponent<NavmeshCut>().rectangleSize = Vector2.zero;
			GetComponent<NavmeshCut>().ForceUpdate();
		}
	}

	public void DoEnable()
	{
		if ((bool)GetComponent<BoxCollider>())
		{
			GetComponent<BoxCollider>().enabled = true;
		}
		if ((bool)GetComponent<NavmeshCut>())
		{
			GetComponent<NavmeshCut>().rectangleSize = rect;
			GetComponent<NavmeshCut>().ForceUpdate();
		}
	}
}
