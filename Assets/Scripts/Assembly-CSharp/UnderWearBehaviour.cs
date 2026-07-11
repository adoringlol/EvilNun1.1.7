public class UnderWearBehaviour : TouchableElement
{
	public TakeableObject totakeItem;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		totakeItem.Touched(byPlayer, trapPlayer);
		base.gameObject.SetActive(false);
	}
}
