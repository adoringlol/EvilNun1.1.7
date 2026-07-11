using UnityEngine;

public class FastHide : HiddenElement
{
	public Transform camPosition;

	public Transform nunPositionDiscover;

	private void OnTriggerStay(Collider col)
	{
		if (!(col.tag == "Player"))
		{
			return;
		}
		PlayersManager.instance.GetCurrentController().currentHide = this;
		if (ZombieBehaviour.instance != null)
		{
			if (ZombieBehaviour.instance.state != ZombieBehaviour.NunState.ATTACK)
			{
				GameUIController.instance.ShowFastHiddingUI();
			}
		}
		else
		{
			GameUIController.instance.ShowFastHiddingUI();
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player" && !isHiddenHere)
		{
			GameUIController.instance.HideFastHiddingUI();
		}
	}

	public void OnHide()
	{
		PlayersManager.instance.child.IsHidden();
		PlayersManager.instance.child.currentHide = this;
		GameUIController.instance.standupButton.SetActive(false);
		isHiddenHere = true;
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
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.OnEnterHide();
		}
		PlayersManager.instance.child.GetComponent<PlayerMovement>().enabled = false;
		PlayersManager.instance.child.GetComponent<CharacterController>().enabled = false;
		SimpleSmoothMouseLook.instance.HideIn(this);
		CameraMoveAnimation.instance.ResetAnimation();
		GameUIController.instance.j.Reset();
	}

	public void OnExitHide()
	{
		isHiddenHere = false;
		PlayersManager.instance.child.LeaveHide();
		PlayersManager.instance.child.isHidden = false;
		if (ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.sawPlayerHiding = false;
		}
		PlayersManager.instance.child.GetComponent<CharacterController>().enabled = true;
		GameUIController.instance.standupButton.SetActive(true);
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.OnExitHide();
		}
		GameUIController.instance.HideFastHiddingUI();
		SimpleSmoothMouseLook.instance.HideOut(this);
		PlayersManager.instance.child.GetComponent<PlayerMovement>().enabled = true;
	}
}
