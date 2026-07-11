using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class GumgunTakeable : TakeableObject
{
	public GameObject shootPrefab;

	public Transform shootPoint;

	public List<GumBallData> gumBalls;

	public bool dev;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (!PlayersManager.instance.GetCurrentController().isHidden)
		{
			GameUIController.instance.shootButton.SetActive(true);
		}
		base.Touched(byPlayer, trapPlayer);
	}

	public void Shoot()
	{
		List<GumBallData> list = gumBalls.FindAll((GumBallData g) => !g.used);
		if (list.Count == 0)
		{
			Debug.Log("no quedan disparos");
			return;
		}
		GumBallData gumBallData = list[Random.Range(0, list.Count)];
		if (!dev)
		{
			gumBallData.used = true;
			gumBallData.ball.SetActive(false);
		}
		GameObject gameObject = Object.Instantiate(shootPrefab);
		gameObject.transform.position = shootPoint.position;
		gameObject.GetComponent<MeshFilter>().mesh = gumBallData.ballMesh;
		gameObject.GetComponent<ShootBehaviour>().Setdata(gumBallData);
		gameObject.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * 5f, ForceMode.Impulse);
		MasterAudio.PlaySound3DAtVector3("hit_nun", base.transform.position);
	}

	public override void OnEnterHide()
	{
		GameUIController.instance.shootButton.SetActive(false);
		base.OnEnterHide();
	}

	public override void OnExitHide()
	{
		GameUIController.instance.shootButton.SetActive(true);
		base.OnExitHide();
	}

	public override void OnReleaseItem()
	{
		GameUIController.instance.shootButton.SetActive(false);
		base.OnReleaseItem();
	}

	public void ChargeGum(ShootBehaviour shoot)
	{
		GumBallData gumBallData = gumBalls.Find((GumBallData g) => g.ballMesh == shoot.data.ballMesh);
		gumBallData.used = false;
		gumBallData.ball.SetActive(true);
		shoot.GetComponent<TakeableObject>().OnDestroyItem();
	}
}
