using DarkTonic.MasterAudio;
using UnityEngine;

public class TouchExitBehaviour : TouchableElement
{
	public Animation anim;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == 50)
		{
			interactuable = false;
			anim.Play("unlock");
			MasterAudio.PlaySound3DAtVector3("PadlockOpen", base.transform.position);
			CinematicsManager.instance.PrepareExitSchoolCinematic();
		}
		else
		{
			MasterAudio.PlaySound3DAtVector3("tryexitdoor", base.transform.position);
			MessagesManager.instance.ShowMessage("need_master_key");
		}
	}
}
