using UnityEngine;

public class ASReaderBehaviour : MonoBehaviour
{
	public AudioClip clip;

	[ContextMenu("ReadClip")]
	public void ReadClip()
	{
		clip = GetComponent<AudioSource>().clip;
	}
}
