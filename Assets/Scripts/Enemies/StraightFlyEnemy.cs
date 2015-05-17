using UnityEngine;
using System.Collections;

public class StraightFlyEnemy : CheckpointActivatedMovement
{
    public float speed;
    private Vector3 moveVector;
    
    protected override IEnumerator Active()
    {   
		float lastRun = Time.time;
        while (TagsAndEnums.GetSqrDistance(mainComponent.position, ShipMovement.shipMovement.transform.position) < activeRange*activeRange)
        {
            mainComponent.position = mainComponent.position + moveVector * speed * (Time.time - lastRun);

			lastRun = Time.time;
            yield return new WaitForSeconds(1f/120f);
        }
    }
    
    protected override void Start()
    {
        moveVector = mainComponent.forward.normalized;
        base.Start();
    }
}
