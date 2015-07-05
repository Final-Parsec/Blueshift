using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

public class BobbingEnemy : CheckpointActivatedMovement {

	public int deltaYDistance;	// distance to bob
	public float bobbingSpeed;
	private float originalY;
	private int direction = 1;	// direction of bobbing.

	protected override IEnumerator Active()
	{	
		float lastRun = Time.time;
        while (TagsAndEnums.GetSqrDistance(mainComponent.position, ShipMovement.shipMovement.transform.position) < activeRange*activeRange)
		{
            if(mainComponent.position.y >= originalY + deltaYDistance || mainComponent.position.y <= originalY - deltaYDistance)
			{
				direction = -direction;
                mainComponent.position = new Vector3(mainComponent.position.x,
				                                      originalY + (deltaYDistance - .0001f) * direction,
                                                       mainComponent.position.z);
			}
            mainComponent.position = new Vector3(mainComponent.position.x,
                                                   mainComponent.position.y - direction * (Time.time - lastRun) * bobbingSpeed,
                                                   mainComponent.position.z);

			lastRun = Time.time;
            yield return new WaitForSeconds(1f / 120f);
		}
	}

	protected override void Start()
	{
        base.Start();
	    Debug.Assert(this.mainComponent != null, "mainComponent is set in base.Start()");
	    this.originalY = this.mainComponent.position.y;
        this.bobbingSpeed -= Random.Range(0f,3f);
	}
}
