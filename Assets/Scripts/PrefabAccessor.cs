using UnityEngine;
using System.Collections.Generic;

public class PrefabAccessor : MonoBehaviour
{
    public static Dictionary<TagsAndEnums.ProjectileType, List<Projectile>> projectilePool = new Dictionary<TagsAndEnums.ProjectileType, List<Projectile>>();
    public static List<Explosion> explosionPool = new List<Explosion>();
    public static PrefabAccessor prefabAccessor;

    public static Projectile GetProjectile(TagsAndEnums.ProjectileType projectileType, string shooter,  Vector3 spawnPosition)
    {
        Projectile proj;
        if (PrefabAccessor.projectilePool.ContainsKey(projectileType) && PrefabAccessor.projectilePool [projectileType].Count != 0)
        {
            proj = PrefabAccessor.projectilePool [projectileType] [0];
            PrefabAccessor.projectilePool [projectileType].RemoveAt(0);
            proj.transform.position = spawnPosition;
            
        } else
        {
            proj = (Instantiate(PrefabAccessor.prefabAccessor.projectilePrefabs [(int)projectileType],
                                spawnPosition,
                                Quaternion.Euler(Vector3.zero)) as GameObject).GetComponent<Projectile>();
        }
        proj.transform.LookAt(ShipMovement.shipMovement.transform.position);
        proj.armed = true;
        proj.shooter = shooter;
        return proj;
    }

    public static Explosion GetExplosion(Vector3 worldPosition)
    {
        Explosion explosion;
        if (PrefabAccessor.explosionPool.Count != 0)
        {
            explosion = PrefabAccessor.explosionPool[0];
            PrefabAccessor.explosionPool.RemoveAt(0);
            explosion.transform.position = worldPosition;

        }
        else
        {
            explosion = (Instantiate(prefabAccessor.explosionPrefab,
                                     worldPosition,
                                     Quaternion.Euler(Vector3.zero)) as GameObject).GetComponent<Explosion>();
        }
        return explosion;
    }

    public List<GameObject> projectilePrefabs;
    public List<AudioClip> shootSounds;
    public List<AudioClip> destructionSounds;
    public GameObject explosionPrefab;

    void Start()
    {
        prefabAccessor = this;
    }

    public AudioClip GetRandomeSound(List<AudioClip> audioList)
    {
        return audioList[Random.Range(0, audioList.Count)];
    }
}
