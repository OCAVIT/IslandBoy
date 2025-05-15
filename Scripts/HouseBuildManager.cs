using UnityEngine;
using TMPro;

public class HouseBuildManager : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory resourceInventory;

    [Header("GameObjects")]
    public GameObject clickObject;
    public GameObject predHouse1;
    public GameObject house1;
    public GameObject predHouse2;
    public GameObject house2;
    public GameObject predHouse3;
    public GameObject house3;

    [Header("Text")]
    public TMP_Text predText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buildClip;

    public ResourceUIManager resourceUIManager;

    private bool clickActivated = false;
    private bool house1Built = false;
    private bool house2Built = false;
    private bool house3Built = false;

    void Start()
    {
        clickObject.SetActive(false);
        predHouse1.SetActive(true);
        house1.SetActive(false);
        predHouse2.SetActive(false);
        house2.SetActive(false);
        predHouse3.SetActive(false);
        house3.SetActive(false);

        predText.gameObject.SetActive(true);
        predText.text = "50 дерева";
        predText.color = Color.white;
    }

    void Update()
    {
        int wood = resourceInventory.woodCount;
        int stone = resourceInventory.stoneCount;

        if (!clickActivated && wood >= 50)
        {
            clickObject.SetActive(true);
            predText.color = Color.green;
            predHouse1.SetActive(true);
            clickActivated = true;
        }

        if (house1Built && !house2Built && !predHouse2.activeSelf && wood >= 80 && stone >= 80)
        {
            predHouse2.SetActive(true);
        }

        if (house2Built && !house3Built && !predHouse3.activeSelf && wood >= 120 && stone >= 120)
        {
            predHouse3.SetActive(true);
        }

        if (!house1Built)
        {
            predText.text = "50 дерева";
            predText.color = (wood >= 50) ? Color.green : Color.white;
        }
        else if (!house2Built)
        {
            predText.text = "80 Дерева\n80 Камня";
            predText.color = (wood >= 80 && stone >= 80) ? Color.green : Color.white;
        }
        else if (!house3Built)
        {
            predText.text = "120 Дерева\n120 Камня";
            predText.color = (wood >= 120 && stone >= 120) ? Color.green : Color.white;
        }
    }

    public void OnPredHouse1Clicked()
    {
        if (resourceInventory.woodCount >= 50 && !house1Built)
        {
            resourceInventory.woodCount -= 50;
            resourceUIManager.UpdateUI();
            predHouse1.SetActive(false);
            house1.SetActive(true);
            clickObject.SetActive(false);
            PlayBuildSound();

            house1Built = true;

            predText.color = Color.white;
            predText.text = "80 Дерева\n80 Камня";
        }
    }

    public void OnPredHouse2Clicked()
    {
        if (resourceInventory.woodCount >= 80 && resourceInventory.stoneCount >= 80 && house1Built && !house2Built)
        {
            resourceInventory.woodCount -= 80;
            resourceInventory.stoneCount -= 80;
            resourceUIManager.UpdateUI();
            predHouse2.SetActive(false);
            house1.SetActive(false);
            house2.SetActive(true);
            PlayBuildSound();

            house2Built = true;

            predText.color = Color.white;
            predText.text = "120 Дерева\n120 Камня";
        }
    }

    public void OnPredHouse3Clicked()
    {
        if (resourceInventory.woodCount >= 120 && resourceInventory.stoneCount >= 120 && house2Built && !house3Built)
        {
            resourceInventory.woodCount -= 120;
            resourceInventory.stoneCount -= 120;
            resourceUIManager.UpdateUI();
            predHouse3.SetActive(false);
            house3.SetActive(true);
            PlayBuildSound();

            house3Built = true;

            predText.gameObject.SetActive(false);
        }
    }

    void PlayBuildSound()
    {
        if (audioSource && buildClip)
            audioSource.PlayOneShot(buildClip);
    }
}