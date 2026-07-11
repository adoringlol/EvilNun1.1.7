using DG.Tweening;

public class RoomCodeButtonBehaviour : TouchableElement
{
	public int number;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (number >= 0 && number <= 9)
		{
			base.transform.parent.GetComponent<RoomCodeBehaviour>().OnPressButton(number);
		}
		else if (number == -1)
		{
			base.transform.parent.GetComponent<RoomCodeBehaviour>().OnPressAcceptButton();
		}
		else if (number == -2)
		{
			base.transform.parent.GetComponent<RoomCodeBehaviour>().OnPressRemoveButton();
		}
		base.transform.DOLocalMoveX(0.02f, 0.2f);
		base.transform.DOLocalMoveX(0.033f, 0.3f).SetDelay(0.2f);
	}
}
