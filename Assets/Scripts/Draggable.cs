using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform rootParent;
	
	public static GameObject itemBeginDragged;
	Vector3 startPosition;
	Transform startParent;
	
	 void Start()
    {
        rootParent = transform.parent.transform.parent.transform.parent.transform.parent;
    }
	
	#region IBeginDragHandler implementation
	public void OnBeginDrag(PointerEventData eventData){
		itemBeginDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		transform.SetParent(rootParent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	#endregion
	
	#region IDragHandler implementation
	public void OnDrag(PointerEventData eventData){
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (point.x, point.y, transform.position.z), 1);
	}
	#endregion
	
	#region IEndDragHandler implementation
	public void OnEndDrag(PointerEventData eventData){
		//itemBeginDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == rootParent || transform.parent == startParent){
			transform.position = startPosition;
			transform.SetParent(startParent);
		}
	}
	#endregion
}
