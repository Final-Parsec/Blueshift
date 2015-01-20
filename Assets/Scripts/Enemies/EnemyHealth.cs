using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                PlayDeathSound();
                Destroy(gameObject, .25f);
            }
        }
    }

    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayDeathSound()
    {
        audioSource.clip = PrefabAccessor.prefabAccessor.GetRandomeSound(PrefabAccessor.prefabAccessor.destructionSounds);
        audioSource.Play();
    }
}
