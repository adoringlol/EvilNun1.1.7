using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Analytics;

public class LightSwitchBehaviour : OpenCloseBehaviour
{
	public Light l;

	public TouchableElement currentItem;

	public bool hasItem;

	public Transform itemParent;

	public List<int> validIds;

	public GameObject cable;

	public SkeletonLightBehaviour skl;

	public override void Initialize()
	{
		if (l == null)
		{
			l = GetComponentInChildren<Light>();
		}
		if (l != null)
		{
			l.enabled = false;
		}
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!hasItem)
		{
			base.Touched(byPlayer, false);
		}
	}

	public override void OnFinishAction()
	{
		base.OnFinishAction();
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && !hasItem)
		{
			if (state == openState.OPENED)
			{
				StartCoroutine("TrySetItem");
			}
		}
		else if (!hasItem)
		{
			if (state == openState.OPENED)
			{
				base.Touched(true, false);
			}
			else
			{
				MessagesManager.instance.ShowMessage("lever_description");
			}
		}
		else if (state == openState.OPENED)
		{
			l.enabled = true;
		}
		else
		{
			l.enabled = false;
		}
	}

	public IEnumerator TrySetItem()
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			if (!validIds.Contains(PlayersManager.instance.GetCurrentController().objectOnHand.id))
			{
				yield return new WaitForSeconds(1f);
				state = openState.CLOSING;
				trans.DOKill();
				trans.DOLocalRotate(rotationClosed, actionTime, RotateMode.FastBeyond360).OnComplete(OnClosedAutomatically);
				MessagesManager.instance.ShowMessage("lever_try");
				yield break;
			}
			hasItem = true;
			PlayersManager.instance.GetCurrentController().objectOnHand.OnDestroyItem();
			ProgressManager.instance.SwitchLightOn();
			MessagesManager.instance.ShowMessage("lever_success");
			cable.SetActive(true);
			interactuable = false;
			l.enabled = true;
			skl.OnLightEnabled();
			MasterAudio.PlaySound("generator_on");
			AnalyticsEvent.Custom("generator_on");
			AnalyticsController.instance.OnSolvePuzzle("generator-on");
			MasterAudio.PlaySound("puzle_complete");
		}
	}

	public void OnClosedAutomatically()
	{
		l.enabled = false;
		state = openState.CLOSED;
		SetColliders(true);
	}
}
