using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour {

	public float onTouchMoveSpeed = 10f;
	public bool clicked;
	public Vector3 mouseCache; //cheese cache
	private Vector3 center;
	private ShipMovement shipMover;

	// Use this for initialization
	void Start () {
		clicked = false;
		shipMover = GameObject.Find ("mover").GetComponent<ShipMovement>();
		Debug.Log (shipMover.bankSpeed);

	}
	
	// Update is called once per frame
	void Update () {
		if (clicked) {
			Vector3 curMouse = Input.mousePosition;
			Vector3 tempTrans = gameObject.transform.position;

			if (curMouse.x > mouseCache.x)
					tempTrans.x += onTouchMoveSpeed;
			else if (curMouse.x < mouseCache.x)
					tempTrans.x -= onTouchMoveSpeed;
			if (curMouse.y > mouseCache.y)
					tempTrans.y += onTouchMoveSpeed;
			else if (curMouse.y < mouseCache.y)
					tempTrans.y -= onTouchMoveSpeed;

			gameObject.transform.position = tempTrans;
			mouseCache = curMouse;

			//now check which direction to move

			if (curMouse.x > center.x)
				//move right
				shipMover.BankRight();
			else if (curMouse.x < center.x)
				shipMover.BankLeft ();
			else if (curMouse.y > center.y)
				shipMover.Ascend ();
			else if (curMouse.y < center.y)
				shipMover.Dive ();
		}

	}

	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	void OnMouseDown(){
		//Debug.Log ("Mmmmmm");
		clicked = true;
		mouseCache = Input.mousePosition;
		center = mouseCache;
		}

	/// <summary>
	/// Raises the mouse up event.
	/// </summary>
	void OnMouseUp(){
		clicked = false;
		Vector3 temp;
		//temp = gameObject.transform.position;
		//temp.x = center.x;
		//temp.y = center.y;
		//gameObject.transform.position = temp;
		//Debug.Log ("ooooh.");
	}
//    //chunk disappear behavior here, dump joystick center to be set at next touch
//	public void OnDraggingStopped() {
//		Debug.Log ("hello!, That Felt Good!");
//	}
//
//	public void OnDrag(DragSummary dragSummary) {
//		this.transform.position += onTouchMoveSpeed * new Vector3(dragSummary.direction.x, dragSummary.direction.y, 0f);
//		Debug.Log ("this shouldnt happen");
//	}
//}
