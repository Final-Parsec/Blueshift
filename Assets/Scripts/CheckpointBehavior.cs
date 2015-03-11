using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public string sceneToLoad;
    public bool cameraWaypoint;
    public bool triggerEnemies;
    public List<CheckpointActivatedMovement> enemiesToTrigger;
    public bool bossFight;
	public List<EnemyHealth> bosses;
	public List<EnemyHealth> destroyOnVictory;

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
			StartCoroutine(TestForVictory());
        }
    }

	IEnumerator TestForVictory()
	{
		while (bosses.Count > 0) 
		{
			bosses.RemoveAll(enemyHealth => enemyHealth == null);
			yield return new WaitForSeconds(.2f);
		}

		CameraMovement.cameraMovement.fightingBoss = false;

		yield return new WaitForSeconds(.4f);

		foreach (EnemyHealth enemyHealth in destroyOnVictory)
			enemyHealth.Health = 0;


	}
}
