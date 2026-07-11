using UnityEngine;

public class BloodDecalsBehaviour : MonoBehaviour
{
	private void OnEnable()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).localEulerAngles = new Vector3(0f, 0f, Random.Range(0, 360));
		}
	}
}
