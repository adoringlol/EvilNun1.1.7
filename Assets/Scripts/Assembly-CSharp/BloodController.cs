using UnityEngine;

public class BloodController : MonoBehaviour
{
	public GameObject[] bloodDecals;

	private void Start()
	{
		GameObject[] array = bloodDecals;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(VariablesGlobales.bloodEnabled);
		}
	}
}
