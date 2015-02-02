using UnityEngine;
using System.Collections.Generic;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public string sceneToLoad;
    public bool cameraWaypoint;
    public bool triggerEnemies;
    public List<CheckpointActivatedMovement> enemiesToTrigger;
    public bool bossFight;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "playerShip")
            return;

        if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            Application.LoadLevel(sceneToLoad);
            loadScene = false;
        }

        if (cameraWaypoint)
        {
            CameraMovement.cameraMovement.NextWaypoint();
            cameraWaypoint = false;
        }

        if (triggerEnemies)
        {
            foreach (CheckpointActivatedMovement enemy in enemiesToTrigger)
                enemy.Trigger();
            triggerEnemies = false;
        }

        if (bossFight)
        {
            CameraMovement.cameraMovement.fightingBoss = true;
            bossFight = false;
        }

    }
}
