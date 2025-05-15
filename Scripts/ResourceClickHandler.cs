using UnityEngine;

public class ResourceClickHandler : MonoBehaviour
{
    System.Action<GameObject, ResourceType, int> onClick;
    ResourceType type;
    ResourceHitCounter hitCounter;
    float shakeStrength;
    float shakeDuration;
    bool isShaking = false;
    Vector3 originalPos;

    GameObject stoneHitParticlesPrefab;
    GameObject woodHitParticlesPrefab;
    Color goldColor;

    public void Init(System.Action<GameObject, ResourceType, int> callback, ResourceType resourceType, float shakeStrength, float shakeDuration, GameObject stoneHitParticlesPrefab, GameObject woodHitParticlesPrefab, Color goldColor)
    {
        onClick = callback;
        type = resourceType;
        hitCounter = GetComponent<ResourceHitCounter>();
        if (hitCounter == null)
            hitCounter = gameObject.AddComponent<ResourceHitCounter>();
        this.shakeStrength = shakeStrength;
        this.shakeDuration = shakeDuration;
        this.stoneHitParticlesPrefab = stoneHitParticlesPrefab;
        this.woodHitParticlesPrefab = woodHitParticlesPrefab;
        this.goldColor = goldColor;
        originalPos = transform.localPosition;
    }

    void OnMouseDown()
    {
        int hits = hitCounter.AddHit();
        if (onClick != null)
            onClick(gameObject, type, hits);

        if (type == ResourceType.Stone || type == ResourceType.Wood)
        {
            if (hits < 10)
            {
                if (!isShaking)
                    StartCoroutine(Shake());
                SpawnParticles(false);
            }
            else if (hits == 10)
            {
                SpawnParticles(true);
            }
        }
    }

    System.Collections.IEnumerator Shake()
    {
        isShaking = true;
        float elapsed = 0f;
        Vector3 startPos = originalPos;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;
            transform.localPosition = startPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPos;
        isShaking = false;
    }

    void SpawnParticles(bool isGold)
    {
        GameObject prefab = null;
        if (type == ResourceType.Stone)
            prefab = stoneHitParticlesPrefab;
        else if (type == ResourceType.Wood)
            prefab = woodHitParticlesPrefab;

        if (prefab == null) return;

        Vector3 spawnPos = transform.position;
        Collider col = GetComponent<Collider>();
        if (col != null)
            spawnPos = col.bounds.center;

        GameObject particlesObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (isGold)
        {
            var ps = particlesObj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startColor = goldColor;
            }
        }

        Destroy(particlesObj, 2f);
    }
}