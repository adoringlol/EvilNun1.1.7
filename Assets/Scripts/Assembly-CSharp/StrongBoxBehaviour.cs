using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class StrongBoxBehaviour : OpenCloseBehaviour
{
	public Transform manivela;

	public Transform key;

	public Transform keyTapa;

	public bool opened;

	[SoundGroup]
	public string manivelaSound;

	[SoundGroup]
	public string enterKey;

	[SoundGroup]
	public string openSafeBox;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (state == openState.CLOSED_BY_KEY)
		{
			if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == 60)
			{
				keyTapa.DOLocalRotate(new Vector3(0f, 0f, -90f), 0.3f).OnComplete(BackToStartState);
				MasterAudio.PlaySound3DAtVector3(manivelaSound, base.transform.position);
				PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
				Object.Destroy(PlayersManager.instance.GetCurrentController().objectOnHand.gameObject);
				key.gameObject.SetActive(true);
				MessagesManager.instance.ShowMessage("enter_safe_key");
				state = openState.CLOSED;
			}
			else
			{
				manivela.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.3f).OnComplete(BackToStartState);
				interactuable = false;
				MasterAudio.PlaySound3DAtVector3("flick_door", base.transform.position);
				trans.DOShakeRotation(1.5f, new Vector3(0f, 1f, 0f));
				MessagesManager.instance.ShowMessage("need_safe_key");
			}
		}
		else if (state == openState.CLOSED)
		{
			if (!opened)
			{
				MasterAudio.PlaySound3DAtVector3(openSafeBox, base.transform.position);
				manivela.DOLocalRotate(new Vector3(0f, 0f, 240f), 0.5f).OnComplete(Open);
				interactuable = false;
			}
			else
			{
				base.Touched(true, false);
			}
		}
		else if (state == openState.OPENED)
		{
			base.Touched(true, false);
		}
	}

	public void BackToStartState()
	{
		manivela.DOLocalRotate(new Vector3(0f, 0f, 60f), 0.3f).SetDelay(0.2f);
		interactuable = true;
	}

	public void Open()
	{
		interactuable = true;
		opened = true;
		base.Touched(true, false);
	}
}
