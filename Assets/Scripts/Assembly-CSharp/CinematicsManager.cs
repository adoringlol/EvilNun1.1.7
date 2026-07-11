using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class CinematicsManager : MonoBehaviour
{
	public static CinematicsManager instance;

	public Camera camera;

	public Camera gameCam;

	public GameObject exterior;

	public GameObject blackScreen;

	public Transform nunPositionInGameOver01;

	public Transform gameover1AnimationParent;

	public GameObject gameOver1AnimationObject;

	public Animation gameOver1Anim;

	public GameObject mask;

	public Vector3 exitSchoolCameraPosition;

	public Vector3 exitSchoolCameraRotation;

	public Transform laundryDoor;

	public Transform cartel;

	public GameObject nunMask;

	[SoundGroup]
	public string finalDoor1;

	[SoundGroup]
	public string finishMusic;

	[SoundGroup]
	public string exteriorAmbient;

	private void Awake()
	{
		instance = this;
	}

	private void StopAmbient()
	{
		List<SoundGroupVariation> allPlayingVariationsInBus = MasterAudio.GetAllPlayingVariationsInBus("Ambient");
		foreach (SoundGroupVariation item in allPlayingVariationsInBus)
		{
			item.Stop();
		}
	}

	[ContextMenu("PrepareExitSchoolCinematic")]
	public void PrepareExitSchoolCinematic()
	{
		AnalyticsController.instance.OnCompleteGame(ProgressManager.instance.day);
		GameUIController.instance.centerPoint.SetActive(false);
		PlayersManager.instance.GetCurrentController().BlockPlayer();
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand)
		{
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
			PlayersManager.instance.GetCurrentController().objectOnHand.gameObject.SetActive(false);
			PlayersManager.instance.GetCurrentController().objectOnHand = null;
		}
		if (ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.BlockNun();
		}
		List<SoundGroupVariation> allPlayingVariationsInBus = MasterAudio.GetAllPlayingVariationsInBus("Ambient");
		foreach (SoundGroupVariation item in allPlayingVariationsInBus)
		{
			item.FadeToVolume(0f, 1f);
		}
		MasterAudio.StopAllPlaylists();
		MasterAudio.PlaySound(exteriorAmbient, 0.5f, 1f, 0.5f);
		MasterAudio.PlaySound(finalDoor1);
		MasterAudio.PlaySound(finishMusic);
		camera.gameObject.SetActive(true);
		gameCam.GetComponent<AudioListener>().enabled = false;
		camera.depth = -1f;
		camera.transform.position = exitSchoolCameraPosition;
		camera.transform.eulerAngles = exitSchoolCameraRotation;
		CameraMoveAnimation.instance.StopAllAnimations();
		SimpleSmoothMouseLook.instance.canLook = false;
		SimpleSmoothMouseLook.instance.transform.DOMove(camera.transform.position, 1f).OnComplete(StartExitSchoolCinematic);
		SimpleSmoothMouseLook.instance.transform.DORotate(camera.transform.eulerAngles, 1f).OnComplete(StartExitSchoolCinematic);
	}

	public void StartExitSchoolCinematic()
	{
		camera.depth = 1f;
		gameCam.enabled = false;
		camera.GetComponent<Animation>().Play("complete_game_01");
		Invoke("ShowFinish", camera.GetComponent<Animation>()["complete_game_01"].length - 1f);
	}

	public void ShowFinish()
	{
		GameUIController.instance.ShowGameCompletePanel();
	}

	[ContextMenu("StartGameOverCinematic")]
	public void StartGameOverCinematic()
	{
		PlayersManager.instance.GetCurrentController().BlockPlayer();
		if (ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.BlockNun();
		}
		StartCoroutine("GameOverCinematicRoutine");
	}

	public IEnumerator GameOverCinematicRoutine()
	{
		blackScreen.SetActive(true);
		GameUIController.instance.centerPoint.SetActive(false);
		yield return blackScreen.GetComponent<Image>().DOFade(1f, 1f).WaitForCompletion();
		MasterAudio.StopAllPlaylists();
		List<SoundGroupVariation> list = MasterAudio.GetAllPlayingVariationsInBus("Ambient");
		foreach (SoundGroupVariation item in list)
		{
			item.FadeToVolume(0f, 1f);
		}
		GameUIController.instance.SetBloodInCamera(true);
		yield return new WaitForSeconds(2f);
		PlayersManager.instance.child.dead = false;
		cartel.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		laundryDoor.transform.eulerAngles = new Vector3(0f, 125f, 0f);
		ZombieBehaviour.instance.hammer.SetActive(false);
		camera.gameObject.SetActive(true);
		gameCam.GetComponent<AudioListener>().enabled = false;
		camera.farClipPlane = 30f;
		camera.depth = 1f;
		ZombieBehaviour.instance.transform.position = nunPositionInGameOver01.position;
		ZombieBehaviour.instance.nunModel.transform.localPosition = new Vector3(ZombieBehaviour.instance.nunModel.localPosition.x, -0.86f, ZombieBehaviour.instance.nunModel.localPosition.z);
		ZombieBehaviour.instance.transform.eulerAngles = nunPositionInGameOver01.eulerAngles;
		ZombieBehaviour.instance.anim.Play("final1");
		gameOver1AnimationObject.SetActive(true);
		camera.transform.parent = gameover1AnimationParent;
		camera.transform.localPosition = Vector3.zero;
		camera.transform.localEulerAngles = new Vector3(-90f, -180f, 0f);
		gameOver1Anim.Play();
		yield return blackScreen.GetComponent<Image>().DOFade(0f, 2.5f).WaitForCompletion();
		blackScreen.SetActive(false);
		yield return new WaitForSeconds(ZombieBehaviour.instance.anim["final1"].length - 2.5f);
		yield return laundryDoor.transform.DORotate(new Vector3(0f, 0f, 0f), 1f).WaitForCompletion();
		GameUIController.instance.ShowGameOver();
		yield return new WaitForSeconds(1f);
	}
}
