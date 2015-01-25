using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public static Dictionary<TagsAndEnums.ProjectileType, List<Projectile>> projectilePool = new Dictionary<TagsAndEnums.ProjectileType, List<Projectile>>();
    public float speed;
    public int damage;
    public TagsAndEnums.ProjectileType projectileType;
    public float selfDestructRange = 500;
    private bool hitObject = false;
    private bool armed = false;
    private string shooter;

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

    public static Projectile GetProjectile(TagsAndEnums.ProjectileType projectileType, string shooter,  Vector3 spawnPosition)
    {
        Projectile proj;
        if (Projectile.projectilePool.ContainsKey(projectileType) && Projectile.projectilePool [projectileType].Count != 0)
        {
            proj = Projectile.projectilePool [projectileType] [0];
            Projectile.projectilePool [projectileType].RemoveAt(0);
            proj.transform.position = spawnPosition;

        } else
        {
            proj = (Instantiate(PrefabAccessor.prefabAccessor.projectilePrefabs [(int)projectileType],
                                spawnPosition,
                                Quaternion.Euler(Vector3.zero)) as GameObject).GetComponent<Projectile>();
        }
        proj.transform.LookAt(ShipMovement.shipMovement.transform.position);
        proj.armed = true;
        proj.shooter = shooter;
        return proj;
    }

    public void Intercept(Vector3 moveVector)
    {
        StartCoroutine(InterceptCoroutine(moveVector));
    }

    IEnumerator InterceptCoroutine(Vector3 moveVector)
    {
        Vector3 origin = transform.position;
        while (TagsAndEnums.GetSqrDistance(origin, transform.position) < selfDestructRange*selfDestructRange && !hitObject)
        {
            float step = speed * Time.deltaTime;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag != TagsAndEnums.projectile && other.tag != TagsAndEnums.ignore && shooter != other.transform.root.gameObject.tag && armed)
        {
            hitObject = true;
            if (other.tag == TagsAndEnums.enemy)
            {
                EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
                enemy.Health -= damage;
            }
        }
    }
}
