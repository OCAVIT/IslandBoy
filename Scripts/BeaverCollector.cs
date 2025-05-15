using UnityEngine;

public class BeaverCollector : MonoBehaviour
{
    public ResourceInventory playerInventory;
    private int localFish = 0;
    private float timer = 0f;
    public float collectInterval = 10f;

    void Update()
    {
        if (!isActiveAndEnabled) return;

        timer += Time.deltaTime;
        if (timer >= collectInterval)
        {
            timer -= collectInterval;
            localFish += 1;

            if (localFish >= 5)
            {
                playerInventory.AddFish(localFish);
                localFish = 0;
            }
        }
    }
}