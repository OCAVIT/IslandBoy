using UnityEngine;
using TMPro;

public class ResourceUIManager : MonoBehaviour
{
    [Header("Resource Inventory")]
    public ResourceInventory inventory;

    [Header("Resource Texts")]
    public TMP_Text StoneGot;
    public TMP_Text WoodGot;
    public TMP_Text FishGot;

    public void UpdateUI()
    {
        if (StoneGot) StoneGot.text = inventory.stoneCount.ToString();
        if (WoodGot) WoodGot.text = inventory.woodCount.ToString();
        if (FishGot) FishGot.text = inventory.fishCount.ToString();
    }
}