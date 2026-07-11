public class SecretDoorLockerWoodBehaviour : OpenCloseBehaviour
{
	public OpenCloseBehaviour door;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (door.state == openState.CLOSED_FOREVER || door.state == openState.CLOSED)
		{
			if (state == openState.OPENED)
			{
				door.state = openState.CLOSED_FOREVER;
			}
			else
			{
				door.state = openState.CLOSED;
			}
			base.Touched(byPlayer, trapPlayer);
		}
	}
}
