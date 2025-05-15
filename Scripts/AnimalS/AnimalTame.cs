using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class AnimalTame : MonoBehaviour
{
    public AnimalType animalType;
    public bool isTamed = false;

    [Header("Приручение")]
    public ResourceInventory inventory;
    public AudioSource audioSource;
    public AudioClip ewwClip;

    public TMP_Text stoneText;
    public TMP_Text woodText;
    public TMP_Text fishText;

    public UnityEvent onTamed;
    public int tameCost = 10;

    [Header("Resource Production")]
    public AudioClip collectClip;
    [Range(0f, 1f)]
    public float collectVolume = 1f;

    [Header("Уровень")]
    public int level = 1;
    public int maxLevel = 5;

    [Header("Resource Icon Settings")]
    public GameObject woodIconPrefab;
    public GameObject stoneIconPrefab;
    public GameObject fishIconPrefab;
    public Transform sklad1Transform;
    public Transform iconParent;
    public float iconScale = 0.2f;

    [Header("Ballistic Settings")]
    public float arcHeight = 2.0f;

    [Header("Billboard Camera")]
    public Camera billboardCamera;

    private Coroutine resourceCoroutine;

    private void Awake()
    {
        Billboard[] billboards = GetComponentsInChildren<Billboard>(true);
        foreach (var bb in billboards)
        {
            bb.targetCamera = billboardCamera;
        }
    }

    private void OnMouseDown()
    {
        if (isTamed) return;

        bool enough = false;

        switch (animalType)
        {
            case AnimalType.StoneMushroom:
                if (inventory.stoneCount >= tameCost)
                {
                    inventory.stoneCount -= tameCost;
                    enough = true;
                }
                break;
            case AnimalType.WoodMushroom:
                if (inventory.woodCount >= tameCost)
                {
                    inventory.woodCount -= tameCost;
                    enough = true;
                }
                break;
            case AnimalType.Bober:
                if (inventory.fishCount >= tameCost)
                {
                    inventory.fishCount -= tameCost;
                    enough = true;
                }
                break;
        }

        UpdateResourceTexts();

        if (enough)
        {
            isTamed = true;
            DisableAllColliders();
            onTamed.Invoke();
            StartResourceProduction();
        }
        else
        {
            if (audioSource && ewwClip)
                audioSource.PlayOneShot(ewwClip);
        }
    }

    private void DisableAllColliders()
    {
        foreach (var col in GetComponents<Collider>())
            col.enabled = false;
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    private void StartResourceProduction()
    {
        if (resourceCoroutine != null)
            StopCoroutine(resourceCoroutine);
        resourceCoroutine = StartCoroutine(ResourceProductionRoutine());
    }

    private IEnumerator ResourceProductionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetCurrentInterval());

            switch (animalType)
            {
                case AnimalType.StoneMushroom:
                    inventory.stoneCount += 1;
                    if (stoneIconPrefab && sklad1Transform && sklad1Transform.gameObject.activeInHierarchy)
                        SpawnAndAnimateResourceIcon(stoneIconPrefab);
                    break;
                case AnimalType.WoodMushroom:
                    inventory.woodCount += 1;
                    if (woodIconPrefab && sklad1Transform && sklad1Transform.gameObject.activeInHierarchy)
                        SpawnAndAnimateResourceIcon(woodIconPrefab);
                    break;
                case AnimalType.Bober:
                    inventory.fishCount += 1;
                    if (fishIconPrefab && sklad1Transform && sklad1Transform.gameObject.activeInHierarchy)
                        SpawnAndAnimateResourceIcon(fishIconPrefab);
                    break;
            }
            UpdateResourceTexts();

            if (audioSource && collectClip)
                audioSource.PlayOneShot(collectClip, collectVolume);
        }
    }

    private void SpawnAndAnimateResourceIcon(GameObject iconPrefab)
    {
        GameObject icon = Instantiate(iconPrefab, transform.position, Quaternion.identity, iconParent);

        icon.transform.localScale = Vector3.one * iconScale * 0.7f;

        StartCoroutine(AnimateIconToSklad(icon));
    }

    private IEnumerator AnimateIconToSklad(GameObject icon)
    {
        Vector3 start = icon.transform.position;
        Vector3 end = sklad1Transform.position;

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            Vector3 currentPos = Vector3.Lerp(start, end, progress);
            float height = Mathf.Sin(Mathf.PI * progress) * arcHeight;
            currentPos.y += height;
            icon.transform.position = currentPos;

            if (progress < 0.2f)
                icon.transform.localScale = Vector3.Lerp(Vector3.one * iconScale * 0.7f, Vector3.one * iconScale * 1.2f, progress / 0.2f);
            else
                icon.transform.localScale = Vector3.Lerp(Vector3.one * iconScale * 1.2f, Vector3.one * iconScale, (progress - 0.2f) / 0.8f);

            yield return null;
        }

        float punchTime = 0.15f;
        float punchScale = 1.3f;
        float punchT = 0f;
        Vector3 originalScale = Vector3.one * iconScale;
        while (punchT < punchTime)
        {
            punchT += Time.deltaTime;
            float punchProgress = punchT / punchTime;
            float scale = Mathf.Lerp(1f, punchScale, 1f - Mathf.Pow(1f - punchProgress, 2));
            icon.transform.localScale = originalScale * scale;
            yield return null;
        }
        icon.transform.localScale = originalScale;

        if (sklad1Transform != null)
            StartCoroutine(SkladPunchEffect());

        Destroy(icon);
    }

    private IEnumerator SkladPunchEffect()
    {
        float punchTime = 0.15f;
        float returnTime = 0.15f;
        float punchScaleY = 1.3f;
        Vector3 originalScale = sklad1Transform.localScale;

        float t = 0f;
        while (t < punchTime)
        {
            t += Time.deltaTime;
            float progress = t / punchTime;
            float scaleY = Mathf.Lerp(originalScale.y, originalScale.y * punchScaleY, 1f - Mathf.Pow(1f - progress, 2));
            sklad1Transform.localScale = new Vector3(originalScale.x, scaleY, originalScale.z);
            yield return null;
        }

        t = 0f;
        while (t < returnTime)
        {
            t += Time.deltaTime;
            float progress = t / returnTime;
            float scaleY = Mathf.Lerp(originalScale.y * punchScaleY, originalScale.y, 1f - Mathf.Pow(1f - progress, 2));
            sklad1Transform.localScale = new Vector3(originalScale.x, scaleY, originalScale.z);
            yield return null;
        }

        sklad1Transform.localScale = originalScale;
    }

    public void UpdateResourceTexts()
    {
        if (stoneText != null)
            stoneText.text = inventory.stoneCount.ToString();
        if (woodText != null)
            woodText.text = inventory.woodCount.ToString();
        if (fishText != null)
            fishText.text = inventory.fishCount.ToString();
    }

    public bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public int GetUpgradeCost()
    {
        return 10 * level;
    }

    public float GetCurrentInterval()
    {
        return Mathf.Max(5f - 0.5f * (level - 1), 0.1f);
    }

    public void Upgrade()
    {
        if (level < maxLevel)
        {
            level++;
            if (isTamed)
                StartResourceProduction();
        }
    }
}