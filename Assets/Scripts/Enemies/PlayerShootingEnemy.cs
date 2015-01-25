using UnityEngine;
using System.Collections;

public class PlayerShootingEnemy : Enemy {

	public float shootSpeed;
	public int aimError;
	public int numProjectilesInBurst;
	public float burstInterval;

	protected IEnumerator BurstFire()
	{
		while (target != null)
		{
			yield return new WaitForSeconds(shootSpeed);
			for (int x = 0; x < numProjectilesInBurst; x++)
			{
				if (target == null)
					break;
				
				ShootProjectile();

				if (numProjectilesInBurst != 1)
					yield return new WaitForSeconds(burstInterval);
			}
		}
	}

	new void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == TagsAndEnums.player)
		{
			target = other.gameObject;
			StartCoroutine(BurstFire());
		}
	}

	protected void ShootProjectile()
	{
		int count = 0;
		do{
			Projectile proj = Projectile.GetProjectile(projectileType, transform.root.tag, GetMuzzlePoint().transform.position);
			
			Vector3 aimErrorVector = new Vector3(Random.Range(-1f, 1f),
			                                     Random.Range(-1f, 1f),
			                                     Random.Range(-1f, 1f)).normalized * aimError;
			Vector3 shipMoveVector = CameraMovement.cameraMovement.GetMovementVector();
			Vector3 shipVelocity = shipMoveVector * CameraMovement.cameraMovement.speed * -1;
			
			Vector3 aimVector = Projectile.FindInterceptVector(proj.transform.position, proj.speed, ShipMovement.shipMovement.transform.position - aimErrorVector, shipVelocity);
			
			proj.Intercept(aimVector);
			count++;
		}while(shootFromEveryMuzzle && count < muzzlePoints.Count);
	}

}
