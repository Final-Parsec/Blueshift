using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int _health;
	public int numberOfExplosions;
	public float explosionPositionVariance;
	public Animator damageAnimator;

	private MeshRenderer[] meshRenderers;
	private float blinkTime = .3f;
	private float blinkSpeed = .04f;
	private Color blinkColor = Color.red;
	private Color naturalColor;
    public int MaxHealth{get; set;}

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                Death();
                Destroy(gameObject);
            }
			else
			{
				StartCoroutine(AnimateHit());
			}
        }
    }

	void Start()
	{
        MaxHealth = _health;
		meshRenderers = transform.root.GetComponentsInChildren<MeshRenderer>();
		if(meshRenderers.Length > 0)
			naturalColor = meshRenderers[0].material.color;
	}

	IEnumerator AnimateHit()
	{
		float startTime = Time.time;
		int counter = 1;

		if (damageAnimator != null)
			damageAnimator.SetTrigger("damage");

		while(Time.time - startTime < blinkTime)
		{
			if (counter % 2 == 1)
				SetColorForMeshs(blinkColor);
			else
				SetColorForMeshs(naturalColor);

			counter++;
			yield return new WaitForSeconds(blinkSpeed);
		}

		SetColorForMeshs(naturalColor);
	}

    void Death()
    {
		foreach (EnemyHealth child in GetComponentsInChildren<EnemyHealth>() as EnemyHealth[]){
			if(child != this)
				child.Health = 0;
		}


		for (int x = 0; x < numberOfExplosions; x++) {
			Explosion explosion = PrefabAccessor.GetExplosion (transform.position, explosionPositionVariance);
			explosion.Explode (x);
		}
    }

	void SetColorForMeshs(Color color)
	{
		foreach (MeshRenderer mr in meshRenderers)
			mr.material.color = color;
	}
}
