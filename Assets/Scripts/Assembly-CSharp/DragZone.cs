using UnityEngine;
using UnityEngine.EventSystems;

public class DragZone : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IEventSystemHandler
{
	public void OnDrag(PointerEventData eventData)
	{
		SimpleSmoothMouseLook.instance.DoOrbit(eventData.delta);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		SimpleSmoothMouseLook.instance.OnStartOrbit(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		SimpleSmoothMouseLook.instance.OnFinishOrbit(eventData.delta);
	}
}
