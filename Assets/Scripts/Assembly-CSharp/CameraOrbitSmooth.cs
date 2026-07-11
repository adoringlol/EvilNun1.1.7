using UnityEngine;

public class CameraOrbitSmooth : MonoBehaviour
{
	public static CameraOrbitSmooth instance;

	public Transform target;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float xMinLimit = 90f;

	public float xMaxLimit = -90f;

	public float distanceMin = 0.5f;

	public float distanceMax = 15f;

	public float smoothTime = 2f;

	private float rotationYAxis;

	private float rotationXAxis;

	private float positionYAxis;

	private float positionXAxis;

	private float velocityX;

	private float velocityY;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		rotationYAxis = eulerAngles.y;
		rotationXAxis = eulerAngles.x;
	}

	private void Update()
	{
		instance.ProcessOrbit(Vector2.zero);
	}

	public void ProcessOrbit(Vector2 delta)
	{
		rotationYAxis += delta.x;
		rotationXAxis -= delta.y;
		rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
		Quaternion quaternion = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
		Quaternion quaternion2 = Quaternion.Euler(rotationXAxis, rotationYAxis, 0f);
		Quaternion quaternion3 = quaternion2;
		Vector3 vector = new Vector3(0f, 0f, 4f);
		Vector3 position = quaternion3 * vector + target.position;
		base.transform.rotation = quaternion3;
		base.transform.position = position;
		velocityX = Mathf.Lerp(velocityX, 0f, Time.deltaTime * 8f);
		velocityY = Mathf.Lerp(velocityY, 0f, Time.deltaTime * 8f);
		base.transform.LookAt(target);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
