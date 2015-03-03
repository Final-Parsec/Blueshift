using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

    public float blowUpFactor;
    public float blowUpDuration; // in seconds

    private Vector3 originalScale;
    private AudioSource audioSource;
    private ExplosionMat explosionMat;

    // Use this for initialization
    void Awake()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
        explosionMat = GetComponent<ExplosionMat>();
    }

    public void Explode()
    {
        StartCoroutine(ExplodeCoroutine());
    }
    
    IEnumerator ExplodeCoroutine()
    {
        audioSource.clip = PrefabAccessor.prefabAccessor.GetRandomeSound(PrefabAccessor.prefabAccessor.destructionSounds);
        audioSource.Play();

        float usedDuration = Random.Range(-.2f,.2f);
        usedDuration += blowUpDuration;

        explosionMat._frequency = Random.Range(1f,3f);
        float startTime = Time.time;
        while (startTime + usedDuration > Time.time)
        {
            Vector3 transformSize = new Vector3(transform.localScale.x + blowUpFactor * Time.deltaTime,
                                            transform.localScale.y + blowUpFactor * Time.deltaTime,
                                            transform.localScale.z + blowUpFactor * Time.deltaTime);
            transform.localScale = transformSize;
            explosionMat._alpha = 1 - ((Time.time - startTime)/blowUpDuration);

            yield return new WaitForEndOfFrame();
        }

        BackInThePool();
    }
    
    protected void BackInThePool()
    {
        explosionMat._alpha = 1;
        transform.localScale = originalScale;
        transform.position = new Vector3(transform.position.x, -99, transform.position.z);
        PrefabAccessor.explosionPool.Add(this);
    }
}
