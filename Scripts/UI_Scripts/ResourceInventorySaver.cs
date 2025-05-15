using UnityEngine;
using System.IO;

public static class ResourceInventorySaver
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, "resource_inventory.json");

    [System.Serializable]
    private class ResourceInventoryData
    {
        public int stoneCount;
        public int woodCount;
        public int fishCount;
        public int resourceLimit;
        public bool isSCLAD1Active;
        public bool isSCLAD2Active;
    }

    public static void Save(ResourceInventory inventory)
    {
        var data = new ResourceInventoryData
        {
            stoneCount = inventory.stoneCount,
            woodCount = inventory.woodCount,
            fishCount = inventory.fishCount,
            resourceLimit = inventory.ResourceLimit,
            isSCLAD1Active = inventory.IsSCLAD1Active,
            isSCLAD2Active = inventory.IsSCLAD2Active
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, json);
    }

    public static void Load(ResourceInventory inventory)
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);
        var data = JsonUtility.FromJson<ResourceInventoryData>(json);

        inventory.SetStone(data.stoneCount);
        inventory.SetWood(data.woodCount);
        inventory.SetFish(data.fishCount);

        inventory.ResetSCLADs();
        if (data.isSCLAD1Active)
            inventory.ActivateSCLAD1();
        if (data.isSCLAD2Active)
            inventory.ActivateSCLAD2();
    }

    public static void DeleteSave()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}