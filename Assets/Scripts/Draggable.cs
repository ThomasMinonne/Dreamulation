using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject itemBeginDragged;
	Vector3 startPosition;
	Transform startParent;
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		itemBeginDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		transform.position = Input.mousePosition;
		GetComponent<RawImage>().maskable = false;
	}
	#endregion
	
	#region IEndDragHandler implementation
	public void OnEndDrag(PointerEventData eventData){
		itemBeginDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		GetComponent<RawImage>().maskable = true;
		if(transform.parent == startParent){
			transform.position = startPosition;
		}
	}
	#endregion
}
