using UnityEngine;

public class ChainBehaviour : MonoBehaviour
{
	public Transform mTrans;

	public Rigidbody mRb;

	private void Awake()
	{
		Configure();
	}

	[ContextMenu("Configure")]
	public void Configure()
	{
		mTrans = base.transform;
		mRb = GetComponent<Rigidbody>();
		int childCount = base.transform.childCount;
		float y = base.transform.GetChild(0).GetChild(0).localScale.y;
		Vector3 position = base.transform.GetChild(0).position;
		float num = 0.01f;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (i == 0)
			{
				child.GetComponent<HingeJoint>().connectedBody = mRb;
			}
			else
			{
				child.GetComponent<HingeJoint>().connectedBody = mTrans.GetChild(i - 1).GetComponent<Rigidbody>();
				child.transform.position = position - Vector3.up * (y + num);
			}
			child.GetComponent<HingeJoint>().axis = new Vector3(0f, 0f, 1f);
			position = child.position;
		}
	}
}
