using DarkTonic.MasterAudio;

public static class ResonanceAudioHelper
{
	public static bool ResonanceAudioOptionExists
	{
		get
		{
			return true;
		}
	}

	public static bool AddResonanceAudioSourceToAllVariations()
	{
		return false;
	}

	public static bool RemoveResonanceAudioSourceFromAllVariations()
	{
		return false;
	}

	public static void CopyResonanceAudioSource(DynamicGroupVariation sourceVariation, DynamicGroupVariation destVariation)
	{
	}

	public static void CopyResonanceAudioSource(DynamicGroupVariation sourceVariation, SoundGroupVariation destVariation)
	{
	}

	public static void CopyResonanceAudioSource(SoundGroupVariation sourceVariation, DynamicGroupVariation destVariation)
	{
	}
}
