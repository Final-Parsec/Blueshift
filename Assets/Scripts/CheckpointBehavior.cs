using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    public List<EnemyHealth> bosses;
    public bool bossFight;
    public BossHealthBar bossHealthBar;
    public bool cameraWaypoint;
    public List<EnemyHealth> destroyOnVictory;
    public string dialogue;
    public List<CheckpointActivatedMovement> enemiesToTrigger;
    public bool loadScene;
    public int sceneToLoad;
    public string speaker;
    public bool triggerEnemies;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != "playerShip")
        {
            return;
        }

        if (!string.IsNullOrEmpty(this.dialogue))
        {
            var inGameDialogue = InGameDialogue.Instance;
            inGameDialogue.StartCoroutine(inGameDialogue.SayDialogue(this.speaker, this.dialogue));
        }

        if (this.loadScene)
        {
            SceneLoader.LoadCutscene(this.sceneToLoad);
            this.loadScene = false;
        }

        if (this.cameraWaypoint)
        {
            CameraMovement.cameraMovement.NextWaypoint();
            this.cameraWaypoint = false;
        }

        if (this.triggerEnemies)
        {
            foreach (var enemy in this.enemiesToTrigger)
            {
                enemy.Trigger();
            }
            this.triggerEnemies = false;
        }

        if (this.bossFight)
        {
            CameraMovement.cameraMovement.fightingBoss = true;
            this.bossFight = false;
            this.StartCoroutine(this.TestForVictory());
        }
    }

    private IEnumerator TestForVictory()
    {
        this.bossHealthBar.SwoopIn();

        var sumMaxHealth = 0;
        foreach (var boss in this.bosses)
            sumMaxHealth += boss.MaxHealth;

        while (this.bosses.Count > 0)
        {
            this.bosses.RemoveAll(enemyHealth => enemyHealth == null);

            var health = 0;
            foreach (var boss in this.bosses)
                health += boss.Health;

            this.bossHealthBar.UpdateHealthBar(health, sumMaxHealth);

            yield return new WaitForEndOfFrame();
        }

        CameraMovement.cameraMovement.fightingBoss = false;

        yield return new WaitForSeconds(.4f);

        foreach (var enemyHealth in this.destroyOnVictory)
            enemyHealth.Death();

        this.bossHealthBar.SwoopOut();
    }
}