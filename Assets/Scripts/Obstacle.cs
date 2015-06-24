using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == TagsAndEnums.player)
		{
			ShipHealth.Instance.Health -= (int)(ShipHealth.Instance.Health * .1f);
//			ShipHealth.Instance.transform.position = new Vector3(ShipHealth.Instance.transform.position.x, 
//			                                                     ShipHealth.Instance.transform.position.y + 1,
//			                                                     ShipHealth.Instance.transform.position.z);
			ShipMovement.shipMovement.Ascend();
		}

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == TagsAndEnums.enemy)
		{
            Debug.Log("hit");

			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth.HitsObstacles)
				enemyHealth.Health = 0;
		}
	}
}
