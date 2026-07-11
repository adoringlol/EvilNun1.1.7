using DG.Tweening;
using UnityEngine;

public class ShootBehaviour : TakeableObject
{
	public GumBallData data;

	public override void Initialize()
	{
		base.Initialize();
		base.transform.localScale = Vector3.one * 0.1f;
		base.transform.DOScale(1f, 0.3f);
	}

	public void Setdata(GumBallData d)
	{
		data = d;
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Nun" && rb.velocity.magnitude > 2f && (bool)col.gameObject.GetComponent<NunBodyPartBehaviour>())
		{
			if (col.gameObject.GetComponent<NunBodyPartBehaviour>().bodyPart == BodyPart.HEAD)
			{
				ZombieBehaviour.instance.gumHead.GetComponent<MeshRenderer>().material.color = data.color;
				ZombieBehaviour.instance.OnHeadShot();
				Object.Destroy(base.gameObject);
			}
			else if (col.gameObject.GetComponent<NunBodyPartBehaviour>().bodyPart == BodyPart.LEG && ZombieBehaviour.instance.animState != ZombieBehaviour.ZombieAnimation.CRAWL && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.DEAD && ZombieBehaviour.instance.state != ZombieBehaviour.NunState.PRAYING)
			{
				ZombieBehaviour.instance.gumFeet.GetComponent<MeshRenderer>().material.color = data.color;
				ZombieBehaviour.instance.OnFeetContactGum();
				Object.Destroy(base.gameObject);
			}
			else if (col.gameObject.GetComponent<NunBodyPartBehaviour>().bodyPart == BodyPart.BODY || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.DEAD || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.PRAYING || ZombieBehaviour.instance.animState == ZombieBehaviour.ZombieAnimation.CRAWL)
			{
				ZombieBehaviour.instance.gumBody.GetComponent<MeshRenderer>().material.color = data.color;
				ZombieBehaviour.instance.OnShotBody();
				Object.Destroy(base.gameObject);
			}
		}
		if (col.gameObject.tag == "Rat" && rb.velocity.magnitude > 2f)
		{
			RatPathManager.instance.mRat.OnBeHitByGum();
			Object.Destroy(base.gameObject);
		}
	}
}
