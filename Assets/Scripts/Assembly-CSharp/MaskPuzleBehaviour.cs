using System.Collections.Generic;
using DarkTonic.MasterAudio;

public class MaskPuzleBehaviour : TouchableElement
{
	public static MaskPuzleBehaviour instance;

	public List<MaskPart> maskParts;

	public override void Initialize()
	{
		instance = this;
		foreach (MaskPart maskPart in maskParts)
		{
			maskPart.CheckPart();
		}
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!PlayersManager.instance.GetCurrentController().hasObjectOnHand || !(PlayersManager.instance.GetCurrentController().objectOnHand != null))
		{
			return;
		}
		bool flag = false;
		foreach (MaskPart maskPart in maskParts)
		{
			if (maskPart.maskPartID == PlayersManager.instance.GetCurrentController().objectOnHand.id && !maskPart.isSet)
			{
				maskPart.SetPart();
				AnalyticsController.instance.OnSolvePuzzle(maskPart.saveName);
				PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
				MessagesManager.instance.ShowMessage("skeleton_set_object", "1/7");
				flag = true;
				MasterAudio.PlaySound("puzle_complete");
				return;
			}
		}
		MessagesManager.instance.ShowMessage("skeleton_set_object_fail");
	}

	public bool IsMaskPartSet(int id)
	{
		MaskPart maskPart = maskParts.Find((MaskPart m) => m.maskPartID == id);
		return maskPart.isSet;
	}
}
