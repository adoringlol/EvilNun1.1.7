using UnityEngine;

public class TouchableElement : MonoBehaviour
{
	public int id;

	public string labelName;

	public string actionName;

	public bool interactuable;

	public bool startInteractuable = true;

	public bool specialLocalization;

	public void Start()
	{
		interactuable = startInteractuable;
		Initialize();
	}

	public virtual void Initialize()
	{
	}

	public virtual void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
	}
}
