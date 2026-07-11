using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	public AudioMixerSnapshot musicNormalSnapshot;

	public AudioMixerSnapshot musicPausedSnapshot;

	public AudioMixerSnapshot soundNormalSnapshot;

	public AudioMixerSnapshot soundPausedSnapshot;

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void StopAllCurrentSounds()
	{
		List<SoundGroupVariation> allPlayingVariations = MasterAudio.GetAllPlayingVariations();
		foreach (SoundGroupVariation item in allPlayingVariations)
		{
			item.Stop();
		}
	}

	public void OnGameLoaded()
	{
		Debug.Log("OnGameLoaded");
		StopAllCurrentSounds();
		MasterAudio.OnlyPlaylistController.ChangePlaylist("Game Music");
	}

	public void OnMenuLoaded()
	{
		Debug.Log("on menu loaded");
		StopAllCurrentSounds();
		if (!MasterAudio.OnlyPlaylistController.IsSongPlaying("music_mainmenu_01"))
		{
			MasterAudio.OnlyPlaylistController.ChangePlaylist("Menu Music");
		}
	}

	public void OnPaused()
	{
		musicPausedSnapshot.TransitionTo(0.2f);
		soundPausedSnapshot.TransitionTo(0.2f);
	}

	public void OnContinue()
	{
		musicNormalSnapshot.TransitionTo(0.2f);
		soundNormalSnapshot.TransitionTo(0.2f);
	}

	public void OnGameOver()
	{
	}
}
