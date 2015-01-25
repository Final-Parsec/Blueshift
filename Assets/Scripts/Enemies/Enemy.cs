using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	public TagsAndEnums.ProjectileType projectileType;
	public List<Transform> muzzlePoints;
	public bool shootFromEveryMuzzle;
	protected int currentMuzzlePoint = 0;
	protected GameObject target;

	protected Transform GetMuzzlePoint()
	{
		Transform point = muzzlePoints[currentMuzzlePoint];
		currentMuzzlePoint++;
		if(currentMuzzlePoint >= muzzlePoints.Count)
			currentMuzzlePoint = 0;
		return point;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == TagsAndEnums.player)
		{
			target = other.gameObject;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == TagsAndEnums.player)
			target = null;
	}
}
