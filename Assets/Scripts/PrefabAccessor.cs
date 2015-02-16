using UnityEngine;
using System.Collections.Generic;

public class PrefabAccessor : MonoBehaviour
{
    public static Dictionary<TagsAndEnums.ProjectileType, List<Projectile>> projectilePool = new Dictionary<TagsAndEnums.ProjectileType, List<Projectile>>();
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

    public static void MakeExplosion(Vector3 worldPosition)
    {
    }

    public List<GameObject> projectilePrefabs;
    public List<AudioClip> shootSounds;
    public List<AudioClip> destructionSounds;
    public GameObject explosion;

    void Start()
    {
        prefabAccessor = this;
    }

    public AudioClip GetRandomeSound(List<AudioClip> audioList)
    {
        return audioList[Random.Range(0, audioList.Count)];
    }
}
