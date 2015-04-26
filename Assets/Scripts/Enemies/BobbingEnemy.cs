using UnityEngine;
using System.Collections;

public class BobbingEnemy : CheckpointActivatedMovement {

	public int deltaYDistance;	// distance to bob
	public float bobbingSpeed;
	private float originalY;
	private int direction = 1;	// direction of bobbing.

	protected override IEnumerator Active()
	{	
		float lastRun = Time.time;
		while (TagsAndEnums.GetSqrDistance(transform.root.position, ShipMovement.shipMovement.transform.position) < activeRange*activeRange)
		{
			if(transform.root.position.y >= originalY + deltaYDistance || transform.root.position.y <= originalY - deltaYDistance)
			{
				direction = -direction;
				transform.root.position = new Vector3(transform.root.position.x,
				                                      originalY + (deltaYDistance - .0001f) * direction,
				                                      transform.root.position.z);
			}
			transform.root.position = new Vector3(transform.root.position.x,
			                                      transform.root.position.y - direction * (Time.time - lastRun) * bobbingSpeed,
			                                      transform.root.position.z);

			lastRun = Time.time;
			yield return new WaitForEndOfFrame();
		}
	}

	protected override void Start()
	{
		originalY = transform.root.position.y;
		base.Start();
	}
}
