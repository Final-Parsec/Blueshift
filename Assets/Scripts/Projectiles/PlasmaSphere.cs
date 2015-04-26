using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaSphere : Projectile {

	public override void Intercept(Vector3 moveVector, float reletiveSpeed)
	{
		StartCoroutine(InterceptCoroutine(moveVector));
	}
	
	IEnumerator InterceptCoroutine(Vector3 moveVector)
	{
		Vector3 origin = transform.position;
		float lastRun = Time.time;
		while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
		{
			transform.Rotate(Time.deltaTime*Random.Range(300,600),
			                 Time.deltaTime*Random.Range(300,600),
			                 Time.deltaTime*Random.Range(300,600));

			float step = (speed) * (Time.time - lastRun);
			// update the position
			transform.position = new Vector3(transform.position.x - moveVector.x * step,
			                                 transform.position.y - moveVector.y * step,
			                                 transform.position.z - moveVector.z * step);

			lastRun = Time.time;
			yield return new WaitForEndOfFrame();
		}
		armed = false;
		hitObject = false;
		
		transform.position = Vector3.zero;
        BackInThePool();
	}
}
