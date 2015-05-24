using UnityEngine;
using System.Collections;

public class MoonOrbit : MonoBehaviour {

    private Transform earth;

	// Use this for initialization
	void Start () {
        earth = GameObject.Find("Earth").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(earth.position, Vector3.up, -.5f * Time.deltaTime);
	}
}
