using UnityEngine;
using System.Collections.Generic;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public string sceneToLoad;
    public bool cameraWaypoint;
    public bool triggerEnemies;
    public List<FlyingEnemy> enemiesToTrigger;

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
            foreach(FlyingEnemy enemy in enemiesToTrigger)
                enemy.Trigger();
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
