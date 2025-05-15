using UnityEngine;

public class StonebreakerMushroom : MonoBehaviour
{
    public ResourceInventory playerInventory;
    private int localStone = 0;
    private float timer = 0f;
    public float collectInterval = 12f;

    void Update()
    {
        if (!isActiveAndEnabled) return;

        timer += Time.deltaTime;
        if (timer >= collectInterval)
        {
            timer -= collectInterval;
            localStone += 1;

            if (localStone >= 5)
            {
                playerInventory.AddStone(localStone);
                localStone = 0;
            }
        }
    }
}