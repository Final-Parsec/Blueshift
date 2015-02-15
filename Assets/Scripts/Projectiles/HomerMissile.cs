using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomerMissile : Projectile {
	public float hommingSpeed = 1;
	public float hommingError = 2;
	public float hommingDuration = 3; // in seconds

	public override void Intercept(Vector3 moveVector, float reletiveSpeed)
	{
		StartCoroutine(InterceptCoroutine(moveVector));
	}
	
	IEnumerator InterceptCoroutine(Vector3 moveVector)
	{
		Vector3 targetPositionWithError = ShipMovement.shipMovement.transform.position +
			new Vector3 (Random.Range(-hommingError,hommingError),
			             Random.Range(-hommingError,hommingError),
			             Random.Range(-hommingError,hommingError));

		float startTime = Time.time;
		Vector3 origin = transform.position;
		while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
		{
			if (startTime + hommingDuration > Time.time)
			{
				Vector3 targetDirection = targetPositionWithError - transform.root.position;
				Vector3 newDirection = Vector3.RotateTowards(transform.root.forward, targetDirection, Time.deltaTime * hommingSpeed, 0);
				transform.root.rotation = Quaternion.LookRotation(newDirection);
			}

			transform.Rotate(0,0,Time.deltaTime*360);

			float step = (speed) * Time.deltaTime;
			// update the position
			transform.position = transform.position - (transform.forward * -1 * step);

			yield return new WaitForEndOfFrame();
		}
		armed = false;
		hitObject = false;
		
		transform.position = Vector3.zero;
		if (!projectilePool.ContainsKey(projectileType))
			projectilePool.Add(projectileType, new List<Projectile>());
		projectilePool [projectileType].Add(this);
	}
}
