using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

    public float blowUpFactor;
    public float blowUpDuration; // in seconds

    private Vector3 originalScale;
    private AudioSource audioSource;
    private ExplosionMat explosionMat;
	private Renderer rendererComponent;

    // Use this for initialization
    void Awake()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
        explosionMat = GetComponent<ExplosionMat>();
		rendererComponent = GetComponent<Renderer> ();
    }

    public void Explode(int timeDelayMagnitude)
    {
		StartCoroutine(ExplodeCoroutine(timeDelayMagnitude));
    }
    
	IEnumerator ExplodeCoroutine(int timeDelayMagnitude)
    {
		rendererComponent.enabled = false;
		yield return new WaitForSeconds(timeDelayMagnitude * .1f);
		rendererComponent.enabled = true;

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
		rendererComponent.enabled = false;
        explosionMat._alpha = 1;
        transform.localScale = originalScale;
        transform.position = new Vector3(transform.position.x, -99, transform.position.z);
        PrefabAccessor.explosionPool.Add(this);
    }
}
