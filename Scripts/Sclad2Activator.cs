using UnityEngine;
using TMPro;

public class Sclad2Activator : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory resourceInventory;

    [Header("UI")]
    public TMP_Text sclad2Text;
    public TMP_Text stoneText;
    public TMP_Text woodText;

    [Header("GameObjects")]
    public GameObject scladGolo2;
    public GameObject SCLAD2;

    [Header("Sclad1 Reference")]
    public GameObject SCLAD1;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buildClip;

    private bool canActivate = false;

    void Start()
    {
        UpdateResourceTexts();
        UpdateSclad2TextColor();
        UpdateSclad2Visibility();
    }

    void Update()
    {
        UpdateSclad2Visibility();
    }

    void UpdateSclad2Visibility()
    {
        if (resourceInventory.IsSCLAD1Active)
        {
            if (!scladGolo2.activeSelf) scladGolo2.SetActive(true);
            if (!sclad2Text.gameObject.activeSelf) sclad2Text.gameObject.SetActive(true);
            UpdateSclad2TextColor();
        }
        else
        {
            if (scladGolo2.activeSelf) scladGolo2.SetActive(false);
            if (sclad2Text.gameObject.activeSelf) sclad2Text.gameObject.SetActive(false);
        }
    }

    void UpdateSclad2TextColor()
    {
        if (resourceInventory.stoneCount >= 150 && resourceInventory.woodCount >= 150)
        {
            sclad2Text.color = Color.green;
            canActivate = true;
        }
        else
        {
            sclad2Text.color = Color.white;
            canActivate = false;
        }
    }

    void UpdateResourceTexts()
    {
        stoneText.text = resourceInventory.stoneCount.ToString();
        woodText.text = resourceInventory.woodCount.ToString();
    }

    void OnMouseDown()
    {
        if (!canActivate || !resourceInventory.IsSCLAD1Active) return;

        resourceInventory.stoneCount -= 150;
        resourceInventory.woodCount -= 150;

        UpdateResourceTexts();

        resourceInventory.ActivateSCLAD2();

        scladGolo2.SetActive(false);
        SCLAD2.SetActive(true);

        sclad2Text.gameObject.SetActive(false);

        if (audioSource != null && buildClip != null)
        {
            audioSource.PlayOneShot(buildClip);
        }
    }
}