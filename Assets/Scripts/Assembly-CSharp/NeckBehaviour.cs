using UnityEngine;

public class NeckBehaviour : MonoBehaviour
{
	public Vector3 directionToLook;

	public float smoothRotation = 10f;

	public float fastRotation = 20f;

	public float initialY;

	public ZombieBehaviour nun;

	public float rotationAngle;

	public float rotationAngleZ;

	public Quaternion currentRotation;

	public bool lookPlayer;

	private void Start()
	{
		nun = base.transform.root.GetComponent<ZombieBehaviour>();
	}

	public Vector3 GetNoiseDirection()
	{
		return nun.lastNoisePosition - nun.transform.position;
	}

	public Vector3 GetPlayerDirection()
	{
		return PlayersManager.instance.GetCurrentController().transform.position - nun.transform.position;
	}

	private void LateUpdate()
	{
		if (ZombieBehaviour.instance.animState != ZombieBehaviour.ZombieAnimation.CRAWL && ZombieBehaviour.instance.animState != ZombieBehaviour.ZombieAnimation.ATTACK)
		{
			bool flag = false;
			flag = nun.state == ZombieBehaviour.NunState.HEAR_NOISE || nun.state == ZombieBehaviour.NunState.FOLLOWING_PLAYER || nun.state == ZombieBehaviour.NunState.SEEN_PLAYER;
			Vector3 vector = ((nun.state != ZombieBehaviour.NunState.HEAR_NOISE) ? GetPlayerDirection() : GetNoiseDirection());
			float num = Vector3.Angle(vector, base.transform.root.forward);
			if (Vector3.Cross(vector, base.transform.root.forward).y < 0f)
			{
				num = 0f - num;
			}
			if (!flag)
			{
				num = 0f;
			}
			if (num >= -90f && num <= 90f)
			{
				num = ClampAngle(num, -90f, 90f);
				rotationAngle = num;
				Quaternion b = base.transform.localRotation * Quaternion.Euler(new Vector3(rotationAngle, 0f, 0f));
				currentRotation = Quaternion.Lerp(currentRotation, b, smoothRotation * Time.deltaTime);
				base.transform.localRotation = currentRotation;
			}
		}
	}

	private float ClampAngle(float angle, float from, float to)
	{
		if (angle < 0f)
		{
			angle = 360f + angle;
		}
		if (angle > 180f)
		{
			return Mathf.Max(angle, 360f + from);
		}
		return Mathf.Min(angle, to);
	}
}
