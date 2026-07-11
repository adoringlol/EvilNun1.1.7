using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class MagicLibraryBehaviour : TouchableElement
{
	public Transform bookParent;

	public GameObject blocker;

	public float usedPositionY;

	public GameObject crawlEntranceBiblio;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null && PlayersManager.instance.GetCurrentController().objectOnHand.id == 600)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand.interactuable = false;
			PlayersManager.instance.GetCurrentController().objectOnHand.transform.parent = null;
			PlayersManager.instance.GetCurrentController().objectOnHand.ResetScale();
			PlayersManager.instance.GetCurrentController().objectOnHand.transform.parent = bookParent;
			PlayersManager.instance.GetCurrentController().objectOnHand.transform.localPosition = Vector3.zero;
			PlayersManager.instance.GetCurrentController().objectOnHand.transform.localEulerAngles = Vector3.zero;
			PlayersManager.instance.GetCurrentController().objectOnHand.OnReleaseItem();
			PlayersManager.instance.GetCurrentController().objectOnHand = null;
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
			interactuable = false;
			MasterAudio.PlaySound("grab");
			Invoke("StartPedestal", 0.3f);
		}
		else
		{
			MessagesManager.instance.ShowMessage("need_fit_space");
		}
	}

	public void StartPedestal()
	{
		MasterAudio.PlaySound3DAtVector3("pedestal", base.transform.position);
		base.transform.DOLocalMoveY(usedPositionY, 5f).OnComplete(DiscoverEntrance);
	}

	public void DiscoverEntrance()
	{
		blocker.GetComponent<Rigidbody>().isKinematic = false;
		blocker.GetComponent<Rigidbody>().AddTorque(base.transform.forward * 15f, ForceMode.Impulse);
		blocker.GetComponent<Rigidbody>().AddForce(-base.transform.right * 10f, ForceMode.Impulse);
		crawlEntranceBiblio.SetActive(true);
	}
}
