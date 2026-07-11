using UnityEngine;

namespace DarkTonic.MasterAudio
{
	public static class SpatializerHelper
	{
		private const string OculusSpatializer = "OculusSpatializer";

		private const string ResonanceAudioSpatializer = "Resonance Audio";

		public static bool IsSupportedSpatializer
		{
			get
			{
				switch (SelectedSpatializer)
				{
				case "OculusSpatializer":
					return true;
				case "Resonance Audio":
					return true;
				default:
					return false;
				}
			}
		}

		public static bool IsResonanceAudioSpatializer
		{
			get
			{
				return SelectedSpatializer == "Resonance Audio";
			}
		}

		public static string SelectedSpatializer
		{
			get
			{
				return AudioSettings.GetSpatializerPluginName();
			}
		}

		public static bool SpatializerOptionExists
		{
			get
			{
				return true;
			}
		}

		public static void TurnOnSpatializerIfEnabled(AudioSource source)
		{
			if (SpatializerOptionExists && !(MasterAudio.SafeInstance == null) && MasterAudio.Instance.useSpatializer)
			{
				source.spatialize = true;
				if (ResonanceAudioHelper.ResonanceAudioOptionExists && MasterAudio.Instance.useSpatializerPostFX)
				{
					source.spatializePostEffects = true;
				}
			}
		}
	}
}
