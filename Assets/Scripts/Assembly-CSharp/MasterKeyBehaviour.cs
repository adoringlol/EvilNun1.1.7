using UnityEngine.Analytics;

public class MasterKeyBehaviour : TakeableObject
{
	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		base.Touched(byPlayer, trapPlayer);
		AnalyticsEvent.Custom("master_key_taken");
	}
}
