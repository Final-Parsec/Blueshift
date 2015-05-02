using UnityEngine;
using System.Collections;

public class BossFlying : CheckpointActivatedMovement {

	public Transform bankingShip;
	public float travelTime;

	private float bankSpeed = 2;
	private int vertical = 0;
	private int horizontal = 0;

	/// <summary>
	/// Decelerates the ship and raises on y-axis. 
	/// </summary>
	public void Ascend()
	{
		bool pastTopScreenEdge = Camera.main.WorldToScreenPoint(transform.position).y > Screen.height;
		if (!pastTopScreenEdge)
		{
			transform.Translate(0, .2f, 0);
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(-20, 0, 0), (Time.fixedDeltaTime * bankSpeed));
		}
		else
		{
			vertical = 1;
		}
	}
	
	/// <summary>
	/// Steers ship to the left.
	/// </summary>
	void BankLeft()
	{
		bool pastRightScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x >= Screen.width;

		if (!pastRightScreenEdge)
		{
			transform.Translate(-.2f, 0, 0);
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(0, -45, 0), (Time.fixedDeltaTime * bankSpeed));
		}
		else
		{
			horizontal = 1;
		}
	}
	
	/// <summary>
	/// Steers ship to the right.
	/// </summary>
	void BankRight()
	{
		bool pastLeftScreenEdge = Camera.main.WorldToScreenPoint(transform.position).x <= 0;
		if (!pastLeftScreenEdge)
		{
			transform.Translate(.2f, 0, 0);
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(0, 45, 0), (Time.fixedDeltaTime * bankSpeed));
		}
		else
		{
			horizontal = 2;
		}
	}
	
	/// <summary>
	/// Accelerates ship and lowers on y-axis.
	/// </summary>
	void Dive()
	{
		bool pastBottomScreenEdge = Camera.main.WorldToScreenPoint(transform.position).y <= 0;
		
		if (!pastBottomScreenEdge)
		{
			transform.Translate(0, -.2f, 0);
			bankingShip.localRotation = Quaternion.Slerp(bankingShip.localRotation, Quaternion.Euler(10, 0, 0), (Time.fixedDeltaTime * bankSpeed));
		}
		else
		{
			vertical = 2;
		}
	}


	protected override IEnumerator Active()
	{   
		float lastRun = Time.time;
		float lastDirectionChangeTime = 0;

		while (true)
		{
			if(Time.time - lastDirectionChangeTime > travelTime)
			{
				vertical = Random.Range(1,3);
				horizontal = Random.Range(1,3);

				lastDirectionChangeTime = Time.time;
			}

			if(vertical == 1)
				Dive();
			else if (vertical == 2)
				Ascend();

			if(horizontal == 1)
				BankRight();
			else if(horizontal == 2)
				BankLeft();
			
			lastRun = Time.time;
			yield return new WaitForEndOfFrame();
		}
	}

}
