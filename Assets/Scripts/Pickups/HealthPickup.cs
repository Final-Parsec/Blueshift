using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

    public static int healthPickupValue = 100;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == TagsAndEnums.player)
        {
            ShipHealth.Instance.Health += healthPickupValue;
            Destroy(gameObject);
        }
    }
}
