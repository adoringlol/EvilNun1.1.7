using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
	[Range(0.1f, 10f)]
	public float sensitivity = 5f;

	[Range(0.01f, 1f)]
	public float smoothing = 0.05f;

	public Vector2 xclamp = new Vector2(-180f, 180f);

	public Vector2 yclamp = new Vector2(-70f, 70f);

	public Vector2 RotationVelocity;

	private Vector2 _mouse;

	private Vector2 _smooth;

	private float _pSmooth;

	private float pVelocity;

	public Transform target;

	public bool dragging;

	public Quaternion lastRotation;

	public Transform slender;

	public float rotationSpeed;

	public float normalSensitivity;

	public bool doingAtraction;

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			_mouse.y += Input.GetAxis("Mouse Y") * sensitivity;
			_mouse.x += Input.GetAxis("Mouse X") * sensitivity;
			pVelocity = 0f;
			_pSmooth = 0f;
		}
		else
		{
			_pSmooth = Mathf.SmoothDamp(_pSmooth, Input.GetAxis("Mouse X") * sensitivity, ref pVelocity, smoothing);
			base.transform.Rotate(0f, _pSmooth, 0f);
		}
		_mouse.y = Mathf.Clamp(_mouse.y, yclamp.x, yclamp.y);
		Vector3 normalized = (slender.position - base.transform.position).normalized;
		_smooth.x = Mathf.SmoothDamp(_smooth.x, _mouse.x, ref RotationVelocity.x, smoothing);
		_smooth.y = Mathf.SmoothDamp(_smooth.y, _mouse.y, ref RotationVelocity.y, smoothing);
		base.transform.localRotation = Quaternion.identity;
		base.transform.Rotate(_smooth.y, _smooth.x, 0f);
	}

	private void LateUpdate()
	{
	}
}
