using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == TagsAndEnums.player)
        {

        }
    }
}
