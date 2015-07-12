using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == TagsAndEnums.player)
		{
			ShipHealth.Instance.Health -= (int)(ShipHealth.Instance.Health * .1f);
			ShipMovement.shipMovement.Ascend();
		}

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == TagsAndEnums.enemy)
		{
			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth.HitsObstacles)
				enemyHealth.Health = 0;
		}
	}
}
