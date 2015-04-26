using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomerMissile : Projectile {
	public float homingSpeed = 1;
	public float homingError = 2;
	public float homingDuration = 3; // in seconds

	public override void Intercept(Vector3 moveVector, float reletiveSpeed)
	{
		StartCoroutine(InterceptCoroutine(moveVector));
	}
	
	IEnumerator InterceptCoroutine(Vector3 moveVector)
	{
		Vector3 targetPositionWithError = ShipMovement.shipMovement.transform.position +
			new Vector3 (Random.Range(-homingError,homingError),
			             Random.Range(-homingError,homingError),
			             Random.Range(-homingError,homingError));

		float lastRun = Time.time;
		float startTime = Time.time;
		Vector3 origin = transform.position;
		while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
		{
			if (startTime + homingDuration > Time.time)
			{
				Vector3 targetDirection = targetPositionWithError - transform.root.position;
				Vector3 newDirection = Vector3.RotateTowards(transform.root.forward, targetDirection, (Time.time - lastRun) * homingSpeed, 0);
				transform.root.rotation = Quaternion.LookRotation(newDirection);
			}

			transform.Rotate(0,0,(Time.time - lastRun)*360);

			float step = (speed) * (Time.time - lastRun);
			// update the position
			transform.position = transform.position - (transform.forward * -1 * step);

			lastRun = Time.time;
			yield return new WaitForEndOfFrame();
		}
		armed = false;
		hitObject = false;
		
		transform.position = Vector3.zero;
        BackInThePool();
	}
}
