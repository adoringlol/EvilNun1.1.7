using DarkTonic.MasterAudio;
using UnityEngine.Analytics;

public class ElevatorMechanicSystemBehaviour : TouchableElement
{
	public ElevatorBehaviour elevator;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!elevator.works)
		{
			if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null && PlayersManager.instance.GetCurrentController().objectOnHand.id == 510)
			{
				MessagesManager.instance.ShowMessage("elevator_fix_oil");
				PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
				elevator.works = true;
				elevator.UseElevator();
				AnalyticsController.instance.OnSolvePuzzle("elevator_fix");
				MasterAudio.PlaySound("puzle_complete");
				AnalyticsEvent.Custom("elevator_oil");
				MasterAudio.PlaySound("gears_working");
			}
			else
			{
				MessagesManager.instance.ShowMessage("elevator_broken");
			}
		}
		else
		{
			MessagesManager.instance.ShowMessage("elevator_works");
		}
	}
}
