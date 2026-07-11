using UnityEngine.Analytics;

public class NunKeyBehaviour : TakeableObject
{
	public bool taken;

	public bool hasNun = true;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (hasNun)
		{
			MessagesManager.instance.ShowMessage("cant_take_nun_key");
			return;
		}
		base.Touched(byPlayer, trapPlayer);
		if (!taken)
		{
			AnalyticsEvent.Custom("nun_key_taken");
		}
		taken = true;
	}

	public void KeyReleased()
	{
		hasNun = false;
	}
}
