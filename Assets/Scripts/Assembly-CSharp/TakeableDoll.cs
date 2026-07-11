public class TakeableDoll : TakeableObject
{
	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (PlayersManager.instance.GetCurrentController().isHidden)
		{
			GameUIController.instance.startDollButton.SetActive(true);
			GameUIController.instance.startDollButton.transform.GetChild(0).gameObject.SetActive(false);
		}
		else
		{
			GameUIController.instance.startDollButton.SetActive(true);
			GameUIController.instance.startDollButton.transform.GetChild(0).gameObject.SetActive(true);
		}
		base.Touched(byPlayer, trapPlayer);
	}

	public override void OnExitHide()
	{
		GameUIController.instance.startDollButton.SetActive(true);
		GameUIController.instance.startDollButton.transform.GetChild(0).gameObject.SetActive(true);
		base.OnExitHide();
	}

	public override void OnEnterHide()
	{
		GameUIController.instance.startDollButton.SetActive(true);
		GameUIController.instance.startDollButton.transform.GetChild(0).gameObject.SetActive(false);
		base.OnEnterHide();
	}

	public override void OnReleaseItem()
	{
		GameUIController.instance.startDollButton.SetActive(false);
		base.OnReleaseItem();
	}
}
