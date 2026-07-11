using System.Collections;
using UnityEngine;

public class SkeletonLightBehaviour : MonoBehaviour
{
	public Light l;

	public void OnLightEnabled()
	{
		StartCoroutine("LightFlickering");
	}

	public IEnumerator LightFlickering()
	{
		while (true)
		{
			l.enabled = true;
			yield return new WaitForSeconds(Random.Range(0.5f, 4f));
			l.enabled = false;
			yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
			l.enabled = true;
			yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
			l.enabled = false;
			yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
		}
	}
}
