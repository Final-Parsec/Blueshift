using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health;

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
        Explosion explosion = PrefabAccessor.GetExplosion(transform.position);
        explosion.Explode();
    }
}
