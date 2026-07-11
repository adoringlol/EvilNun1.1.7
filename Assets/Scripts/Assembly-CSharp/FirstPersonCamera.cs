using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
	private const float Y_ANGLE_MIN = -90f;

	private const float Y_ANGLE_MAX = 90f;

	public Transform player;

	public Transform camTransform;

	public float desiredX;

	public float desiredY;

	[SerializeField]
	private float sensitivityX = 6f;

	[SerializeField]
	private float sensitivityY = 6f;

	public bool canLook = true;

	public bool invertY;

	private void Start()
	{
		desiredX = player.transform.rotation.eulerAngles.y;
	}

	private void Update()
	{
		if (canLook && Input.touchCount > 0)
		{
			CheckLook();
		}
	}

	private void MouseLook()
	{
		desiredX += Input.GetAxis("Mouse X") * sensitivityX * 20f * Time.deltaTime;
		desiredY -= Input.GetAxis("Mouse Y") * sensitivityY * 20f * Time.deltaTime;
		desiredY = Mathf.Clamp(desiredY, -90f, 90f);
	}

	private void CheckLook()
	{
		Touch[] touches = Input.touches;
		foreach (Touch touch in touches)
		{
			if (!Touch_Control_Manager.isMoving)
			{
				TouchLook();
			}
			else if (touch.fingerId != Touch_Control_Manager.moveTouchID)
			{
				TouchLook();
			}
		}
	}

	private void TouchLook()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			switch (touch.phase)
			{
			case TouchPhase.Began:
				if (!Touch_Control_Manager.islooking)
				{
					Touch_Control_Manager.lookTouchID = touch.fingerId;
					Touch_Control_Manager.islooking = true;
				}
				break;
			case TouchPhase.Moved:
				if (touch.fingerId == Touch_Control_Manager.lookTouchID)
				{
					desiredX += touch.deltaPosition.x * sensitivityX * Time.deltaTime;
					if (!invertY)
					{
						desiredY -= touch.deltaPosition.y * sensitivityY * Time.deltaTime;
					}
					else
					{
						desiredY += touch.position.y * sensitivityY * Time.deltaTime;
					}
					desiredY = Mathf.Clamp(desiredY, -90f, 90f);
				}
				break;
			case TouchPhase.Ended:
				if (touch.fingerId == Touch_Control_Manager.lookTouchID)
				{
					Touch_Control_Manager.islooking = false;
				}
				break;
			case TouchPhase.Canceled:
				if (touch.fingerId == Touch_Control_Manager.lookTouchID)
				{
					Touch_Control_Manager.islooking = false;
				}
				break;
			}
		}
	}

	private void LateUpdate()
	{
		camTransform.rotation = Quaternion.Lerp(camTransform.rotation, Quaternion.Euler(desiredY, camTransform.eulerAngles.y, 0f), Time.deltaTime * 20f);
		player.rotation = Quaternion.Lerp(player.rotation, Quaternion.Euler(0f, desiredX, 0f), Time.deltaTime * 20f);
	}
}
