using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement cameraMovement = null;
    public List<Transform> waypoints;
    public float speed = 10;

    public Vector3 GetMovementVector()
    {
        Vector3 moveVector = Vector3.zero;
        if (waypoints.Count != 0)
            moveVector = Vector3.Normalize(transform.position - waypoints [0].transform.position);
        return moveVector;
    }

    public void NextWaypoint()
    {
        waypoints.RemoveAt(0);
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        bool repeat = true;
        while (repeat && waypoints.Count != 0)
        {
            Quaternion originalRotation = transform.rotation;
            Vector3 targetDirection = waypoints [0].position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime / 2, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);

            if (originalRotation != transform.rotation)
                yield return new WaitForEndOfFrame();
            else
                repeat = false;
        }
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
            Vector3 moveVector = Vector3.Normalize(transform.position - waypoints [0].transform.position);
            transform.position = transform.position - moveVector * step;
        }
    }
}
