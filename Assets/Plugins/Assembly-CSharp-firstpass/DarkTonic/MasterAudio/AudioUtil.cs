using UnityEngine;

namespace DarkTonic.MasterAudio
{
	public static class AudioUtil
	{
		public const float DefaultMinOcclusionCutoffFrequency = 22000f;

		public const float DefaultMaxOcclusionCutoffFrequency = 0f;

		private const float SemitonePitchChangeAmt = 1.0594635f;

		public static float FixedDeltaTime
		{
			get
			{
				return UnityEngine.Time.fixedDeltaTime;
			}
		}

		public static float FrameTime
		{
			get
			{
				return UnityEngine.Time.unscaledDeltaTime;
			}
		}

		public static float Time
		{
			get
			{
				return UnityEngine.Time.unscaledTime;
			}
		}

		public static int FrameCount
		{
			get
			{
				return UnityEngine.Time.frameCount;
			}
		}

		private static float CutoffRange(SoundGroupVariationUpdater updater)
		{
			return updater.MinOcclusionFreq - updater.MaxOcclusionFreq;
		}

		private static float MaxCutoffFreq(SoundGroupVariationUpdater updater)
		{
			return updater.MaxOcclusionFreq;
		}

		public static float MinCutoffFreq(SoundGroupVariationUpdater updater)
		{
			return updater.MinOcclusionFreq;
		}

		public static float GetOcclusionCutoffFrequencyByDistanceRatio(float distRatio, SoundGroupVariationUpdater updater)
		{
			return MaxCutoffFreq(updater) + distRatio * CutoffRange(updater);
		}

		public static float GetSemitonesFromPitch(float pitch)
		{
			if (pitch < 1f && pitch > 0f)
			{
				float f = 1f / pitch;
				return Mathf.Log(f, 1.0594635f) * -1f;
			}
			return Mathf.Log(pitch, 1.0594635f);
		}

		public static float GetPitchFromSemitones(float semitones)
		{
			if (semitones >= 0f)
			{
				return Mathf.Pow(1.0594635f, semitones);
			}
			return 1f / Mathf.Pow(1.0594635f, Mathf.Abs(semitones));
		}

		public static float GetDbFromFloatVolume(float vol)
		{
			return Mathf.Log10(vol) * 20f;
		}

		public static float GetFloatVolumeFromDb(float db)
		{
			return Mathf.Pow(10f, db / 20f);
		}

		public static float GetAudioPlayedPercentage(AudioSource source)
		{
			if (source.clip == null || source.time == 0f)
			{
				return 0f;
			}
			return source.time / source.clip.length * 100f;
		}

		public static bool IsAudioPaused(AudioSource source)
		{
			return !source.isPlaying && GetAudioPlayedPercentage(source) > 0f;
		}

		public static void ClipPlayed(AudioClip clip, GameObject actor)
		{
			if (!AudioClipWillPreload(clip))
			{
				AudioLoaderOptimizer.AddNonPreloadedPlayingClip(clip, actor);
			}
		}

		public static void UnloadNonPreloadedAudioData(AudioClip clip, GameObject actor)
		{
			if (!(clip == null) && !AudioClipWillPreload(clip))
			{
				AudioLoaderOptimizer.RemoveNonPreloadedPlayingClip(clip, actor);
				if (!AudioLoaderOptimizer.IsAnyOfNonPreloadedClipPlaying(clip))
				{
					clip.UnloadAudioData();
				}
			}
		}

		public static bool AudioClipWillPreload(AudioClip clip)
		{
			if (clip == null)
			{
				return false;
			}
			return clip.preloadAudioData;
		}

		public static bool IsClipReadyToPlay(this AudioClip clip)
		{
			return clip != null && clip.loadType != AudioClipLoadType.Streaming;
		}

		private static float GetPositiveUsablePitch(AudioSource source)
		{
			return GetPositiveUsablePitch(source.pitch);
		}

		private static float GetPositiveUsablePitch(float pitch)
		{
			return (!(pitch > 0f)) ? 1f : pitch;
		}

		public static float AdjustAudioClipDurationForPitch(float duration, AudioSource sourceWithPitch)
		{
			return AdjustAudioClipDurationForPitch(duration, sourceWithPitch.pitch);
		}

		public static float AdjustAudioClipDurationForPitch(float duration, float pitch)
		{
			return duration / GetPositiveUsablePitch(pitch);
		}

		public static float AdjustEndLeadTimeForPitch(float duration, AudioSource sourceWithPitch)
		{
			return duration * GetPositiveUsablePitch(sourceWithPitch);
		}
	}
}
