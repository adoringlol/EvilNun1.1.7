public class BananaBehaviour : TouchableElement
{
	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		base.gameObject.SetActive(false);
	}
}
