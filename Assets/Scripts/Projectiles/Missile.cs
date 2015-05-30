using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missile : Projectile {

	public override void Intercept(Vector3 moveVector, float reletiveSpeed)
	{
		StartCoroutine(InterceptCoroutine(moveVector, reletiveSpeed));
	}
	
	IEnumerator InterceptCoroutine(Vector3 moveVector, float reletiveSpeed)
	{
		float lastRun = Time.time;
		Vector3 origin = transform.position;
		while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
		{
			float step = (speed+reletiveSpeed) * (Time.time - lastRun);
			// update the position
			transform.position = new Vector3(transform.position.x - moveVector.x * step,
			                                 transform.position.y - moveVector.y * step,
			                                 transform.position.z - moveVector.z * step);

			lastRun = Time.time;
			yield return new WaitForEndOfFrame();
		}
		armed = false;
		hitObject = false;
		
        BackInThePool();
	}
}
