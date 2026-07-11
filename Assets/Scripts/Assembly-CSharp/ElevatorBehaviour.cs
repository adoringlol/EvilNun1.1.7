using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Analytics;

public class ElevatorBehaviour : MonoBehaviour
{
	public static ElevatorBehaviour instance;

	public float currentWeight;

	public Transform aguja;

	public float minRotation = 30f;

	public float maxRotation = -30f;

	public Animation anim;

	public bool works = true;

	public ElevatorCageTrigger cagetrigger;

	public Transform itemPosition;

	public TakeableObject explosiveBomb;

	public TakeableObject craftedgun;

	public bool busy;

	public ElevatorMechanicSystemBehaviour mechanic;

	public AudioClip loopClip;

	public AudioClip finishClip;

	public GameObject helpNote;

	public bool noteUsed;

	private void Awake()
	{
		instance = this;
		works = false;
	}

	public void UpdateWeight(List<Collider> currentItems)
	{
		float num = 0f;
		foreach (Collider currentItem in currentItems)
		{
			if ((bool)currentItem.gameObject.GetComponent<TakeableObject>())
			{
				num += currentItem.gameObject.GetComponent<TakeableObject>().weight;
			}
		}
		currentWeight = num;
		aguja.transform.DOLocalRotate(new Vector3(60f, 0f, Mathf.Lerp(30f, -30f, currentWeight / 2f)), 0.3f);
		if (!works && currentWeight >= 2f)
		{
			MasterAudio.PlaySound("gears");
			works = true;
			MessagesManager.instance.ShowMessage("elevator_fix_weight");
			AnalyticsController.instance.OnSolvePuzzle("elevator_fix");
			MasterAudio.PlaySound("puzle_complete");
			AnalyticsEvent.Custom("elevator_weight");
			UseElevator();
		}
	}

	public void UseElevator()
	{
		busy = true;
		StartCoroutine("GoDown");
	}

	public IEnumerator GoDown()
	{
		PlaySoundResult playingAudio = MasterAudio.PlaySound3DFollowTransform("lever_movement_7s", cagetrigger.transform);
		anim["go_down"].normalizedTime = 0f;
		anim["go_down"].speed = 1f;
		anim.Play("go_down");
		yield return new WaitForSeconds(anim["go_down"].length);
		playingAudio.ActingVariation.FadeOutNow(0.25f);
		MessagesManager.instance.ShowMessage("elevator_on_use", "20");
		MasterAudio.PlaySound3DAtVector3("lever_end_movement", cagetrigger.transform.position);
		yield return new WaitForSeconds(20f);
		StartCoroutine("GoUp");
	}

	public IEnumerator GoUp()
	{
		if (!noteUsed)
		{
			noteUsed = true;
			helpNote.SetActive(true);
		}
		PlaySoundResult playingAudio = MasterAudio.PlaySound3DFollowTransform("lever_movement_7s", cagetrigger.transform);
		if (cagetrigger.IsMaterialForDoll())
		{
			explosiveBomb.ResetScale();
			AnalyticsController.instance.OnSolvePuzzle("craft-doll");
			MasterAudio.PlaySound("puzle_complete");
			AnalyticsEvent.Custom("craft_doll");
			explosiveBomb.transform.parent = cagetrigger.transform;
			explosiveBomb.transform.localPosition = itemPosition.localPosition;
			explosiveBomb.transform.localEulerAngles = Vector3.zero;
			explosiveBomb.rb.velocity = Vector3.zero;
			explosiveBomb.rb.isKinematic = false;
			explosiveBomb.gameObject.SetActive(true);
		}
		if (cagetrigger.IsMaterialForGun())
		{
			craftedgun.ResetScale();
			AnalyticsController.instance.OnSolvePuzzle("craft-gun");
			AnalyticsEvent.Custom("craft_gun");
			MasterAudio.PlaySound("puzle_complete");
			craftedgun.transform.parent = cagetrigger.transform;
			craftedgun.transform.localPosition = itemPosition.localPosition;
			craftedgun.transform.localEulerAngles = new Vector3(0f, 70f, 0f);
			craftedgun.rb.velocity = Vector3.zero;
			craftedgun.rb.isKinematic = false;
			craftedgun.gameObject.SetActive(true);
		}
		yield return null;
		anim["go_down"].normalizedTime = 1f;
		anim["go_down"].speed = -1f;
		anim.Play("go_down");
		yield return new WaitForSeconds(anim["go_down"].length);
		playingAudio.ActingVariation.FadeOutNow(0.25f);
		MasterAudio.PlaySound3DAtVector3("lever_end_movement", cagetrigger.transform.position);
		busy = false;
	}
}
