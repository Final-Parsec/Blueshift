using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public TagsAndEnums.ProjectileType projectileType;
    public float shootSpeed;
    public int aimRotationSpeed;
    public int aimError;
    public int numProjectilesInBurst;
    public float burstInterval;
    private GameObject target;

    private static Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed,
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

    IEnumerator BurstFire()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootSpeed);
            for (int x = 0; x < numProjectilesInBurst; x++)
            {
                if (target == null)
                    break;

                Projectile proj = Projectile.GetProjectile(projectileType, this);
            
                Vector3 aimErrorVector = new Vector3(Random.Range(-1, 1),
                                                     Random.Range(-1, 1),
                                                     Random.Range(-1, 1)).normalized * aimError;
                Vector3 projMoveVector = Vector3.Normalize(proj.transform.position - target.transform.position);
                Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
                Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed * -1;
            
                Vector3 aimVector = FindInterceptVector(proj.transform.position, proj.speed, target.transform.position - aimErrorVector, shipVelocity);
            
                StartCoroutine(proj.Intercept(aimVector));
                if(numProjectilesInBurst != 1)
                    yield return new WaitForSeconds(burstInterval);
            }
        }
    }

    IEnumerator Aim()
    {
        while (target != null)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * aimRotationSpeed, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return new WaitForEndOfFrame();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == TagsAndEnums.player)
        {
            target = other.gameObject;
            StartCoroutine(Aim());
            StartCoroutine(BurstFire());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagsAndEnums.player)
            target = null;
    }
}
