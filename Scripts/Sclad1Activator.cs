using UnityEngine;
using TMPro;

public class Sclad1Activator : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory resourceInventory;

    [Header("UI")]
    public TMP_Text sclad1Text;
    public TMP_Text stoneText;
    public TMP_Text woodText;

    [Header("GameObjects")]
    public GameObject scladGolo1;
    public GameObject SCLAD1;
    public GameObject Sclad2Golo;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buildClip;

    private bool canActivate = false;

    void Start()
    {
        UpdateResourceTexts();
        UpdateSclad1TextColor();
    }

    void Update()
    {
        UpdateSclad1TextColor();
    }

    void UpdateSclad1TextColor()
    {
        if (resourceInventory.stoneCount >= 50 && resourceInventory.woodCount >= 50)
        {
            sclad1Text.color = Color.green;
            canActivate = true;
        }
        else
        {
            sclad1Text.color = Color.white;
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
        if (!canActivate) return;

        resourceInventory.stoneCount -= 50;
        resourceInventory.woodCount -= 50;

        UpdateResourceTexts();

        resourceInventory.ActivateSCLAD1();

        scladGolo1.SetActive(false);
        SCLAD1.SetActive(true);

        sclad1Text.gameObject.SetActive(false);

        if (audioSource != null && buildClip != null)
        {
            audioSource.PlayOneShot(buildClip);
        }

        if (Sclad2Golo != null)
            Sclad2Golo.SetActive(true);
    }
}