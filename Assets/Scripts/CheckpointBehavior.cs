using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene;
    public int sceneToLoad;
    public bool cameraWaypoint;
    public bool triggerEnemies;
    public List<CheckpointActivatedMovement> enemiesToTrigger;
    public bool bossFight;
	public List<EnemyHealth> bosses;
	public List<EnemyHealth> destroyOnVictory;
    public BossHealthBar bossHealthBar;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "playerShip")
            return;

        if (loadScene)
        {
			SceneLoader.LoadCutscene(sceneToLoad);
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
        bossHealthBar.SwoopIn();

        int sumMaxHealth = 0;
        foreach(EnemyHealth boss in bosses)
            sumMaxHealth += boss.MaxHealth;

		while (bosses.Count > 0) 
		{
            bosses.RemoveAll(enemyHealth => enemyHealth == null);

            int health = 0;
            foreach(EnemyHealth boss in bosses)
                health += boss.Health;
            
            bossHealthBar.UpdateHealthBar(health, sumMaxHealth);
			
			yield return new WaitForEndOfFrame();
		}

		CameraMovement.cameraMovement.fightingBoss = false;

		yield return new WaitForSeconds(.4f);

		foreach (EnemyHealth enemyHealth in destroyOnVictory)
			enemyHealth.Health = 0;

        bossHealthBar.SwoopOut();
	}
}
