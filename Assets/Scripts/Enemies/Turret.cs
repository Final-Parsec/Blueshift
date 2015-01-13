using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public static TagsAndEnums.ProjectileType projectileType;
    public float shootSpeed;
    public int aimRotationSpeed = 3;
    private GameObject target;

    IEnumerator Fire()
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootSpeed);
            if(target == null)
                break;

            if (Projectile.projectilePool.ContainsKey(projectileType) && Projectile.projectilePool [projectileType].Count != 0)
            {
                Projectile proj = Projectile.projectilePool [projectileType] [0];
                Projectile.projectilePool [projectileType].RemoveAt(0);

                proj.transform.rotation = transform.rotation;
                proj.transform.position = transform.position;

                Vector3 moveVector = new Vector3 (transform.position.x - target.transform.position.x,
                                                  transform.position.y - target.transform.position.y,
                                                  transform.position.z - target.transform.position.z).normalized;
                StartCoroutine( proj.Intercepting(moveVector));


            } else
            {
                Projectile proj = (Instantiate(PrefabAccessor.prefabAccessor.projectilePrefabs [(int)projectileType],
                                              transform.position,
                                              transform.rotation) as GameObject).GetComponent<Projectile>();
                Vector3 moveVector = new Vector3 (transform.position.x - target.transform.position.x,
                                                  transform.position.y - target.transform.position.y,
                                                  transform.position.z - target.transform.position.z).normalized;
                StartCoroutine( proj.Intercepting(moveVector));
            }
        }
    }

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
    
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == TagsAndEnums.player)
        {
            target = other.gameObject;
            StartCoroutine(Aim());
            StartCoroutine(Fire());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagsAndEnums.player)
            target = null;
    }

    // Use this for initialization
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
