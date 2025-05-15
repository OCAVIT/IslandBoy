using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public ResourceInventory inventory;

    void Start()
    {
        ResourceInventorySaver.Load(inventory);
    }
}