public class OpenCloseBehaviourCollider : TouchableElement
{
	public OpenCloseBehaviour door;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		door.Touched(byPlayer, trapPlayer);
	}
}
