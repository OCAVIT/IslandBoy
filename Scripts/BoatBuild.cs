using UnityEngine;
using TMPro;

public class BoatBuilder : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory resourceInventory;

    [Header("UI")]
    public TMP_Text Boattxt;

    [Header("Boat Objects")]
    public GameObject BrokedBoat;
    public GameObject GoodBoat;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buildClip;

    [Header("Build Requirements")]
    public int requiredStone = 15;
    public int requiredWood = 30;

    private bool canBuild = false;
    private bool built = false;

    void Update()
    {
        if (!built && resourceInventory.stoneCount >= requiredStone && resourceInventory.woodCount >= requiredWood)
        {
            if (!canBuild)
            {
                canBuild = true;
                Boattxt.color = Color.green;
            }
        }
        else
        {
            if (canBuild)
            {
                canBuild = false;
                Boattxt.color = Color.white;
            }
        }
    }

    void OnMouseDown()
    {
        if (canBuild && !built)
        {
            BuildBoat();
        }
    }

    void BuildBoat()
    {
        built = true;
        resourceInventory.stoneCount -= requiredStone;
        resourceInventory.woodCount -= requiredWood;

        BrokedBoat.SetActive(false);
        GoodBoat.SetActive(true);

        if (audioSource != null && buildClip != null)
            audioSource.PlayOneShot(buildClip);

        if (Boattxt != null)
            Boattxt.gameObject.SetActive(false);
    }
}