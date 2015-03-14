using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossHealthBar : HealthBar {

    public Animator animator;

    void Awake () 
    {
        animator = GetComponentInParent<Animator>();
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
