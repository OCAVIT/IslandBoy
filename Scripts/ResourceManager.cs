using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory inventory;

    [Header("Resource Objects")]
    public GameObject[] Stones;
    public GameObject[] Woods;

    [Header("UI Manager")]
    public ResourceUIManager uiManager;

    [Header("Sounds")]
    public AudioClip StoneHitSound;
    public AudioClip StonePickUpSound;
    public AudioClip WoodHitSound;
    public AudioClip WoodPickUpSound;
    public AudioClip FishHitSound;
    public AudioClip FishGotSound;
    public AudioSource audioSource;

    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float soundVolume = 1f;

    [Header("Shake Settings")]
    public float shakeStrength = 0.1f;
    public float shakeDuration = 0.1f;

    [Header("Particles")]
    public GameObject stoneHitParticlesPrefab;
    public GameObject woodHitParticlesPrefab;
    public Color goldColor = new Color(1f, 0.84f, 0f, 1f);

    void Start()
    {
        foreach (var stone in Stones)
        {
            AddResourceHandler(stone, ResourceType.Stone);
        }
        foreach (var wood in Woods)
        {
            AddResourceHandler(wood, ResourceType.Wood);
        }

        GameObject water = GameObject.FindGameObjectWithTag("Water");
        if (water != null)
        {
            AddResourceHandler(water, ResourceType.Fish);
        }

        if (uiManager) uiManager.UpdateUI();
    }

    void AddResourceHandler(GameObject obj, ResourceType type)
    {
        if (obj == null) return;
        var handler = obj.GetComponent<ResourceClickHandler>();
        if (handler == null)
            handler = obj.AddComponent<ResourceClickHandler>();
        handler.Init(OnResourceClicked, type, shakeStrength, shakeDuration, stoneHitParticlesPrefab, woodHitParticlesPrefab, goldColor);
    }

    void OnResourceClicked(GameObject obj, ResourceType type, int hitCount)
    {
        if (type == ResourceType.Stone)
        {
            if (hitCount < 10)
            {
                audioSource.PlayOneShot(StoneHitSound, soundVolume);
            }
            else
            {
                inventory.AddStone();
                audioSource.PlayOneShot(StonePickUpSound, soundVolume);
                obj.GetComponent<ResourceHitCounter>().ResetHits();
                if (uiManager) uiManager.UpdateUI();
            }
        }
        else if (type == ResourceType.Wood)
        {
            if (hitCount < 10)
            {
                audioSource.PlayOneShot(WoodHitSound, soundVolume);
            }
            else
            {
                inventory.AddWood();
                audioSource.PlayOneShot(WoodPickUpSound, soundVolume);
                obj.GetComponent<ResourceHitCounter>().ResetHits();
                if (uiManager) uiManager.UpdateUI();
            }
        }
        else if (type == ResourceType.Fish)
        {
            if (hitCount < 10)
            {
                audioSource.PlayOneShot(FishHitSound, soundVolume);
            }
            else
            {
                inventory.AddFish();
                audioSource.PlayOneShot(FishGotSound, soundVolume);
                obj.GetComponent<ResourceHitCounter>().ResetHits();
                if (uiManager) uiManager.UpdateUI();
            }
        }
    }
}