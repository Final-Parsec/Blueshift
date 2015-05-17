using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CheckpointActivatedMovement : MonoBehaviour
{
    public float activeRange;
    public Transform mainComponent;
    protected BoxCollider[] boxColliders;
    protected SphereCollider[] sphereColliders;

    protected abstract IEnumerator Active();

    // Use this for initialization
    protected virtual void Start()
    {
        if (mainComponent == null)
            mainComponent = transform.root;

        boxColliders = mainComponent.GetComponentsInChildren<BoxCollider>();
        sphereColliders = mainComponent.GetComponentsInChildren<SphereCollider>();

        if(activeRange > 0)
        {
            foreach (MeshRenderer meshrenderer in mainComponent.GetComponentsInChildren<MeshRenderer>())
                meshrenderer.enabled = false;

            if(boxColliders != null)
                foreach(BoxCollider bc in boxColliders)
                    bc.enabled = false;

            if(sphereColliders != null)
                foreach(SphereCollider sc in sphereColliders)
                    sc.enabled = false;
        }
    }
    
    public virtual void Trigger()
    {
        foreach (MeshRenderer meshrenderer in mainComponent.GetComponentsInChildren<MeshRenderer>())
            meshrenderer.enabled = true;

        if(boxColliders != null)
            foreach(BoxCollider bc in boxColliders)
                bc.enabled = true;

        if(sphereColliders != null)
            foreach(SphereCollider sc in sphereColliders)
                sc.enabled = true;

        StartCoroutine(Active());
    }
}
