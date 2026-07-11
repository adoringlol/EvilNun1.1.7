using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
	public static PauseController instance;

	public bool paused;

	public GameObject panel;

	public CustomSlider musicSlider;

	public CustomSlider soundSlider;

	public CustomCheckBoxBehaviour adsCheckbox;

	public GameObject closeQuestionWindow;

	private void Awake()
	{
		instance = this;
		if (panel == null)
		{
			GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
			for (int i = 0; i < objects.Length; i++)
			{
				if (objects[i].name == "PausePanel" && objects[i].scene == gameObject.scene)
				{
					panel = objects[i];
					break;
				}
			}
		}
	}

	private void Start()
	{
		if (musicSlider != null) musicSlider.Initialize();
		if (soundSlider != null) soundSlider.Initialize();
		if (adsCheckbox != null && VariablesGlobales.adsEnabled)
		{
			adsCheckbox.OnSelect();
		}
		else if (adsCheckbox != null)
		{
			adsCheckbox.OnDeselect();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause && SceneManager.GetActiveScene().buildIndex == 3 && TransitionManager.instance != null && !TransitionManager.instance.loading && GameUIController.instance != null && !GameUIController.instance.gameOver && !GameUIController.instance.completed)
		{
			OnPaused();
		}
	}

	public void OnPaused()
	{
		MasterAudio.PlaySound("button");
		paused = true;
		if (panel != null) panel.SetActive(true);
		if (MusicManager.instance != null) MusicManager.instance.OnPaused();
		Time.timeScale = 0f;
	}

	public void ToggleAdsState()
	{
		if (VariablesGlobales.adsEnabled)
		{
			adsCheckbox.OnSelect();
		}
		else
		{
			adsCheckbox.OnDeselect();
		}
	}

	public void OnContinue()
	{
		MasterAudio.PlaySound("button");
		if (GameUIController.instance != null && GameUIController.instance.j != null) GameUIController.instance.j.Reset();
		paused = false;
		if (panel != null) panel.SetActive(false);
		if (MusicManager.instance != null) MusicManager.instance.OnContinue();
		Time.timeScale = 1f;
		if (musicSlider != null) musicSlider.OnSaveChanges();
		if (soundSlider != null) soundSlider.OnSaveChanges();
	}

	public void AcceptBackToMenu()
	{
		MasterAudio.PlaySound("button");
		Time.timeScale = 1f;
		GameUIController.instance.OnClickMenu();
	}

	public void DeclineBackToMenu()
	{
		MasterAudio.PlaySound("button");
		closeQuestionWindow.SetActive(false);
	}

	public void BackToMenu()
	{
		AnalyticsController.instance.OnGameClose(ProgressManager.instance.day);
		closeQuestionWindow.SetActive(true);
	}
}
