using UnityEngine;
using System.Collections;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public bool cameraWaypoint;
    public string sceneToLoad;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag != "playerShip")
            return;

        if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            Application.LoadLevel(sceneToLoad);
        }

        if (cameraWaypoint)
        {
            cameraWaypoint = false;
            CameraMovement.cameraMovement.NextWaypoint();
        }

    }

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
