using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement cameraMovement = null;
    public List<Transform> waypoints;
    public float speed = 10;


    public void NextWaypoint()
    {
        waypoints.RemoveAt(0);
    }

    // Use this for initialization
    void Start()
    {
        cameraMovement = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        // make the camera move
        if (waypoints.Count != 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waypoints [0].position, step);
        }
    }
}
