using UnityEngine;
using System;
using System.Collections;

public class ShipHealth : MonoBehaviour, Health
{
	private static ShipHealth instance;
	public static ShipHealth Instance
	{
		get
		{
			if (instance == null)
			{
				throw new InvalidOperationException("Bruh?");
			}
			return instance;
		}
	}
	
	private Animator animator;
	public int MaxHealth{get; set;}
    public HealthBar healthBar;

	private bool isDead = false;
	private bool isDying = false;
	private GameObject deathModal;
	
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
			if(isDying)
			{
				return;
			}

            if (value < _health)
            {
                if (value <= 0)
    			{
                    _health = 0;
    				Death();
                    GetComponent<BoxCollider>().isTrigger = true;
    			}
    			else
    			{
    				AnimateHit();
    			}
            }
            else if(value > MaxHealth)
            {
                value = MaxHealth;
            }

            _health = value;
            healthBar.UpdateHealthBar(_health, MaxHealth);
		}
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		deathModal = GameObject.Find("DeathModal");
		deathModal.SetActive(false);
        MaxHealth = Health;
        instance = this;
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
	
	void Explode()
	{
		if (!isDead)
		{
			isDead = true;
		
			Explosion explosion = PrefabAccessor.GetExplosion (transform.position, 0);
			explosion.Explode (0);
		}
	}

	void AfterDeathAnimation()
	{
		Explode();
		deathModal.SetActive(true);
        deathModal.GetComponent<Animator>().SetTrigger("fadein");
	}
}