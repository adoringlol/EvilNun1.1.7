using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class CokeMachineBehaviour : TouchableElement
{
	public TakeableObject coke;

	public Transform cokesDoor;

	public int tries;

	public bool used;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (used)
		{
			return;
		}
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == 450)
		{
			MessagesManager.instance.ShowMessage("use_machine_message");
			PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
			used = true;
			Invoke("EnableCoke", 1f);
			cokesDoor.DOKill();
			cokesDoor.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.3f);
			MasterAudio.PlaySound3DAtVector3("insert_coin", base.transform.position);
			return;
		}
		tries++;
		if (tries >= 50)
		{
			used = true;
			MasterAudio.PlaySound3DAtVector3("hit_machine", base.transform.position);
			Invoke("EnableCoke", 1f);
			cokesDoor.DOKill();
			cokesDoor.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.3f);
		}
		else
		{
			cokesDoor.DOShakeRotation(2f, new Vector3(0f, 2f, 0f));
			MessagesManager.instance.ShowMessage("try_use_machine_message");
			MasterAudio.PlaySound3DAtVector3("hit_machine", base.transform.position);
			ZombieBehaviour.instance.AddNoise(base.transform.position, 6f, string.Empty);
		}
	}

	public void EnableCoke()
	{
		coke.gameObject.SetActive(true);
	}
}
