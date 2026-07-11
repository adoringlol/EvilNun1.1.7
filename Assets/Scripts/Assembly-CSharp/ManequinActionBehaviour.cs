using UnityEngine;

public class ManequinActionBehaviour : TouchableElement
{
	public bool used;

	public ManequinBehaviour manequin;

	public static ManequinActionBehaviour instance;

	public GameObject rope;

	public override void Initialize()
	{
		instance = this;
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!used)
		{
			if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand.id == 20)
			{
				rope.SetActive(false);
				manequin.OnAction();
				interactuable = false;
			}
			else
			{
				MessagesManager.instance.ShowMessage("mannequin");
			}
		}
	}
}
