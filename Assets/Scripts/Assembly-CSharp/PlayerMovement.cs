using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private bool m_IsWalking;

	[SerializeField]
	private float m_WalkSpeed;

	[SerializeField]
	private float m_RunSpeed;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_RunstepLenghten;

	[SerializeField]
	private float m_JumpSpeed;

	[SerializeField]
	private float m_StickToGroundForce;

	[SerializeField]
	private float m_GravityMultiplier;

	[SerializeField]
	private float m_StepInterval;

	[SerializeField]
	private AudioClip[] m_FootstepSounds;

	public Transform m_Camera;

	private float m_YRotation;

	private Vector2 m_Input;

	private Vector3 m_MoveDir = Vector3.zero;

	private CharacterController m_CharacterController;

	private CollisionFlags m_CollisionFlags;

	private bool m_PreviouslyGrounded;

	private Vector3 m_OriginalCameraPosition;

	private float m_StepCycle;

	private float m_NextStep;

	private AudioSource m_AudioSource;

	public Transform eyesPosition;

	private void Start()
	{
		m_CharacterController = GetComponent<CharacterController>();
		if (m_Camera == null && SimpleSmoothMouseLook.instance != null)
		{
			// The original reference points to CameraRoot. Moving Camera.main here
			// would move the animated child while its parent rotates, causing jitter
			// and an incorrect eye offset.
			m_Camera = SimpleSmoothMouseLook.instance.transform;
		}
		else if (m_Camera == null && Camera.main != null)
		{
			m_Camera = Camera.main.transform.root;
		}
		if (eyesPosition == null)
		{
			MyController controller = GetComponent<MyController>();
			if (controller != null)
			{
				eyesPosition = controller.eyesTrans;
			}
		}
		m_StepCycle = 0f;
		m_NextStep = m_StepCycle / 2f;
		m_AudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
		{
			m_MoveDir.y = 0f;
		}
		m_PreviouslyGrounded = m_CharacterController.isGrounded;
	}

	private void LateUpdate()
	{
		if (PlayersManager.instance != null && !PlayersManager.instance.isBaby && m_Camera != null && eyesPosition != null)
		{
			m_Camera.transform.position = eyesPosition.position;
		}
	}

	private void FixedUpdate()
	{
		if (m_CharacterController == null)
		{
			m_CharacterController = GetComponent<CharacterController>();
		}
		if (m_CharacterController == null || PlayersManager.instance == null || PlayersManager.instance.GetCurrentController() == null)
		{
			return;
		}
		float num = 2.8f;
		GetInput();
		if (PlayersManager.instance.isBaby || !PlayersManager.instance.GetCurrentController().standup)
		{
			num = 1.6f;
		}
		else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			// PC build: hold Shift to run (only while standing up).
			num = 4.5f;
		}
		Vector3 vector = base.transform.forward.normalized * m_Input.y + base.transform.right * m_Input.x;
		RaycastHit hitInfo;
		Physics.SphereCast(base.transform.position, m_CharacterController.radius, Vector3.down, out hitInfo, m_CharacterController.height / 2f, -1, QueryTriggerInteraction.Ignore);
		vector = Vector3.ProjectOnPlane(vector, hitInfo.normal);
		m_MoveDir.x = vector.x * num;
		m_MoveDir.z = vector.z * num;
		if (m_CharacterController.isGrounded)
		{
			m_MoveDir.y = 0f - m_StickToGroundForce;
		}
		else
		{
			m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
		}
		m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
		ProgressStepCycle(num);
	}

	private void ProgressStepCycle(float speed)
	{
		if (m_CharacterController.velocity.sqrMagnitude > 0f && (m_Input.x != 0f || m_Input.y != 0f))
		{
			m_StepCycle += (m_CharacterController.velocity.magnitude + speed * ((!m_IsWalking) ? m_RunstepLenghten : 1f)) * Time.fixedDeltaTime;
		}
		if (m_StepCycle > m_NextStep)
		{
			m_NextStep = m_StepCycle + m_StepInterval;
		}
	}

	public void GetInput()
	{
		// The cross-platform layer now combines registered joystick axes with
		// keyboard hardware, so both control schemes drive the same movement.
		float axis = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float axis2 = CrossPlatformInputManager.GetAxisRaw("Vertical");
		m_Input = new Vector2(axis, axis2);
		if (m_Input.sqrMagnitude > 1f)
		{
			m_Input.Normalize();
		}
	}
}
