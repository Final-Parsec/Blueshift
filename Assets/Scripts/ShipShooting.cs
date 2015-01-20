using UnityEngine;
using System.Collections;

public class ShipShooting : MonoBehaviour
{
    public TagsAndEnums.ProjectileType projectileType;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
            RaycastHit mainHit = new RaycastHit();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == TagsAndEnums.enemy)
                {
                    mainHit = hit;
                    break;
                }
                if (hit.collider.tag == TagsAndEnums.terrain)
                    mainHit = hit;

            }
            Projectile proj = Projectile.GetProjectile(projectileType, this);
            proj.transform.LookAt(mainHit.point);
            Vector3 aimVector = Vector3.Normalize(transform.position - mainHit.point);
            proj.Intercept(aimVector);
            PlayShootSound();
        }
    }

    void PlayShootSound()
    {
        audioSource.clip = PrefabAccessor.prefabAccessor.GetRandomeSound(PrefabAccessor.prefabAccessor.shootSounds);
        audioSource.Play();
    }
}