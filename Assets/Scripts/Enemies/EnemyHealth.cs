using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int health;
	public int numberOfExplosions;
	public float explosionPositionVariance;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                Death();
                Destroy(gameObject);
            }
        }
    }

    void Death()
    {
		foreach (EnemyHealth child in GetComponentsInChildren<EnemyHealth>() as EnemyHealth[]){
			if(child != this)
				child.Health = 0;
		}


		for (int x = 0; x < numberOfExplosions; x++) {
			Explosion explosion = PrefabAccessor.GetExplosion (transform.position, explosionPositionVariance);
			explosion.Explode (x);
		}
    }
}
