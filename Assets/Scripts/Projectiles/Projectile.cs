using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public static Dictionary<TagsAndEnums.ProjectileType, List<Projectile>> projectilePool = new Dictionary<TagsAndEnums.ProjectileType, List<Projectile>>();
    public float speed;
    public int damage;
    public TagsAndEnums.ProjectileType projectileType;
    public float selfDestructRange = 500;
    private bool hitObject = false;
    private bool armed = false;
    private string shooter;

    public static Projectile GetProjectile(TagsAndEnums.ProjectileType projectileType, MonoBehaviour shooter)
    {
        Projectile proj;
        if (Projectile.projectilePool.ContainsKey(projectileType) && Projectile.projectilePool [projectileType].Count != 0)
        {
            proj = Projectile.projectilePool [projectileType] [0];
            Projectile.projectilePool [projectileType].RemoveAt(0);
            
            proj.transform.rotation = shooter.transform.rotation;
            proj.transform.position = shooter.transform.position;

        } else
        {
            proj = (Instantiate(PrefabAccessor.prefabAccessor.projectilePrefabs [(int)projectileType],
                                shooter.transform.position,
                                shooter.transform.rotation) as GameObject).GetComponent<Projectile>();
        }
        proj.armed = true;
        proj.shooter = shooter.transform.root.gameObject.tag.Clone() as string;
        return proj;
    }

    public void Intercept(Vector3 moveVector)
    {
        StartCoroutine(InterceptCoroutine(moveVector));
    }

    IEnumerator InterceptCoroutine(Vector3 moveVector)
    {
        Vector3 origin = transform.position;
        while (Vector3.Distance(origin, transform.position) < selfDestructRange && !hitObject)
        {
            float step = speed * Time.deltaTime;
            // update the position
            transform.position = new Vector3(transform.position.x - moveVector.x * step,
                                              transform.position.y - moveVector.y * step,
                                              transform.position.z - moveVector.z * step);
            yield return new WaitForEndOfFrame();
        }
        armed = false;
        hitObject = false;

        transform.position = Vector3.zero;
        if (!projectilePool.ContainsKey(projectileType))
            projectilePool.Add(projectileType, new List<Projectile>());
        projectilePool [projectileType].Add(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag != TagsAndEnums.projectile && other.tag != TagsAndEnums.ignore && shooter != other.transform.root.gameObject.tag && armed)
        {
            hitObject = true;
            if (other.tag == TagsAndEnums.enemy)
            {
                EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
                enemy.Health -= damage;
            }
        }
    }
}
