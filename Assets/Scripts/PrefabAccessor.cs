using UnityEngine;
using System.Collections.Generic;

public class PrefabAccessor : MonoBehaviour {
    public static PrefabAccessor prefabAccessor;

    public List<GameObject> projectilePrefabs;

    void Start ()
    {
        prefabAccessor = this;
    }
}
