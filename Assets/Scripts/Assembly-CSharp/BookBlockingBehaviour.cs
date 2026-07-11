public class BookBlockingBehaviour : TakeableObject
{
	private bool blocking = true;

	public MagicLibraryBehaviour pedestal;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (blocking)
		{
			blocking = false;
			pedestal.interactuable = true;
		}
		base.Touched(byPlayer, trapPlayer);
	}
}
