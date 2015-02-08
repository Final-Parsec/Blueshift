using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShooting : MonoBehaviour {
	
    //  Shooting
    public TagsAndEnums.ProjectileType projectileType;
    public List<Transform> muzzlePoints;
    public bool shootFromEveryMuzzle;
    public float shootSpeed;
    public int aimError;
    public int numProjectilesInBurst;
    public float burstInterval;
    public bool shotsLeadPlayer;
    public bool shootsStraight;  // Note: does not use aimError when true

    private int currentMuzzlePoint = 0;
    private GameObject target;

    // Target Tracking
    public float aimRotationSpeed; // Note: set to 0 to not track the player.
    public Transform rotatingObject;
	public TagsAndEnums.AimingDirection direction;

    IEnumerator Aim()
    {
        while (target != null)
        {
            Vector3 targetDirection = target.transform.position - rotatingObject.position;
            Vector3 newDirection = Vector3.RotateTowards(rotatingObject.forward, targetDirection, Time.deltaTime * aimRotationSpeed, 0);
            rotatingObject.rotation = Quaternion.LookRotation(newDirection);
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

	Vector3 GetAimingDirection()
	{
		switch(direction)
		{
		case TagsAndEnums.AimingDirection.forward:
			return rotatingObject.forward;

		case TagsAndEnums.AimingDirection.back:
			return rotatingObject.forward * -1f;

		case TagsAndEnums.AimingDirection.right:
			return rotatingObject.right;

		case TagsAndEnums.AimingDirection.left:
			return rotatingObject.right * -1f;

		case TagsAndEnums.AimingDirection.up:
			return rotatingObject.up;

		case TagsAndEnums.AimingDirection.down:
			return rotatingObject.up * -1f;

		default:
			return rotatingObject.right;
			
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagsAndEnums.player)
        {
            target = other.gameObject;
            if (aimRotationSpeed != 0) // floating point persision won't matter
                StartCoroutine(Aim());
            if (numProjectilesInBurst != 0)
                StartCoroutine(this.Fire());
        }
    }
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == TagsAndEnums.player)
			target = null;
	}

    IEnumerator Fire()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootSpeed);
            for (int x = 0; x < numProjectilesInBurst; x++)
            {
                if (target == null)
                    break;
                
                if (shotsLeadPlayer)
                    ShootLeadingProjectile();
                else if (shootsStraight)
                    ShootStraightProjectile();
                
                if (numProjectilesInBurst != 1)
                    yield return new WaitForSeconds(burstInterval);
            }
        }
    }
    
    void ShootStraightProjectile()
    {
        int count = 0;
        do
        {
            Projectile proj = Projectile.GetProjectile(projectileType, transform.root.tag, GetMuzzlePoint().transform.position);
            
            Vector3 aimVector = transform.root.forward * -1;
            
            proj.Intercept(aimVector, 0);
            count++;
        } while(shootFromEveryMuzzle && count < muzzlePoints.Count);
    }
    
    void ShootLeadingProjectile()
    {
        int count = 0;
        do
        {
            Projectile proj = Projectile.GetProjectile(projectileType, transform.root.tag, GetMuzzlePoint().transform.position);
            
            Vector3 aimErrorVector = new Vector3(Random.Range(-1f, 1f),
                                                 Random.Range(-1f, 1f),
                                                 Random.Range(-1f, 1f)).normalized * aimError;
            Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
            Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed * -1;
            
            Vector3 aimVector = Projectile.FindInterceptVector(proj.transform.position, proj.speed, ShipMovement.shipMovement.transform.position - aimErrorVector, shipVelocity);
            
            proj.Intercept(aimVector, 0);
            count++;
        } while(shootFromEveryMuzzle && count < muzzlePoints.Count);
    }
}
