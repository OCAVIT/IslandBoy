using UnityEngine;

public class ResourceHitCounter : MonoBehaviour
{
    private int hitCount = 0;

    public int AddHit()
    {
        hitCount++;
        return hitCount;
    }

    public void ResetHits()
    {
        hitCount = 0;
    }
}