using UnityEngine;

public class WoodcutterMushroom : MonoBehaviour
{
    public ResourceInventory playerInventory;
    private int localWood = 0;
    private float timer = 0f;
    public float collectInterval = 10f;

    void Update()
    {
        if (!isActiveAndEnabled) return;

        timer += Time.deltaTime;
        if (timer >= collectInterval)
        {
            timer -= collectInterval;
            localWood += 1;

            if (localWood >= 5)
            {
                playerInventory.AddWood(localWood);
                localWood = 0;
            }
        }
    }
}