using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

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
        base.Start();
        Debug.Assert(this.mainComponent != null, "mainComponent is set in base.Start()");
        this.moveVector = this.mainComponent.forward.normalized;
    }
}
