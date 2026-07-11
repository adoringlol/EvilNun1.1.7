using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class MuseumDoorButtonBehaviour : TouchableElement
{
	public SecretDoorBehaviour door;

	public bool touched;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (touched)
		{
			return;
		}
		MasterAudio.PlaySound3DAtVector3("code_button_press", base.transform.position);
		base.transform.DOLocalMoveX(0.09f, 0.2f);
		// The door's 24h painting-timer gate (turnedOnLights > 0) was removed so the
		// button works immediately; the wood-locker + button steps still gate the door.
		interactuable = false;
		door.interactuable = true;
		foreach (Collider collider in door.colliders)
		{
			if ((bool)collider.GetComponent<OpenCloseBehaviourCollider>())
			{
				collider.GetComponent<OpenCloseBehaviourCollider>().interactuable = true;
			}
		}
		touched = true;
		door.Touched();
		AnalyticsController.instance.OnOpenMuseumDoor();
	}
}
