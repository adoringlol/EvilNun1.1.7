using UnityEngine;

public class WindowBehaviour : OpenCloseBehaviour
{
	public Transform camPosition;

	public static WindowBehaviour instance;

	public bool isInside;

	public override void Initialize()
	{
		instance = this;
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!isInside)
		{
			isInside = true;
			PlayersManager.instance.GetCurrentController().GetComponent<PlayerMovement>().enabled = false;
			GameUIController.instance.standupButton.SetActive(false);
			SimpleSmoothMouseLook.instance.OnOpenWindow(camPosition.position);
		}
		else
		{
			isInside = false;
			SimpleSmoothMouseLook.instance.OnHideWindow();
			PlayersManager.instance.GetCurrentController().GetComponent<PlayerMovement>().enabled = true;
			GameUIController.instance.standupButton.SetActive(true);
		}
	}
}
