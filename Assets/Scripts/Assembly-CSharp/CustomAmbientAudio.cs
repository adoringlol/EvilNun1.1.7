using DarkTonic.MasterAudio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CustomAmbientAudio : MonoBehaviour
{
	public enum AmbientSoundState
	{
		NONE = 0,
		FADE_IN = 1,
		PLAYING = 2,
		FADE_OUT = 3
	}

	[SoundGroup]
	public string ambient_sound_name;

	public AmbientSoundState _state;

	public float fadeInTime;

	public float fadeOutTime;

	public int zoneCount;

	public PlaySoundResult currentSound;

	private AmbientSoundState state
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}

	private void Start()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		BoxCollider[] components = GetComponents<BoxCollider>();
		foreach (BoxCollider boxCollider in components)
		{
			boxCollider.isTrigger = true;
		}
		state = AmbientSoundState.NONE;
		CustomAmbientAudioZone[] componentsInChildren = base.transform.GetComponentsInChildren<CustomAmbientAudioZone>();
		foreach (CustomAmbientAudioZone customAmbientAudioZone in componentsInChildren)
		{
			customAmbientAudioZone.main = this;
		}
	}

	public void StartAmbient()
	{
		if (!MasterAudio.IsSoundGroupPlaying(ambient_sound_name))
		{
			currentSound = MasterAudio.PlaySound(ambient_sound_name, 0f);
		}
		FadeIn();
	}

	public void FadeIn()
	{
		state = AmbientSoundState.FADE_IN;
		if (currentSound != null && currentSound.ActingVariation != null)
		{
			float newVolume = Mathf.Lerp(0f, currentSound.ActingVariation.ParentGroup.OriginalVolume, PauseController.instance.soundSlider.mSlider.value);
			currentSound.ActingVariation.FadeToVolume(newVolume, fadeInTime);
			CancelInvoke("OnFadeInFinished");
			Invoke("OnFadeInFinished", fadeInTime);
		}
	}

	public void OnFadeInFinished()
	{
		if (state == AmbientSoundState.FADE_IN)
		{
			state = AmbientSoundState.PLAYING;
		}
	}

	public void FadeOut()
	{
		state = AmbientSoundState.FADE_OUT;
		if (currentSound != null && currentSound.ActingVariation != null)
		{
			currentSound.ActingVariation.FadeToVolume(0f, fadeOutTime);
			CancelInvoke("OnFadeOutFinished");
			Invoke("OnFadeOutFinished", fadeOutTime);
		}
	}

	public void OnFadeOutFinished()
	{
		if (state == AmbientSoundState.FADE_OUT)
		{
			state = AmbientSoundState.NONE;
			currentSound.ActingVariation.Stop();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			zoneCount++;
			OnEnterZone();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			zoneCount--;
			OnExitZone();
		}
	}

	public void OnEnterZone()
	{
		zoneCount++;
		ResolveFading();
	}

	public void OnExitZone()
	{
		zoneCount--;
		ResolveFading();
	}

	public void ResolveFading()
	{
		if (zoneCount == 0 && state != AmbientSoundState.FADE_OUT)
		{
			FadeOut();
		}
		else if (state != AmbientSoundState.FADE_IN)
		{
			StartAmbient();
		}
	}
}
