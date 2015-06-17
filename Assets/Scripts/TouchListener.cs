using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchListener : MonoBehaviour {
		
	public Camera usedCamera;
	public float minimumDragDistance = 1f;
	private Vector3 touchPoint;
	
	protected bool isTouching = false;
	
	private static GameObject selectedObject;
	
	public void Start() {}
	
	public void FixedUpdate() {

		if(!isTouching) {
			if (Input.GetMouseButtonDown(0)) {
				isTouching = true;
				selectedObject = null;

				RaycastHit hitSummary;
				Physics.Raycast(usedCamera.ScreenPointToRay(Input.mousePosition), out hitSummary, 200);

				this.touchPoint = usedCamera.ScreenToWorldPoint(Input.mousePosition);
				
				if (hitSummary.collider != null) {
					selectedObject = hitSummary.collider.gameObject;
					Debug.Log (selectedObject.name);
					selectedObject.SendMessage("OnTouched", hitSummary, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0)) {
			Debug.Log ("UP");
			this.isTouching = false;
			
			if(selectedObject != null) {
				selectedObject.SendMessage("OnDraggingStopped", null, SendMessageOptions.DontRequireReceiver);
			}
			
			selectedObject = null;
		}
		
		if(isTouching && selectedObject != null) {
			OnTouchHeld();
		}
	}
	
	private void OnTouchHeld() {
		Vector3 currentMousePosition = usedCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 dragAmount = (currentMousePosition - touchPoint);
		Vector3 directionToCurrentMousePosition = currentMousePosition - selectedObject.transform.position;

		DragSummary dragSummary = new DragSummary();
		dragSummary.position = currentMousePosition;
		dragSummary.amount = dragAmount;
		dragSummary.direction = directionToCurrentMousePosition;

		if(dragAmount.magnitude > minimumDragDistance) {
			selectedObject.SendMessage("OnDrag", dragSummary, SendMessageOptions.DontRequireReceiver);
		}

		if(Mathf.Abs(dragAmount.x) > minimumDragDistance) {
			selectedObject.SendMessage("OnHorizontalDrag", dragSummary, SendMessageOptions.DontRequireReceiver);
		}
		
		if(Mathf.Abs(dragAmount.y) > minimumDragDistance) {
			selectedObject.SendMessage("OnVerticalDrag", dragSummary, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public static GameObject GetSelectedObject() {
		return selectedObject;
	}
}
