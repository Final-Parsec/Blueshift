using UnityEngine;
using System.Collections.Generic;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public string sceneToLoad;
    public bool cameraWaypoint;
    public bool triggerEnemies;
    public List<CheckpointActivatedMovement> enemiesToTrigger;

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

        if (triggerEnemies)
        {
            foreach(CheckpointActivatedMovement enemy in enemiesToTrigger)
                enemy.Trigger();
        }

    }
}
