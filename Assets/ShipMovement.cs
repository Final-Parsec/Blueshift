using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

    void Move(float x, float y, float z)
    {
        // Do not let ship go below environment.
        if (transform.position.y < 3)
        {
            y = Mathf.Abs(y);
        }

        transform.Translate(x, y, z);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime;

        var inputX = Input.acceleration.x;
        var inputY = -Input.acceleration.y;
        var inputZ = -Input.acceleration.z;

        Move(inputX, inputY, inputZ);        
	}
}