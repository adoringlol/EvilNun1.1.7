using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
	public enum AxisOption
	{
		Both = 0,
		OnlyHorizontal = 1,
		OnlyVertical = 2
	}

	public int MovementRange = 100;

	public AxisOption axesToUse;

	public string horizontalAxisName = "Horizontal";

	public string verticalAxisName = "Vertical";

	private float extraSpeed;

	private Vector3 m_StartPos;

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	private void Start()
	{
		CreateVirtualAxes();
		m_StartPos = base.transform.position;
	}

	public void Reset()
	{
		CreateVirtualAxes();
		m_HorizontalVirtualAxis.Update(0f);
		m_VerticalVirtualAxis.Update(0f);
		Input.ResetInputAxes();
		base.transform.position = m_StartPos;
	}

	private void UpdateVirtualAxes(Vector3 value)
	{
		Vector3 vector = m_StartPos - value;
		vector.y = 0f - vector.y;
		vector /= (float)MovementRange;
		if (PlayersManager.instance.isBaby)
		{
			vector *= 0.6f;
		}
		else
		{
			vector *= ((!PlayersManager.instance.child.standup) ? 0.7f : 1f);
		}
		m_HorizontalVirtualAxis.Update(0f - vector.x);
		m_VerticalVirtualAxis.Update(vector.y);
	}

	private void CreateVirtualAxes()
	{
		if (!CrossPlatformInputManager.AxisExists(horizontalAxisName))
		{
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
		}
		else if (m_HorizontalVirtualAxis == null)
		{
			CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
			m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
		}
		if (!CrossPlatformInputManager.AxisExists(verticalAxisName))
		{
			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}
		else if (m_VerticalVirtualAxis == null)
		{
			CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
			m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
			CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		Vector3 zero = Vector3.zero;
		int value = (int)(data.position.x - m_StartPos.x);
		value = Mathf.Clamp(value, -MovementRange, MovementRange);
		zero.x = value;
		value = (int)(data.position.y - m_StartPos.y);
		value = Mathf.Clamp(value, -MovementRange, MovementRange);
		zero.y = value;
		base.transform.position = new Vector3(m_StartPos.x + zero.x, m_StartPos.y + zero.y, m_StartPos.z + zero.z);
		UpdateVirtualAxes(base.transform.position);
		if (TutorialController.instance.isEnabled)
		{
			TutorialController.instance.OnMoveControlFinished();
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		base.transform.position = m_StartPos;
		Touch_Control_Manager.isMoving = false;
		UpdateVirtualAxes(m_StartPos);
	}

	public void UnRegisterAxis()
	{
		m_HorizontalVirtualAxis.Remove();
		m_VerticalVirtualAxis.Remove();
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (Touch_Control_Manager.isMoving)
		{
			return;
		}
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began)
			{
				Touch_Control_Manager.moveTouchID = touch.fingerId;
			}
		}
		Touch_Control_Manager.isMoving = true;
	}
}
