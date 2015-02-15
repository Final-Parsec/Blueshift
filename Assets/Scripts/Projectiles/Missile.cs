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
		Vector3 origin = transform.position;
		while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
		{
			float step = (speed+reletiveSpeed) * Time.deltaTime;
			// update the position
			transform.position = new Vector3(transform.position.x - moveVector.x * step,
			                                 transform.position.y - moveVector.y * step,
			                                 transform.position.z - moveVector.z * step);
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
