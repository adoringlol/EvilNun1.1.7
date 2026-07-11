using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MuseumBehaviour : MonoBehaviour
{
	public static MuseumBehaviour instance;

	public DateTime startedDateTime;

	public DateTime dateTime;

	public bool doorUnlocked;

	public TextMeshProUGUI hours;

	public TextMeshProUGUI minutes;

	public TextMeshProUGUI seconds;

	public int turnedOnLights;

	public GameObject[] cuadros;

	public GameObject[] lights;

	public OpenCloseBehaviour tapa;

	public int unlockTimeInSeconds = 120;

	[ContextMenu("DeleteMuseumPlayerPrefs")]
	public void DeleteMuseumPlayerPrefs()
	{
		PlayerPrefs.DeleteKey("museum_door");
		PlayerPrefs.DeleteKey("museum_lights");
	}

	[ContextMenu("SetOnMuseumLights")]
	public void SetOnMuseumLights()
	{
		dateTime = UnbiasedTime.Instance.Now();
		startedDateTime = dateTime;
		PlayerPrefs.SetString("museum_door", startedDateTime.ToString());
		PlayerPrefs.SetInt("museum_lights", 7);
		turnedOnLights = 7;
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		CheckTimer();
	}

	private void RegisterForNotifications()
	{
	}

	private void OpenTapa()
	{
		if (tapa != null) tapa.Touched();
	}

	private void CheckTimer()
	{
		if (PlayerPrefs.HasKey("museum_door"))
		{
			startedDateTime = DateTime.Parse(PlayerPrefs.GetString("museum_door"));
			turnedOnLights = PlayerPrefs.GetInt("museum_lights");
			if (turnedOnLights == 0)
			{
				unlockTimeInSeconds = 86400;
			}
			else
			{
				unlockTimeInSeconds = 86400;
			}
			if (NotificationsManager.instance != null) NotificationsManager.instance.ScheduleSimple(unlockTimeInSeconds);
		}
		else
		{
			Invoke("OpenTapa", 1f);
			dateTime = UnbiasedTime.Instance.Now();
			startedDateTime = dateTime;
			PlayerPrefs.SetString("museum_door", startedDateTime.ToString());
			PlayerPrefs.SetInt("museum_lights", 0);
			turnedOnLights = 0;
			unlockTimeInSeconds = 86400;
			if (NotificationsManager.instance != null) NotificationsManager.instance.ScheduleSimple(unlockTimeInSeconds);
		}
		UpdateLights();
		UpdateCuadros();
		if (turnedOnLights < 7)
		{
			StartCoroutine("TimerBehaviour");
			return;
		}
		if (hours != null) hours.gameObject.SetActive(false);
		if (minutes != null) minutes.gameObject.SetActive(false);
		if (seconds != null) seconds.gameObject.SetActive(false);
	}

	private IEnumerator TimerBehaviour()
	{
		while (true)
		{
			dateTime = UnbiasedTime.Instance.Now();
			TimeSpan dt = new TimeSpan(0, 0, unlockTimeInSeconds);
			TimeSpan rest = dateTime - startedDateTime;
			dt -= rest;
			if (hours != null) hours.text = dt.Hours.ToString("00");
			if (minutes != null) minutes.text = dt.Minutes.ToString("00");
			if (seconds != null) seconds.text = dt.Seconds.ToString("00");
			if (dt.Hours <= 0 && dt.Minutes <= 0 && dt.Seconds <= 0)
			{
				turnedOnLights++;
				unlockTimeInSeconds = 86400;
				startedDateTime = dateTime;
				if (NotificationsManager.instance != null) NotificationsManager.instance.ScheduleSimple(unlockTimeInSeconds);
				PlayerPrefs.SetString("museum_door", startedDateTime.ToString());
				PlayerPrefs.SetInt("museum_lights", turnedOnLights);
				if (AnalyticsController.instance != null) AnalyticsController.instance.OnUnlockNewPicture(turnedOnLights);
				UpdateLights();
				UpdateCuadros();
				yield return null;
				if (turnedOnLights >= 7)
				{
					break;
				}
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
		}
		if (hours != null) hours.gameObject.SetActive(false);
		if (minutes != null) minutes.gameObject.SetActive(false);
		if (seconds != null) seconds.gameObject.SetActive(false);
	}

	public void UpdateLights()
	{
		if (lights == null) return;
		for (int i = 0; i < lights.Length; i++)
		{
			if (lights[i] != null) lights[i].SetActive(false);
		}
		for (int j = 0; j < lights.Length; j++)
		{
			if (j < turnedOnLights)
			{
				if (lights[j] != null) lights[j].SetActive(true);
			}
		}
	}

	public void UpdateCuadros()
	{
		if (cuadros == null) return;
		for (int i = 0; i < cuadros.Length; i++)
		{
			if (cuadros[i] != null) cuadros[i].SetActive(false);
		}
		for (int j = 0; j < cuadros.Length; j++)
		{
			if (j < turnedOnLights)
			{
				if (cuadros[j] != null) cuadros[j].SetActive(true);
			}
		}
	}
}
