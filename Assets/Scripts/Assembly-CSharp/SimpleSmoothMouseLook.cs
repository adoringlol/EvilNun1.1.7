using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleSmoothMouseLook : MonoBehaviour
{
	public static SimpleSmoothMouseLook instance;

	[Range(0.1f, 10f)]
	public float sensitivity = 5f;

	public Vector2 yclamp = new Vector2(-70f, 70f);

	public Vector2 _smooth;

	public Quaternion lastRotation;

	public Vector2 vpTarget;

	public Vector3 destinationPosition;

	public float normalSensitivity;

	public float currentDistance;

	public Vector2 lastAutoMouse;

	public float speed = 2f;

	public bool initialized;

	public Transform itemPosition;

	public float lastXsmooth;

	public float lastYsmooth;

	public bool invertAxis;

	public Vector3 lastPos;

	public Vector3 lastDirection;

	public Vector3 playerLastDir;

	public Vector3 beforeDelta;

	public Animation gameOverAnim;

	public Vector3 offset;

	public float sensivityValue;

	[Header("Collision config - Doll")]
	public float bumperDistanceCheck = 2.5f;

	public Transform minZoomPosition;

	public Vector3 bumperRayOffset;

	public float zoomSpeed = 10f;

	public float Y_ANGLE_MIN = -85f;

	public float Y_ANGLE_MAX = 85f;

	public Transform player;

	public Transform camTransform;

	public float desiredX;

	public float desiredY;

	public bool canLook = true;

	public bool invertY;

	private bool draging;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		canLook = true;
		if (camTransform == null)
		{
			camTransform = base.transform;
		}
		if (player == null && PlayersManager.instance != null && PlayersManager.instance.GetCurrentController() != null)
		{
			player = PlayersManager.instance.GetCurrentController().transform;
		}
		desiredX = ((player == null) ? base.transform : player).rotation.eulerAngles.y;
		Application.targetFrameRate = 60;
	}

	private void LateUpdate()
	{
		if (camTransform == null)
		{
			camTransform = base.transform;
		}
		if (PlayersManager.instance == null || PlayersManager.instance.GetCurrentController() == null || camTransform == null)
		{
			return;
		}
		if (canLook)
		{
			// PC build: drive the camera with the mouse. The touch version only
			// moved the view while a finger was dragging (the draging flag); here
			// we feed the mouse delta straight into desiredX/desiredY.
			if (PauseController.instance == null || !PauseController.instance.paused)
			{
				// sensivityValue is tuned for touch drag (delta*sens*dt). Mouse delta is
				// already per-frame, so use a smaller multiplier for a natural feel.
				float pcSens = 2.5f;
				float mouseX = Input.GetAxisRaw("Mouse X");
				float mouseY = Input.GetAxisRaw("Mouse Y");
				desiredX += mouseX * pcSens;
				if (!invertY)
				{
					desiredY -= mouseY * pcSens;
				}
				else
				{
					desiredY += mouseY * pcSens;
				}
				if (!PlayersManager.instance.isBaby)
				{
					desiredY = Mathf.Clamp(desiredY, -85f, 85f);
				}
				else
				{
					desiredY = Mathf.Clamp(desiredY, 0f, 30f);
				}
			}
			// Framerate-independent smoothing (Slerp + exponential factor) so the view
			// eases consistently instead of getting choppy when the frame rate varies.
			camTransform.rotation = Quaternion.Slerp(camTransform.rotation, Quaternion.Euler(desiredY, desiredX, 0f), 1f - Mathf.Exp(-25f * Time.deltaTime));
			PlayersManager.instance.GetCurrentController().transform.rotation = Quaternion.Euler(0f, desiredX, 0f);
		}
	}

	private void FixedUpdate()
	{
		if (canLook && PlayersManager.instance.isBaby)
		{
			Vector3 position = PlayersManager.instance.GetCurrentController().eyesTrans.position;
			Vector3 direction = -PlayersManager.instance.GetCurrentController().eyesTrans.transform.forward;
			RaycastHit hitInfo;
			if (Physics.Raycast(PlayersManager.instance.GetCurrentController().eyesTrans.TransformPoint(bumperRayOffset), direction, out hitInfo, bumperDistanceCheck) && hitInfo.transform != PlayersManager.instance.GetCurrentController().eyesTrans)
			{
				float num = Vector3.Distance(hitInfo.point, PlayersManager.instance.GetCurrentController().eyesTrans.position);
				Vector3 b = Vector3.Lerp(PlayersManager.instance.GetCurrentController().eyesTrans.position, minZoomPosition.position, 1f / (num / bumperDistanceCheck));
				position = Vector3.Lerp(base.transform.position, b, 1f - Mathf.Exp((0f - zoomSpeed) * Time.deltaTime));
			}
			else
			{
				position = Vector3.Lerp(base.transform.position, PlayersManager.instance.GetCurrentController().eyesTrans.position, 1f - Mathf.Exp((0f - zoomSpeed) * Time.deltaTime));
			}
			PlayersManager.instance.GetCurrentController().transform.localEulerAngles = new Vector3(PlayersManager.instance.GetCurrentController().transform.localEulerAngles.x, base.transform.localEulerAngles.y, PlayersManager.instance.GetCurrentController().transform.localEulerAngles.z);
			base.transform.position = position;
		}
	}

	public void StartLookingNun()
	{
		canLook = false;
		CameraMoveAnimation.instance.StopAllAnimations();
		base.transform.DOLookAt(ZombieBehaviour.instance.yaw.position, 0.2f);
	}

	public void OnFallInHole()
	{
		canLook = false;
		CameraMoveAnimation.instance.StopAllAnimations();
	}

	public void HideIn(FastHide hide)
	{
		CameraMoveAnimation.instance.canAnimate = true;
		CameraMoveAnimation.instance.CameraIdleAnimation();
		playerLastDir = PlayersManager.instance.GetCurrentController().transform.eulerAngles;
		lastPos = PlayersManager.instance.GetCurrentController().transform.position;
		lastDirection = base.transform.forward;
		canLook = false;
		PlayersManager.instance.GetCurrentController().transform.position = hide.nunPositionDiscover.position;
		base.transform.position = hide.camPosition.position;
		base.transform.forward = hide.camPosition.forward;
	}

	public void HideOut(FastHide hide)
	{
		PlayersManager.instance.GetChild().transform.position = lastPos;
		PlayersManager.instance.GetCurrentController().transform.eulerAngles = playerLastDir;
		base.transform.position = PlayersManager.instance.GetCurrentController().eyesTrans.position;
		base.transform.forward = lastDirection;
		canLook = true;
	}

	public void OnOpenWindow(Vector3 pos)
	{
		playerLastDir = PlayersManager.instance.GetChild().transform.eulerAngles;
		lastPos = PlayersManager.instance.GetChild().transform.position;
		lastDirection = base.transform.forward;
		base.transform.position = pos;
		canLook = true;
	}

	public void OnHideWindow()
	{
		PlayersManager.instance.GetChild().transform.position = lastPos;
		PlayersManager.instance.GetChild().transform.eulerAngles = playerLastDir;
		base.transform.position = PlayersManager.instance.GetChild().eyesTrans.position;
		base.transform.forward = lastDirection;
		canLook = true;
	}

	public void StartCamera()
	{
		CameraMoveAnimation.instance.ResetAnimation();
		base.transform.position = PlayersManager.instance.GetChild().eyesTrans.position;
		CameraMoveAnimation.instance.transform.localPosition = Vector3.zero;
		desiredY = 5f;
		desiredX = 185f;
		camTransform.rotation = Quaternion.Euler(5f, 185f, 0f);
		PlayersManager.instance.GetCurrentController().transform.rotation = Quaternion.Euler(0f, 185f, 0f);
		canLook = true;
	}

	public void StartGameOverAnimation()
	{
		base.transform.parent = gameOverAnim.transform;
		base.transform.localPosition = offset;
		base.transform.localEulerAngles = Vector3.zero;
		base.transform.GetChild(0).localPosition = Vector3.zero;
		base.transform.eulerAngles = new Vector3(15f, 0f, 180f);
		CameraMoveAnimation.instance.StopAllAnimations();
		gameOverAnim.gameObject.SetActive(true);
		gameOverAnim.Play();
		GameUIController.instance.Invoke("ShowGameOver", 6f);
	}

	public void ChangeCameraSensivity(float v)
	{
		sensivityValue = v;
		PlayerPrefs.SetFloat("sensivityFactor", v);
	}

	public void OnUsedDoll()
	{
		playerLastDir = PlayersManager.instance.GetCurrentController().transform.eulerAngles;
		lastPos = PlayersManager.instance.GetCurrentController().transform.position;
		lastDirection = base.transform.forward;
	}

	public void OnFinishUsingDoll()
	{
		PlayersManager.instance.GetChild().transform.position = lastPos;
		PlayersManager.instance.GetCurrentController().transform.eulerAngles = playerLastDir;
		base.transform.position = PlayersManager.instance.GetCurrentController().eyesTrans.position;
		base.transform.forward = lastDirection;
		canLook = true;
	}

	public void DoOrbit(Vector2 delta)
	{
		if (draging && canLook)
		{
			desiredX += delta.x * sensivityValue * Time.deltaTime;
			if (!invertY)
			{
				desiredY -= delta.y * sensivityValue * Time.deltaTime;
			}
			else
			{
				desiredY += delta.y * sensivityValue * Time.deltaTime;
			}
			if (!PlayersManager.instance.isBaby)
			{
				desiredY = Mathf.Clamp(desiredY, -85f, 85f);
			}
			else
			{
				desiredY = Mathf.Clamp(desiredY, 0f, 30f);
			}
		}
	}

	public void OnStartOrbit(PointerEventData data)
	{
		if (data.position.x > (float)(Screen.width / 3))
		{
			draging = true;
			TutorialController.instance.OnCameraControlFinished();
		}
	}

	public void OnFinishOrbit(Vector2 delta)
	{
		draging = false;
	}
}
