using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public List<CustomSoundClip> clips;

	public AudioSource source;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
	}

	public void PlaySound(AudioClip clip, float vol = 1f)
	{
	}

	public void PlaySound(string soundName)
	{
		if (clips.Exists((CustomSoundClip c) => c.clipName == soundName))
		{
			CustomSoundClip customSoundClip = clips.Find((CustomSoundClip c) => c.clipName == soundName);
		}
	}

	public void PlaySoundAtPoint(string soundName, Vector3 pos)
	{
		if (clips.Exists((CustomSoundClip c) => c.clipName == soundName))
		{
			CustomSoundClip customSoundClip = clips.Find((CustomSoundClip c) => c.clipName == soundName);
		}
	}

	public AudioClip GetClip(string name)
	{
		return clips.Find((CustomSoundClip c) => c.clipName == name).clip;
	}
}
