using UnityEngine;

public class ProgressManager : MonoBehaviour
{
	public static ProgressManager instance;

	public bool lightState;

	public bool schoolOpened;

	public int day;

	private void Awake()
	{
		instance = this;
		day = 1;
	}

	public void SwitchLightOn()
	{
		lightState = true;
	}

	public void OnRewardedVideoCompleted()
	{
		day--;
		GameUIController.instance.ShowNewDayPanel();
	}

	public void OnRewardedVideoFailedOrSkip()
	{
		GameUIController.instance.ShowNewDayPanel();
	}
}
