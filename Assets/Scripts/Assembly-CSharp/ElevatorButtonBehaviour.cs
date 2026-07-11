using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class ElevatorButtonBehaviour : TouchableElement
{
	public ElevatorBehaviour elevator;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!elevator.busy && elevator.works)
		{
			MasterAudio.PlaySound("ascensor_button");
			elevator.UseElevator();
		}
		else if (!elevator.works)
		{
			MessagesManager.instance.ShowMessage("elevator_broken");
		}
		else if (elevator.busy)
		{
			MessagesManager.instance.ShowMessage("elevator_on_using");
		}
		Vector3 localPosition = base.transform.localPosition;
		base.transform.DOKill(true);
		base.transform.DOLocalMove(base.transform.localPosition - base.transform.up * 0.03f, 0.15f).SetEase(Ease.Linear);
		base.transform.DOLocalMove(localPosition, 0.15f).SetDelay(0.15f).SetEase(Ease.Linear);
	}
}
