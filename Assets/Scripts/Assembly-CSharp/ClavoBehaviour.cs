using DarkTonic.MasterAudio;

public class ClavoBehaviour : TouchableElement
{
	public TrapDoorBlockerBehaviour blocker;

	public ElevatorBlockerBehaviour persianaBlocker;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == 30)
		{
			MasterAudio.PlaySound3DAtVector3("remove_nail", base.transform.position);
			if ((bool)blocker)
			{
				blocker.OnRemovedClavo(base.transform);
			}
			if ((bool)persianaBlocker)
			{
				persianaBlocker.OnRemovedClavo(base.transform);
			}
			base.gameObject.SetActive(false);
		}
		else
		{
			MessagesManager.instance.ShowMessage("need_hammer");
		}
	}
}
