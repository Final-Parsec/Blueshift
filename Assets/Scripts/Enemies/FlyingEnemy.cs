using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FlyingEnemy : PlayerShootingEnemy
{
    public float activeRange;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

	protected abstract IEnumerator Active ();

    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider = transform.root.GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();

        foreach( MeshRenderer meshrenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
            meshrenderer.enabled = false;
        boxCollider.enabled = false;
        sphereCollider.enabled = false;
    }
    
    public void Trigger()
    {
        foreach( MeshRenderer meshrenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
            meshrenderer.enabled = true;
        boxCollider.enabled = true;
        sphereCollider.enabled = true;
        StartCoroutine(Active());
    }
}
