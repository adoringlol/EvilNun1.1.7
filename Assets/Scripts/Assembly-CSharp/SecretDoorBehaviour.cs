using UnityEngine;

public class SecretDoorBehaviour : OpenCloseBehaviour
{
	public SecretDoorLockerWoodBehaviour woodLocker;

	public GameObject colliderClosed;

	public MuseumDoorButtonBehaviour buttonBehaviour;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		// Time-gate (turnedOnLights > 0) removed: the door opens once the wood locker
		// is open and the button has been pressed, with no real-time wait.
		if (woodLocker.state == openState.OPENED && buttonBehaviour.touched)
		{
			base.Touched(byPlayer, trapPlayer);
		}
	}
}
