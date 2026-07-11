using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsTrigger : MonoBehaviour
{
	public enum AnalyticsTriggerType
	{
		EXIT_ROOM = 0
	}

	public AnalyticsTriggerType type;

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Player")
		{
			return;
		}
		int num = (int)type;
		if (!PlayerPrefs.HasKey("analytics_trigger_" + num))
		{
			int num2 = (int)type;
			PlayerPrefs.SetInt("analytics_trigger_" + num2, 1);
			if (type == AnalyticsTriggerType.EXIT_ROOM)
			{
				Analytics.CustomEvent("Exit_First_Room");
				AnalyticsController.instance.OnLeaveFirstRoom();
			}
		}
	}
}
