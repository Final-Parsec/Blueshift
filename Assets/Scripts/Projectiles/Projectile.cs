using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public TagsAndEnums.ProjectileType projectileType;
    public float selfDestructRange = 500;
    protected bool hitObject = false;
	public bool armed = false;
    public string shooter;

    public static Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed,
                                               Vector3 targetOrigin, Vector3 targetVel)
    {
        
        Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);
        
        // Decompose the target's velocity into the part parallel to the
        // direction to the cannon and the part tangential to it.
        // The part towards the cannon is found by projecting the target's
        // velocity on dirToTarget using a dot product.
        Vector3 targetVelOrth = Vector3.Dot(targetVel, dirToTarget) * dirToTarget;
        
        // The tangential part is then found by subtracting the
        // result from the target velocity.
        Vector3 targetVelTang = targetVel - targetVelOrth;
        
        // The tangential component of the velocities should be the same
        // (or there is no chance to hit)
        // THIS IS THE MAIN INSIGHT!
        Vector3 shotVelTang = targetVelTang;
        
        // Now all we have to find is the orthogonal velocity of the shot
        
        float shotVelSpeed = shotVelTang.magnitude;
        if (shotVelSpeed > shotSpeed)
        {
            // Shot is too slow to intercept target, it will never catch up.
            // Do our best by aiming in the direction of the targets velocity.
            return (targetVel.normalized * shotSpeed).normalized * -1;
        } else
        {
            // We know the shot speed, and the tangential velocity.
            // Using pythagoras we can find the orthogonal velocity.
            float shotSpeedOrth =
                Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;
            
            // Finally, add the tangential and orthogonal velocities.
            return (shotVelOrth + shotVelTang).normalized * -1;
        }
    }

	public abstract void Intercept (Vector3 moveVector, float reletiveSpeed);

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
        BackInThePool();
    }

    protected void BackInThePool()
    {
        transform.position = new Vector3(transform.position.x, -99, transform.position.z);
        if (!PrefabAccessor.projectilePool.ContainsKey(projectileType))
            PrefabAccessor.projectilePool.Add(projectileType, new List<Projectile>());
        PrefabAccessor.projectilePool [projectileType].Add(this);
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.root.gameObject.tag != TagsAndEnums.projectile &&
		    other.tag != TagsAndEnums.ignore &&
		    other.tag != TagsAndEnums.shootingBox  &&
		    shooter != other.transform.root.gameObject.tag &&
		    armed)
        {
            hitObject = true;
			if (other.tag == TagsAndEnums.enemy || other.tag == TagsAndEnums.player)
            {
				Health health = other.gameObject.GetComponent<Health>();
 				health.Health -= damage;
            }
        }
    }
}
