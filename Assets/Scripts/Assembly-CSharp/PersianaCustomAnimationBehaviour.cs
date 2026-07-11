using Pathfinding;
using UnityEngine;

public class PersianaCustomAnimationBehaviour : TouchableElement
{
	public Animation anim;

	public bool opened;

	public BoxCollider col;

	public NavmeshCut n;

	public string key_message;

	public bool doNoise;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!opened)
		{
			MessagesManager.instance.ShowMessage(key_message);
		}
	}

	public void OpenPersiana()
	{
		if (!opened)
		{
			if (ZombieBehaviour.instance != null && doNoise)
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position, 60f, string.Empty);
			}
			opened = true;
			anim.Play();
			Invoke("DisableCollider", 2f);
		}
	}

	public void DisableCollider()
	{
		ProgressManager.instance.schoolOpened = true;
		col.enabled = false;
		n.rectangleSize = new Vector2(0f, 0f);
		n.ForceUpdate();
	}
}
