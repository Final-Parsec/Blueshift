using UnityEngine;
using System.Collections;

public class ShipHealth : MonoBehaviour, Health {
	private Animator animator;
	public int MaxHealth{get; set;}

	private bool isDying = false;
	public bool IsDying
	{
		get
		{
			return isDying;
		}
		set
		{
			isDying = value;
		}
	}

	public int _health;
	public int Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = value;
			
			if(isDying)
				return;

			if (_health <= 0)
			{
				Death();
				//Destroy(gameObject);
			}
			else
			{
				AnimateHit();
			}
		}
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void AnimateHit()
	{
		animator.SetTrigger("damage");
	}
	
	void Death()
	{
		isDying = true;
		animator.SetTrigger("death");
	}

	void AfterDeathAnimation()
	{
		Explosion explosion = PrefabAccessor.GetExplosion (transform.position, 0);
		explosion.Explode (0);
	}

	void OnTriggerEnter(Collider other)
	{
		if (IsDying && other.tag == TagsAndEnums.terrain) 
		{
			Explosion explosion = PrefabAccessor.GetExplosion (transform.position, 0);
			explosion.Explode (0);
		}
	}
}
