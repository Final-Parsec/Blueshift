using UnityEngine;
using System.Collections;

public class StraightFlyEnemy : CheckpointActivatedMovement
{
    public float speed;
    private Vector3 moveVector;
    
    protected override IEnumerator Active()
    {   
        while (TagsAndEnums.GetSqrDistance(transform.root.position, ShipMovement.shipMovement.transform.position) < activeRange*activeRange)
        {
            transform.root.position = transform.root.position + moveVector * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
    protected override void Start()
    {
        moveVector = transform.root.forward;
        base.Start();
    }
}
