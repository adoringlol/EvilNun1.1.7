using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public int lastScene;

	private void Awake()
	{
		lastScene = 0;
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		GameAnalytics.Initialize();
		OnLoadScene(1);
	}

	[ContextMenu("DeletePrefs")]
	public void DeletePrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public void OnLoadScene(int index)
	{
		TransitionManager.instance.FadeOut(index, 1f);
	}

	public void OnSceneLoaded(Scene sc)
	{
		if (sc.buildIndex == 2 || sc.buildIndex == 1)
		{
			MusicManager.instance.OnMenuLoaded();
		}
		else if (sc.buildIndex == 3)
		{
			MusicManager.instance.OnGameLoaded();
		}
		if (lastScene == 3 && sc.buildIndex == 1)
		{
			AdsManager.instance.OnBackToMenu();
		}
		if (lastScene == 0 && sc.buildIndex == 1)
		{
			AnalyticsController.instance.OnShowMainMenu();
		}
		else if (lastScene == 1 && sc.buildIndex == 2)
		{
			AnalyticsController.instance.OnShowMatchMenu();
		}
		lastScene = sc.buildIndex;
	}
}
