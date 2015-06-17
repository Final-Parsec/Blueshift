using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour {

	public float onTouchMoveSpeed = .5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //joystick center will start here. set center and bounds
	public void OnTouched(RaycastHit hitSummary) {
		Debug.Log ("ooooh.");
	}

    //chunk disappear behavior here, dump joystick center to be set at next touch
	public void OnDraggingStopped() {
		Debug.Log ("hello!, That Felt Good!");
	}

	public void OnDrag(DragSummary dragSummary) {
		this.transform.position += onTouchMoveSpeed * new Vector3(dragSummary.direction.x, dragSummary.direction.y, 0f);
	}
}
