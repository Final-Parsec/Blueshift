using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretEnemy : PlayerShootingEnemy
{
	public float aimRotationSpeed;
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

    new void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == TagsAndEnums.player)
        {
            target = other.gameObject;
            StartCoroutine(Aim());
            StartCoroutine(this.BurstFire());
        }
    }
}
