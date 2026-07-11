using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine.Analytics;

public class SkeletonBehaviour : TouchableElement
{
	public List<SkeletonPiece> pieces;

	public static SkeletonBehaviour instance;

	public int items;

	public override void Initialize()
	{
		instance = this;
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (CanPray())
		{
			MessagesManager.instance.ShowMessage("skeleton_completed");
		}
		else if (PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			SkeletonPiece skeletonPiece = CheckCanSetItem();
			if (skeletonPiece != null)
			{
				AnalyticsEvent.Custom("skeleton_set_object", new Dictionary<string, object> { { "object_name", skeletonPiece.name } });
				int num = PlayersManager.instance.GetCurrentController().objectOnHand.id;
				PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
				if (num == 120)
				{
					skeletonPiece.alternativeItem.gameObject.SetActive(true);
				}
				else
				{
					skeletonPiece.item.gameObject.SetActive(true);
				}
				skeletonPiece.isSet = true;
				items++;
				MasterAudio.PlaySound3DAtVector3("hide", base.transform.position);
				MessagesManager.instance.ShowMessage("skeleton_set_object", items + "/3");
				if (items == 3)
				{
					AnalyticsController.instance.OnSolvePuzzle("skeleton-complete");
				}
			}
			else
			{
				MessagesManager.instance.ShowMessage("skeleton_set_object_fail");
			}
		}
		else
		{
			MessagesManager.instance.ShowMessage("skeleton_description");
		}
	}

	public SkeletonPiece CheckCanSetItem()
	{
		foreach (SkeletonPiece piece in pieces)
		{
			if (!piece.isSet && (PlayersManager.instance.GetCurrentController().objectOnHand.id == piece.validID || (PlayersManager.instance.GetCurrentController().objectOnHand.id == piece.validID2 && piece.alternativeItem != null)))
			{
				return piece;
			}
		}
		return null;
	}

	public bool CanPray()
	{
		bool result = true;
		foreach (SkeletonPiece piece in pieces)
		{
			if (!piece.isSet)
			{
				result = false;
			}
		}
		return result;
	}
}
