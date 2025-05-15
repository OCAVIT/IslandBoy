using UnityEngine;

public class AnimalTameManager : MonoBehaviour
{
    public AnimalTame[] animals;
    public ResourceInventory inventory;

    private void Awake()
    {
        foreach (var animal in animals)
        {
            animal.inventory = inventory;
        }
    }
}