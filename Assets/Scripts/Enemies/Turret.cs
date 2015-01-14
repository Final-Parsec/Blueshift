using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public static TagsAndEnums.ProjectileType projectileType;
    public float shootSpeed;
    public int aimRotationSpeed = 3;
    private GameObject target;

    private static Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed,
                                               Vector3 targetOrigin, Vector3 targetVel) {
        
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
        if (shotVelSpeed > shotSpeed) {
            // Shot is too slow to intercept target, it will never catch up.
            // Do our best by aiming in the direction of the targets velocity.
            return (targetVel.normalized * shotSpeed).normalized * -1;
        } else {
            // We know the shot speed, and the tangential velocity.
            // Using pythagoras we can find the orthogonal velocity.
            float shotSpeedOrth =
                Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
            Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;
            
            // Finally, add the tangential and orthogonal velocities.
            return (shotVelOrth + shotVelTang).normalized * -1;
        }
    }

    IEnumerator Fire()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootSpeed);
            if (target == null)
                break;

            Projectile proj;
            if (Projectile.projectilePool.ContainsKey(projectileType) && Projectile.projectilePool [projectileType].Count != 0)
            {
                proj = Projectile.projectilePool [projectileType] [0];
                Projectile.projectilePool [projectileType].RemoveAt(0);

                proj.transform.rotation = transform.rotation;
                proj.transform.position = transform.position;

            } else
            {
                proj = (Instantiate(PrefabAccessor.prefabAccessor.projectilePrefabs [(int)projectileType],
                                              transform.position,
                                              transform.rotation) as GameObject).GetComponent<Projectile>();
            }

            Vector3 projMoveVector = new Vector3(proj.transform.position.x - target.transform.position.x,
                                                 proj.transform.position.y - target.transform.position.y,
                                                 proj.transform.position.z - target.transform.position.z).normalized;

            float distance = Vector3.Distance(proj.transform.position, target.transform.position);
            float travelTime = (distance) / proj.speed;
            Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
            Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed;

            Vector3 aimVector = FindInterceptVector(proj.transform.position, proj.speed, target.transform.position, shipVelocity*-1);
            
            StartCoroutine(proj.Intercepting(aimVector));

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
            StartCoroutine(Fire());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagsAndEnums.player)
            target = null;
    }

    // Use this for initialization
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
