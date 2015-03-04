using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject _earth = GameObject.Find ("Earth");

		Transform target = _earth.GetComponent<Transform>();
		//gameObject.GetComponent<> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
