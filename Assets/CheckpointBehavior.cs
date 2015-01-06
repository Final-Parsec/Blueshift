using UnityEngine;
using System.Collections;

public class CheckpointBehavior : MonoBehaviour
{

    public bool loadScene = false;
    public string sceneToLoad = "";

    void OnTriggerEnter(Collider col)
    {
        if (loadScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            Application.LoadLevel(sceneToLoad);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
