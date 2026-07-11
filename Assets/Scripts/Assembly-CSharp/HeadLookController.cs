using System;
using System.Collections;
using UnityEngine;

public class HeadLookController : MonoBehaviour
{
	public Transform rootNode;

	public BendingSegment[] segments;

	public NonAffectedJoints[] nonAffectedJoints;

	public Vector3 headLookVector = Vector3.forward;

	public Vector3 headUpVector = Vector3.up;

	public Vector3 target = Vector3.zero;

	public float effect = 1f;

	public bool overrideAnimation;

	private void Start()
	{
		if (rootNode == null)
		{
			rootNode = base.transform;
		}
		BendingSegment[] array = segments;
		foreach (BendingSegment bendingSegment in array)
		{
			Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
			Quaternion quaternion = Quaternion.Inverse(rotation);
			bendingSegment.referenceLookDir = quaternion * rootNode.rotation * headLookVector.normalized;
			bendingSegment.referenceUpDir = quaternion * rootNode.rotation * headUpVector.normalized;
			bendingSegment.angleH = 0f;
			bendingSegment.angleV = 0f;
			bendingSegment.dirUp = bendingSegment.referenceUpDir;
			bendingSegment.chainLength = 1;
			Transform transform = bendingSegment.lastTransform;
			while (transform != bendingSegment.firstTransform && transform != transform.root)
			{
				bendingSegment.chainLength++;
				transform = transform.parent;
			}
			bendingSegment.origRotations = new Quaternion[bendingSegment.chainLength];
			transform = bendingSegment.lastTransform;
			for (int num = bendingSegment.chainLength - 1; num >= 0; num--)
			{
				bendingSegment.origRotations[num] = transform.localRotation;
				transform = transform.parent;
			}
		}
	}

	private void LateUpdate()
	{
		if (Time.timeScale == 0f || ZombieBehaviour.instance.ghostMode || (ZombieBehaviour.instance.state != ZombieBehaviour.NunState.SEEN_PLAYER && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.HEAR_NOISE && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.FOLLOWING_PLAYER && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.ATTACK && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.LOOKING_DOLL && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.ON_GUM_FEET && !PlayersManager.instance.GetCurrentController().dead))
		{
			return;
		}
		target = Camera.main.transform.position;
		Vector3[] array = new Vector3[nonAffectedJoints.Length];
		for (int i = 0; i < nonAffectedJoints.Length; i++)
		{
			{
				IEnumerator enumerator = nonAffectedJoints[i].joint.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						array[i] = transform.position - nonAffectedJoints[i].joint.position;
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
		BendingSegment[] array2 = segments;
		foreach (BendingSegment bendingSegment in array2)
		{
			Transform transform2 = bendingSegment.lastTransform;
			if (overrideAnimation)
			{
				for (int num = bendingSegment.chainLength - 1; num >= 0; num--)
				{
					transform2.localRotation = bendingSegment.origRotations[num];
					transform2 = transform2.parent;
				}
			}
			Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
			Quaternion quaternion = Quaternion.Inverse(rotation);
			Vector3 normalized = (target - bendingSegment.lastTransform.position).normalized;
			Vector3 vector = quaternion * normalized;
			float f = AngleAroundAxis(bendingSegment.referenceLookDir, vector, bendingSegment.referenceUpDir);
			Vector3 axis = Vector3.Cross(bendingSegment.referenceUpDir, vector);
			Vector3 dirA = vector - Vector3.Project(vector, bendingSegment.referenceUpDir);
			float f2 = AngleAroundAxis(dirA, vector, axis);
			float f3 = Mathf.Max(0f, Mathf.Abs(f) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(f);
			float f4 = Mathf.Max(0f, Mathf.Abs(f2) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(f2);
			f = Mathf.Max(Mathf.Abs(f3) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(f) - bendingSegment.maxAngleDifference) * Mathf.Sign(f) * Mathf.Sign(bendingSegment.bendingMultiplier);
			f2 = Mathf.Max(Mathf.Abs(f4) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(f2) - bendingSegment.maxAngleDifference) * Mathf.Sign(f2) * Mathf.Sign(bendingSegment.bendingMultiplier);
			f = Mathf.Clamp(f, 0f - bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
			f2 = Mathf.Clamp(f2, 0f - bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
			Vector3 axis2 = Vector3.Cross(bendingSegment.referenceUpDir, bendingSegment.referenceLookDir);
			bendingSegment.angleH = Mathf.Lerp(bendingSegment.angleH, f, Time.deltaTime * bendingSegment.responsiveness);
			bendingSegment.angleV = Mathf.Lerp(bendingSegment.angleV, f2, Time.deltaTime * bendingSegment.responsiveness);
			vector = Quaternion.AngleAxis(bendingSegment.angleH, bendingSegment.referenceUpDir) * Quaternion.AngleAxis(bendingSegment.angleV, axis2) * bendingSegment.referenceLookDir;
			Vector3 tangent = bendingSegment.referenceUpDir;
			Vector3.OrthoNormalize(ref vector, ref tangent);
			Vector3 normal = vector;
			bendingSegment.dirUp = Vector3.Slerp(bendingSegment.dirUp, tangent, Time.deltaTime * 5f);
			Vector3.OrthoNormalize(ref normal, ref bendingSegment.dirUp);
			Quaternion b = rotation * Quaternion.LookRotation(normal, bendingSegment.dirUp) * Quaternion.Inverse(rotation * Quaternion.LookRotation(bendingSegment.referenceLookDir, bendingSegment.referenceUpDir));
			Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, b, effect / (float)bendingSegment.chainLength);
			transform2 = bendingSegment.lastTransform;
			for (int k = 0; k < bendingSegment.chainLength; k++)
			{
				transform2.rotation = quaternion2 * transform2.rotation;
				transform2 = transform2.parent;
			}
		}
		for (int l = 0; l < nonAffectedJoints.Length; l++)
		{
			Vector3 vector2 = Vector3.zero;
			{
				IEnumerator enumerator2 = nonAffectedJoints[l].joint.GetEnumerator();
				try
				{
					if (enumerator2.MoveNext())
					{
						Transform transform3 = (Transform)enumerator2.Current;
						vector2 = transform3.position - nonAffectedJoints[l].joint.position;
					}
				}
				finally
				{
					IDisposable disposable2 = enumerator2 as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
				}
			}
			Vector3 toDirection = Vector3.Slerp(array[l], vector2, nonAffectedJoints[l].effect);
			nonAffectedJoints[l].joint.rotation = Quaternion.FromToRotation(vector2, toDirection) * nonAffectedJoints[l].joint.rotation;
		}
	}

	public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}
}
