using System.Collections;
using DarkTonic.MasterAudio;
using I2.Loc;
using UnityEngine;
using UnityEngine.Analytics;

public class TakeableObject : TouchableElement
{
	public Rigidbody rb;

	public string collisionSound;

	public float forceOnRelease = 20f;

	public Collider col;

	public float noiseDistance = 1f;

	public bool usefullItem;

	public float weight = 5f;

	private Vector3 scale;

	[Header("Referencia al efecto de brillo[Opcional]")]
	public GameObject shineEffect;

	[Header("Fuerza a aplicar para que suene al impactar")]
	public float minForceForPlayingSound = 1f;

	public Vector3 forwardAxis;

	public float takenHeightOffset;

	public bool firstTaken;

	[SoundGroup]
	public string fallSound;

	public override void Initialize()
	{
		scale = base.transform.localScale;
		col = GetComponent<Collider>();
		if ((bool)GetComponent<Rigidbody>())
		{
			rb = GetComponent<Rigidbody>();
		}
		base.Initialize();
	}

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		rb.velocity = Vector3.zero;
		col.enabled = false;
		rb.isKinematic = true;
		base.transform.parent = null;
		if (!firstTaken)
		{
			AnalyticsController.instance.OnTakeItem(LocalizationManager.GetTranslation(labelName, true, 0, true, false, null, "Spanish"));
			if (id == 1)
			{
				AnalyticsEvent.Custom("cable_taken");
			}
			firstTaken = true;
		}
		if (shineEffect != null)
		{
			shineEffect.SetActive(false);
		}
		PlayersManager.instance.GetCurrentController().TakeItem(this);
	}

	public void Release()
	{
		base.transform.localScale = scale;
		col.enabled = true;
		base.transform.parent = null;
		base.transform.position = SimpleSmoothMouseLook.instance.transform.position + SimpleSmoothMouseLook.instance.transform.forward * 0.25f;
		rb.isKinematic = false;
		float num = Mathf.Clamp(PlayersManager.instance.GetCurrentController().GetComponent<CharacterController>().velocity.magnitude * 2.5f, 0.5f, 10f);
		float mass = GetComponent<Rigidbody>().mass;
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward.normalized, out hitInfo, 0.5f, PlayersManager.instance.GetCurrentController().techo))
		{
			rb.AddForce(Camera.main.transform.forward.normalized * 0f * mass, ForceMode.Impulse);
		}
		else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward.normalized, out hitInfo, 3f, PlayersManager.instance.GetCurrentController().elevatorLayer))
		{
			rb.AddForce(Camera.main.transform.forward.normalized * 5f * mass, ForceMode.Impulse);
			rb.AddTorque(Camera.main.transform.forward.normalized * 2f * mass, ForceMode.Impulse);
		}
		else
		{
			rb.AddForce(Camera.main.transform.forward.normalized * num * mass, ForceMode.Impulse);
			rb.AddTorque(Camera.main.transform.forward.normalized * num * mass, ForceMode.Impulse);
		}
		StartCoroutine("Releasing");
	}

	public void ReleaseOnDied()
	{
		base.transform.localScale = scale;
		col.enabled = true;
		base.transform.parent = null;
		base.transform.position = SimpleSmoothMouseLook.instance.itemPosition.position + SimpleSmoothMouseLook.instance.transform.forward * 0.25f;
		rb.isKinematic = false;
		StartCoroutine("Releasing");
	}

	public void ResetScale()
	{
		base.transform.localScale = scale;
		col.enabled = true;
		base.transform.parent = null;
	}

	public IEnumerator Releasing()
	{
		while (rb.velocity.magnitude > 0.25f)
		{
			yield return null;
		}
		if (shineEffect != null)
		{
			shineEffect.SetActive(true);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag != "Nun" && ZombieBehaviour.instance != null)
		{
			ZombieBehaviour.instance.OnDidNoiseWithObject(col.contacts[0].point, noiseDistance);
		}
		if (col.impulse.magnitude > minForceForPlayingSound && fallSound != string.Empty)
		{
			MasterAudio.PlaySound3DAtVector3(fallSound, base.transform.position + Vector3.up * 0.2f);
		}
	}

	public virtual void OnExitHide()
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			GameUIController.instance.releaseButton.SetActive(true);
		}
	}

	public virtual void OnEnterHide()
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			GameUIController.instance.releaseButton.SetActive(false);
		}
	}

	public virtual void OnReleaseItem()
	{
		if (PlayersManager.instance.GetCurrentController().hasObjectOnHand && PlayersManager.instance.GetCurrentController().objectOnHand != null)
		{
			Release();
			PlayersManager.instance.GetCurrentController().objectOnHand = null;
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
			GameUIController.instance.releaseButton.SetActive(false);
		}
	}

	public virtual void OnDestroyItem()
	{
		if (PlayersManager.instance.GetCurrentController().objectOnHand == this)
		{
			PlayersManager.instance.GetCurrentController().objectOnHand = null;
			PlayersManager.instance.GetCurrentController().hasObjectOnHand = false;
			GameUIController.instance.releaseButton.SetActive(false);
		}
		Object.Destroy(base.gameObject);
	}
}
