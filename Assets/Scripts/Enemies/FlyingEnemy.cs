using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemy : MonoBehaviour
{
    public float activeRange;
    public float shootingRange;
    public TagsAndEnums.ProjectileType projectileType;
    public float shootSpeed;
    public int aimError;
    public int numProjectilesInBurst;
    public float burstInterval;
    public List<Transform> muzzlePoints;
    public bool shootFromEveryMuzzle;
    private int currentMuzzlePoint = 0;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private GameObject target;

    IEnumerator Active()
    {
        while (TagsAndEnums.GetSqrDistance(transform.position, ShipMovement.shipMovement.transform.position) > activeRange*activeRange)
        {

            //Put movement here

            yield return new WaitForEndOfFrame();
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
                
                Projectile proj = Projectile.GetProjectile(projectileType, transform.root.tag, GetMuzzlePoint().transform.position);
                
                Vector3 aimErrorVector = new Vector3(Random.Range(-1f, 1f),
                                                     Random.Range(-1f, 1f),
                                                     Random.Range(-1f, 1f)).normalized * aimError;
                Vector3 projMoveVector = Vector3.Normalize(proj.transform.position - ShipMovement.shipMovement.transform.position);
                Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
                Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed * -1;
                
                Vector3 aimVector = Projectile.FindInterceptVector(proj.transform.position, proj.speed, ShipMovement.shipMovement.transform.position - aimErrorVector, shipVelocity);
                
                proj.Intercept(aimVector);
                if (numProjectilesInBurst != 1)
                    yield return new WaitForSeconds(burstInterval);
            }
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

    // Use this for initialization
    void Start()
    {
        boxCollider = transform.root.GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();

        foreach( MeshRenderer meshrenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
            meshrenderer.enabled = false;
        boxCollider.enabled = false;
        sphereCollider.enabled = false;
    }
    
    public void Trigger()
    {
        foreach( MeshRenderer meshrenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
            meshrenderer.enabled = true;
        boxCollider.enabled = true;
        sphereCollider.enabled = true;

        StartCoroutine(Active());
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == TagsAndEnums.player)
        {
            target = other.gameObject;
            StartCoroutine(BurstFire());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagsAndEnums.player)
            target = null;
        
    }
}
