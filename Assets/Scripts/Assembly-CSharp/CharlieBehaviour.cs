using DG.Tweening;
using UnityEngine;

public class CharlieBehaviour : TouchableElement
{
	public Transform lapiz;

	public float min = 1f;

	public float max = 3f;

	public float[] possibleRotations;

	public AnimationCurve[] possibleCurves;

	public bool moving;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!moving)
		{
			moving = true;
			interactuable = false;
			lapiz.DOLocalRotate(new Vector3(0f, possibleRotations[Random.Range(0, possibleRotations.Length)], 0f), Random.Range(4f, 8f)).SetEase(possibleCurves[Random.Range(0, possibleCurves.Length)]).OnComplete(AnimationFinish);
		}
	}

	public void AnimationFinish()
	{
		moving = false;
		interactuable = true;
	}
}
