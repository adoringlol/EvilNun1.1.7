public class CraftingRecipeBehaviour : TakeableObject
{
	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		base.Touched(byPlayer, trapPlayer);
		MessagesManager.instance.ShowMessage("item_recipe_message");
	}
}
