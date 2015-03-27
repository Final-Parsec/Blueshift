using UnityEngine;
using System.Collections;

public class TouchBackFlying : CheckpointActivatedMovement 
{
	public float speed;
	public float speedWhileTurning;
	public float postTurnSpeed;
	public float distanceToReach;
	public float rotateSpeed;
	
	private bool wentTheDistance = false;
	private bool turningAround = false;

	protected override IEnumerator Active()
	{   
		float lastRun = Time.time;
		Vector3 targetDir = Vector3.zero;
		while (TagsAndEnums.GetSqrDistance(transform.root.position, ShipMovement.shipMovement.transform.position) < activeRange*activeRange)
		{
			Transform cameraTransform = CameraMovement.cameraMovement.transform;
			if(!wentTheDistance)
			{
				if(TagsAndEnums.GetSqrDistance(transform.root.position, cameraTransform.position) >= distanceToReach * distanceToReach)
				{
					wentTheDistance = true;
					turningAround = true;
					targetDir = transform.root.forward * -1;
					speed = speedWhileTurning;
				}
			} 
			else if(turningAround)
			{
				Quaternion startQuat = transform.root.rotation;
				Vector3 newDirection = Vector3.RotateTowards(transform.root.forward, targetDir, Time.deltaTime * rotateSpeed, 0);
				transform.root.rotation = Quaternion.LookRotation(newDirection);
				if(startQuat.eulerAngles == transform.root.rotation.eulerAngles)
				{
					turningAround = false;
					speed = postTurnSpeed;
                    if(sphereColliders != null)
                        foreach(SphereCollider sc in sphereColliders)
                            sc.enabled = true;
				}
			}

			Vector3 moveVector = transform.root.forward;
			transform.root.position = transform.root.position + moveVector * speed * (Time.time - lastRun);

			lastRun = Time.time;
			yield return new WaitForSeconds(.009f);
		}
	}
	
	protected override void Start()
	{
		base.Start();
	}

	public override void Trigger()
	{
		foreach (MeshRenderer meshrenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
			meshrenderer.enabled = true;
        if(boxColliders != null)
            foreach(BoxCollider bc in boxColliders)
                bc.enabled = true;
		//sphereCollider.enabled = true;
		StartCoroutine(Active());
	}
}
