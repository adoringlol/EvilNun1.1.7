public class ElevatorDoorBehaviour : OpenCloseBehaviour
{
	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (state == openState.OPENED && PlayersManager.instance.GetCurrentController().hasObjectOnHand && (bool)PlayersManager.instance.GetCurrentController().objectOnHand.GetComponent<CraftingRecipeBehaviour>())
		{
			ItemsManager.instance.EnableRecipe(PlayersManager.instance.GetCurrentController().objectOnHand.GetComponent<CraftingRecipeBehaviour>().id);
			PlayersManager.instance.GetCurrentController().objectOnHand.GetComponent<CraftingRecipeBehaviour>().OnDestroyItem();
			MessagesManager.instance.ShowMessage("recipe_set_message");
		}
		else
		{
			base.Touched(byPlayer, trapPlayer);
		}
	}
}
