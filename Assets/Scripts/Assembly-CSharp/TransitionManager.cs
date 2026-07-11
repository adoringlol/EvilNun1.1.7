using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
	public static TransitionManager instance;

	public Image fadeBackground;

	private Canvas mCanvas;

	public int targetScene;

	public GameObject content;

	public GameObject informationTexts;

	public bool loading;

	private void Awake()
	{
		instance = this;
		mCanvas = GetComponent<Canvas>();
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void FadeOut(int sceneIndex, float duration)
	{
		loading = true;
		targetScene = sceneIndex;
		mCanvas.enabled = true;
		fadeBackground.raycastTarget = true;
		informationTexts.SetActive(sceneIndex == 3);
		StartCoroutine(DOAlpha(1f, 0.5f));
		fadeBackground.DOKill();
		fadeBackground.DOFade(1f, duration).OnComplete(FadeOutFinished);
	}

	public void FadeIn(float duration)
	{
		fadeBackground.DOKill();
		fadeBackground.raycastTarget = false;
		mCanvas.enabled = true;
		informationTexts.SetActive(targetScene == 3);
		StartCoroutine(DOAlpha(0f, 0.5f));
		fadeBackground.DOFade(0f, duration).OnComplete(FadeInFinished);
	}

	public IEnumerator DOAlpha(float targetAlpha, float duration)
	{
		CanvasGroup alphaObj = content.GetComponent<CanvasGroup>();
		float startAlpha = alphaObj.alpha;
		float timer = 0f;
		while (timer / duration < 1f)
		{
			timer += Time.deltaTime;
			alphaObj.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
			yield return null;
		}
	}

	private void FadeInFinished()
	{
		mCanvas.enabled = false;
		loading = false;
	}

	public void FadeOutFinished()
	{
		SceneManager.LoadScene(targetScene);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex <= 0 || !(GameManager.instance == null))
		{
			if (scene.buildIndex > 0)
			{
				FadeIn(1f);
			}
			if (scene.buildIndex > 0)
			{
				GameManager.instance.OnSceneLoaded(scene);
				SceneManager.SetActiveScene(scene);
			}
		}
	}
}
