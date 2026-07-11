using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;

public class WaterMechanismBehaviour : TouchableElement
{
	private bool used;

	public Vector3 finishRotation;

	public GameObject dirtyWater;

	public TouchableMessage toilet;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!used)
		{
			interactuable = false;
			used = true;
			toilet.interactuable = false;
			MasterAudio.PlaySound3DAtVector3("fix_water", base.transform.position);
			base.transform.DOLocalRotate(finishRotation, 2f, RotateMode.FastBeyond360);
			dirtyWater.SetActive(true);
		}
	}
}
