using System;
using UnityEngine;

namespace DarkTonic.MasterAudio
{
	[Serializable]
	public class MusicSetting
	{
		public string alias = string.Empty;

		public MasterAudio.AudioLocation audLocation;

		public AudioClip clip;

		public string songName = string.Empty;

		public string resourceFileName = string.Empty;

		public float volume = 1f;

		public float pitch = 1f;

		public bool isExpanded = true;

		public bool isLoop;

		public MasterAudio.CustomSongStartTimeMode songStartTimeMode;

		public float customStartTime;

		public float customStartTimeMax;

		public int lastKnownTimePoint;

		public bool wasLastKnownTimePointSet;

		public int songIndex;

		public bool songStartedEventExpanded;

		public string songStartedCustomEvent = string.Empty;

		public bool songChangedEventExpanded;

		public string songChangedCustomEvent = string.Empty;

		public float SongStartTime
		{
			get
			{
				switch (songStartTimeMode)
				{
				default:
					return 0f;
				case MasterAudio.CustomSongStartTimeMode.SpecificTime:
					return customStartTime;
				case MasterAudio.CustomSongStartTimeMode.RandomTime:
					return UnityEngine.Random.Range(customStartTime, customStartTimeMax);
				}
			}
		}

		public MusicSetting()
		{
			songChangedEventExpanded = false;
		}

		public static MusicSetting Clone(MusicSetting mus)
		{
			MusicSetting musicSetting = new MusicSetting();
			musicSetting.alias = mus.alias;
			musicSetting.audLocation = mus.audLocation;
			musicSetting.clip = mus.clip;
			musicSetting.songName = mus.songName;
			musicSetting.resourceFileName = mus.resourceFileName;
			musicSetting.volume = mus.volume;
			musicSetting.pitch = mus.pitch;
			musicSetting.isExpanded = mus.isExpanded;
			musicSetting.isLoop = mus.isLoop;
			musicSetting.customStartTime = mus.customStartTime;
			musicSetting.songStartedEventExpanded = mus.songStartedEventExpanded;
			musicSetting.songStartedCustomEvent = mus.songStartedCustomEvent;
			musicSetting.songChangedEventExpanded = mus.songChangedEventExpanded;
			musicSetting.songChangedCustomEvent = mus.songChangedCustomEvent;
			return musicSetting;
		}
	}
}
