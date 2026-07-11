public static class VariablesGlobales
{
	public static bool musicEnabled;

	public static float musicVolume = 1f;

	public static bool soundEnabled;

	public static float soundVolume = 1f;

	public static string currentLanguage;

	public static int plays;

	public static string randomName;

	public static bool _removeAdsBought;

	public static bool adsEnabled = true;

	public static bool tutorialEnabled = true;

	public static bool randomYoutubeName;

	public static bool ownName;

	public static string playerOwnName;

	public static int difficultyMode = 1;

	public static int qualityMode = 1;

	public static float distanceToInteract = 1.5f;

	public static bool bloodEnabled = true;

	public static string youtuberName;

	public static bool removeAdsBought
	{
		get
		{
			return _removeAdsBought;
		}
		set
		{
			_removeAdsBought = value;
		}
	}
}
