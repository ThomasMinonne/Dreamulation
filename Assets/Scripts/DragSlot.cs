using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour, IDropHandler
{
    public GameObject item{
		get {
			if(transform.childCount > 0){
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}
	
	#region IDropHandler  implementation
	public void OnDrop (PointerEventData eventData){
		if(!item){
			Draggable.itemBeginDragged.transform.SetParent(transform);
		}
	}
	#endregion
}
