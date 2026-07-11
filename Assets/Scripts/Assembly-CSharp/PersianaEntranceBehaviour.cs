using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Analytics;

public class PersianaEntranceBehaviour : OpenCloseBehaviour
{
	public bool opened;

	public Transform button;

	public TouchableMessage mainDoor;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		Vector3 localPosition = base.transform.localPosition;
		button.DOKill(true);
		button.DOLocalMoveZ(-5.296f, 0.15f).SetEase(Ease.Linear);
		button.DOLocalMoveZ(-5.285f, 0.15f).SetDelay(0.15f).SetEase(Ease.Linear);
		if (!opened)
		{
			MasterAudio.PlaySound3DAtVector3("GarageDoorButton", base.transform.position);
			if (ProgressManager.instance.lightState)
			{
				base.Touched(byPlayer, trapPlayer);
				AnalyticsController.instance.OnSolvePuzzle("switch_on");
				AnalyticsEvent.Custom("switch_on");
				opened = true;
				interactuable = false;
				mainDoor.interactuable = false;
			}
			else
			{
				MessagesManager.instance.ShowMessage("entrance_button_description");
			}
		}
	}
}
