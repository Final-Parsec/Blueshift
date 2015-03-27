using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour, Health
{
    public int _health;
	public int numberOfExplosions;
	public float explosionPositionVariance;
    public float blinkTime = .3f;
    public bool godMode;

	private Animator animator;
	private MeshRenderer[] meshRenderers;
	private float blinkSpeed = .04f;
	private Color blinkColor = Color.red;
	private Color[] naturalColors;
    public int MaxHealth{get; set;}

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            if(godMode)
                return;

            _health = value;
            if (_health <= 0)
            {
                Death();
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
		meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
		animator = GetComponent<Animator>();
        naturalColors = new Color[meshRenderers.Length];
        for(int x = 0; x < meshRenderers.Length; x++)
            naturalColors[x] = meshRenderers[x].material.color;
	}

	IEnumerator AnimateHit()
	{
		float startTime = Time.time;
		int counter = 1;

		if (animator != null)
			animator.SetTrigger("damage");

		while(Time.time - startTime < blinkTime)
		{
			if (counter % 2 == 1)
				SetColorForMeshs(blinkColor);
			else
				SetColorsForMeshs(naturalColors);

			counter++;
			yield return new WaitForSeconds(blinkSpeed);
		}

		SetColorsForMeshs(naturalColors);
	}

    public void Death()
    {
		foreach (EnemyHealth child in GetComponentsInChildren<EnemyHealth>() as EnemyHealth[]){
			if(child != this)
				child.Death();
		}


		for (int x = 0; x < numberOfExplosions; x++) {
			Explosion explosion = PrefabAccessor.GetExplosion (transform.position, explosionPositionVariance);
			explosion.Explode (x);
		}

        Destroy(gameObject);
    }

	void SetColorsForMeshs(Color[] colors)
	{
        for(int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = colors[x];
	}

    void SetColorForMeshs(Color color)
    {
        for(int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = color;
    }
}
