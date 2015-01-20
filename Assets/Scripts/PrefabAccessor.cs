using UnityEngine;
using System.Collections.Generic;

public class PrefabAccessor : MonoBehaviour
{
    public static PrefabAccessor prefabAccessor;
    public List<GameObject> projectilePrefabs;
    public List<AudioClip> shootSounds;
    public List<AudioClip> destructionSounds;

    void Start()
    {
        prefabAccessor = this;
    }

    public AudioClip GetRandomeSound(List<AudioClip> audioList)
    {
        return audioList[Random.Range(0, audioList.Count)];
    }
}
