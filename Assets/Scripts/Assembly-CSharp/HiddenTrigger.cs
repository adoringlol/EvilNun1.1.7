using UnityEngine;

public class HiddenTrigger : HiddenElement
{
	private void OnTriggerStay(Collider col)
	{
		if (!(col.tag == "Player"))
		{
			return;
		}
		if (isHiddenHere)
		{
			if (door.state == OpenCloseBehaviour.openState.OPENED || door.state == OpenCloseBehaviour.openState.OPENING)
			{
				isHiddenHere = false;
				PlayersManager.instance.GetCurrentController().isHidden = false;
				PlayersManager.instance.GetCurrentController().LeaveHide();
			}
		}
		else
		{
			if (door.state != OpenCloseBehaviour.openState.CLOSING && door.state != OpenCloseBehaviour.openState.CLOSED)
			{
				return;
			}
			PlayersManager.instance.GetCurrentController().IsHidden();
			PlayersManager.instance.GetCurrentController().currentHide = this;
			isHiddenHere = true;
			if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
			{
				PlayersManager.instance.GetCurrentController().objectOnHand.OnEnterHide();
			}
			if (ZombieBehaviour.instance != null)
			{
				if (ZombieBehaviour.instance.canSeePlayer && ZombieBehaviour.instance.IsInFOV())
				{
					ZombieBehaviour.instance.sawPlayerHiding = true;
				}
				else
				{
					ZombieBehaviour.instance.sawPlayerHiding = false;
				}
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player" && isHiddenHere)
		{
			PlayersManager.instance.GetCurrentController().isHidden = false;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.sawPlayerHiding = false;
			}
		}
	}
}
