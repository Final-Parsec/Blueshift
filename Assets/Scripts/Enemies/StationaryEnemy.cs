using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StationaryEnemy : MonoBehaviour
{
    public TagsAndEnums.ProjectileType projectileType;
    public float shootSpeed;
    public int aimRotationSpeed;
    public int aimError;
    public int numProjectilesInBurst;
    public float burstInterval;
    public List<Transform> muzzlePoints;
    public bool shootFromEveryMuzzle;
    private int currentMuzzlePoint = 0;
    private GameObject target;

    IEnumerator BurstFire()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootSpeed);
            for (int x = 0; x < numProjectilesInBurst; x++)
            {
                if (target == null)
                    break;

                Projectile proj = Projectile.GetProjectile(projectileType, transform.root.tag, GetMuzzlePoint().transform.position);
            
                Vector3 aimErrorVector = new Vector3(Random.Range(-1f, 1f),
                                                     Random.Range(-1f, 1f),
                                                     Random.Range(-1f, 1f)).normalized * aimError;
                Vector3 projMoveVector = Vector3.Normalize(proj.transform.position - target.transform.position);
                Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
                Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed * -1;
            
                Vector3 aimVector = Projectile.FindInterceptVector(proj.transform.position, proj.speed, target.transform.position - aimErrorVector, shipVelocity);
            
                proj.Intercept(aimVector);
                if (numProjectilesInBurst != 1)
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

    Transform GetMuzzlePoint()
    {
        
        Transform point = muzzlePoints[currentMuzzlePoint];
        currentMuzzlePoint++;
        if(currentMuzzlePoint >= muzzlePoints.Count)
            currentMuzzlePoint = 0;
        return point;
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
