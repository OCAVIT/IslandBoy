using UnityEngine;

[CreateAssetMenu(fileName = "ResourceInventory", menuName = "Game/ResourceInventory")]
public class ResourceInventory : ScriptableObject
{
    public int stoneCount = 0;
    public int woodCount = 0;
    public int fishCount = 0;

    private int resourceLimit = 50;
    private bool isSCLAD1Active = false;
    private bool isSCLAD2Active = false;

    public int ResourceLimit => resourceLimit;
    public bool IsSCLAD1Active => isSCLAD1Active;
    public bool IsSCLAD2Active => isSCLAD2Active;

    public void ActivateSCLAD1()
    {
        isSCLAD1Active = true;
        resourceLimit = 150;
    }

    public void ActivateSCLAD2()
    {
        if (isSCLAD1Active)
        {
            isSCLAD2Active = true;
            resourceLimit = 300;
        }
    }

    public void AddStone(int amount = 1)
    {
        stoneCount = Mathf.Min(stoneCount + amount, resourceLimit);
    }

    public void AddWood(int amount = 1)
    {
        woodCount = Mathf.Min(woodCount + amount, resourceLimit);
    }

    public void AddFish(int amount = 1)
    {
        fishCount = Mathf.Min(fishCount + amount, resourceLimit);
    }

    public void SetStone(int value)
    {
        stoneCount = Mathf.Clamp(value, 0, resourceLimit);
    }

    public void SetWood(int value)
    {
        woodCount = Mathf.Clamp(value, 0, resourceLimit);
    }

    public void SetFish(int value)
    {
        fishCount = Mathf.Clamp(value, 0, resourceLimit);
    }

    public void ResetSCLADs()
    {
        isSCLAD1Active = false;
        isSCLAD2Active = false;
        resourceLimit = 50;
    }
}