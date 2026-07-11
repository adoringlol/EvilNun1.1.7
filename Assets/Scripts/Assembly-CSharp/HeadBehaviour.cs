using UnityEngine;

public class HeadBehaviour : MonoBehaviour
{
	public float smoothRotation = 10f;

	public ZombieBehaviour nun;

	public float rotationAngle;

	public Quaternion currentRotation;

	public int minAngle;

	public int maxAngle;

	private void Start()
	{
		nun = base.transform.root.GetComponent<ZombieBehaviour>();
	}

	public Vector3 GetPlayerDirection()
	{
		return PlayersManager.instance.GetCurrentController().eyesTrans.position - (base.transform.position + base.transform.right * 0.5f);
	}

	private void LateUpdate()
	{
		if (ZombieBehaviour.instance.state == ZombieBehaviour.NunState.NONE || ZombieBehaviour.instance.state == ZombieBehaviour.NunState.FOLLOWING_PLAYER)
		{
			Vector3 playerDirection = GetPlayerDirection();
			float num = Vector3.Angle(playerDirection, base.transform.root.forward);
			if (Vector3.Cross(playerDirection, base.transform.root.forward).y < 0f)
			{
				num = 0f - num;
				Debug.Log("Está detrás");
			}
			else
			{
				Debug.Log("Está delante");
			}
			num = ClampAngle(num, minAngle, maxAngle);
			rotationAngle = num;
			Quaternion b = base.transform.localRotation * Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));
			currentRotation = Quaternion.Lerp(currentRotation, b, smoothRotation * Time.deltaTime);
			base.transform.localRotation = currentRotation;
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
