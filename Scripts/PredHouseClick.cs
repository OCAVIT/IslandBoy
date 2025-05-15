using UnityEngine;

public class PredHouseClick : MonoBehaviour
{
    public HouseBuildManager manager;
    public int houseIndex;

    void OnMouseDown()
    {
        if (houseIndex == 1) manager.OnPredHouse1Clicked();
        if (houseIndex == 2) manager.OnPredHouse2Clicked();
        if (houseIndex == 3) manager.OnPredHouse3Clicked();
    }
}