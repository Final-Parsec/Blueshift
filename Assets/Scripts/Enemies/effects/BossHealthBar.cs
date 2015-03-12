using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossHealthBar : MonoBehaviour {

    public Animator animator;
    private float startingHeight;
    private float startingYPosition;
    private RectTransform rectTransform;

	void Start () 
    {
        animator = GetComponentInParent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        startingHeight = rectTransform.sizeDelta.y;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);

        startingYPosition = rectTransform.localPosition.y;
	}
	
	public void UpdateHealthBar (int health, int maxHealth) 
    {
        float healthRatio = ((float)health) / ((float)maxHealth);
        float newHeight = startingHeight * (1 - healthRatio);

        StartCoroutine(MoveBar(newHeight));
	}

    IEnumerator MoveBar(float targetHeight)
    {
        while (rectTransform.sizeDelta.y < targetHeight)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 1);
            
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,
                                                      startingYPosition + ((startingHeight - rectTransform.sizeDelta.y) / 2.0f),
                                                      rectTransform.localPosition.z);

            yield return new WaitForEndOfFrame();
        }
    }

    public void SwoopIn()
    {
        animator.SetTrigger("swoopin");
    }

    public void SwoopOut()
    {
        animator.SetTrigger("swoopout");
    }
}
