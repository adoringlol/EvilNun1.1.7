using UnityEngine;

public class PlayersManager : MonoBehaviour
{
	public static PlayersManager instance;

	public MyController child;

	public MyController doll;

	public bool isBaby;

	private void Awake()
	{
		instance = this;
		if (child == null)
		{
			child = Object.FindObjectOfType<PlayerController>();
		}
		if (doll == null)
		{
			doll = Object.FindObjectOfType<BabyController>();
		}
		if (child != null)
		{
			if (child.controller == null)
			{
				child.controller = child.GetComponent<CharacterController>();
			}
			if (child.controller != null)
			{
				child.controller.enabled = true;
				child.controller.center = Vector3.zero;
				child.controller.height = 1.35f;
			}
			if (child.movement != null)
			{
				child.movement.enabled = false;
			}
			child.blocked = false;
			if (child.GetComponent<ExportedMovementDriver>() == null)
			{
				child.gameObject.AddComponent<ExportedMovementDriver>();
			}
		}
	}

	public MyController GetCurrentController()
	{
		if (isBaby)
		{
			return doll;
		}
		return child;
	}

	[ContextMenu("StartBaby")]
	public void StartBaby()
	{
		if (child.isHidden)
		{
			if (child.currentHide.door == null)
			{
				doll.playerStartPosition = ((FastHide)child.currentHide).nunPositionDiscover;
			}
			else
			{
				SimpleSmoothMouseLook.instance.OnUsedDoll();
				doll.playerStartPosition = child.currentHide.door.openPositionForEnemie[0];
			}
			doll.gameObject.SetActive(true);
			isBaby = true;
			child.StopControlling();
			doll.StartControlling();
		}
	}

	[ContextMenu("StartChild")]
	public void StartChild()
	{
		isBaby = false;
		if (doll != null)
		{
			doll.gameObject.SetActive(false);
			doll.StopControlling();
		}
		if (child != null)
		{
			child.StartControlling();
		}
	}

	public void BackToChild()
	{
		isBaby = false;
		child.BackToControlling();
	}

	public MyController GetChild()
	{
		return child;
	}

	public MyController GetBaby()
	{
		return doll;
	}

	public void ExplodeDoll()
	{
		((BabyController)doll).Explode();
	}

	public void WaitAndBackToChild()
	{
		Invoke("BackToChild", 1.5f);
	}
}
