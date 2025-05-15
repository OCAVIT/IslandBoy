using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IslandBuyManager : MonoBehaviour
{
    public ResourceInventory inventory;

    public TMP_Text stoneCostText;
    public TMP_Text woodCostText;
    public TMP_Text numOfIslText;
    public GameObject finishText;

    public TMP_Text stoneAmountText;
    public TMP_Text woodAmountText;

    public GameObject island2;
    public GameObject island3;

    public GameObject panelRes;
    public Button buyIslandButton;

    public Color normalColor = Color.white;
    public Color errorColor = Color.red;
    public float colorLerpTime = 0.2f;

    private int currentIsland = 1;
    private bool isFlashing = false;

    private void Start()
    {
        SetCostTexts(150, 150);
        UpdateResourceTexts();
        numOfIslText.text = "У вас 1 остров из 3";
        finishText.SetActive(false);
        island2.SetActive(false);
        island3.SetActive(false);

        buyIslandButton.onClick.AddListener(OnBuyIsland);
    }

    private void SetCostTexts(int stoneCost, int woodCost)
    {
        stoneCostText.text = stoneCost.ToString();
        woodCostText.text = woodCost.ToString();
    }

    private void UpdateResourceTexts()
    {
        stoneAmountText.text = inventory.stoneCount.ToString();
        woodAmountText.text = inventory.woodCount.ToString();
    }

    private void OnBuyIsland()
    {
        if (currentIsland == 1)
        {
            if (inventory.stoneCount >= 150 && inventory.woodCount >= 150)
            {
                inventory.stoneCount -= 150;
                inventory.woodCount -= 150;
                UpdateResourceTexts();

                island2.SetActive(true);
                currentIsland = 2;
                numOfIslText.text = "У вас 2 острова из 3";
                SetCostTexts(300, 300);
            }
            else
            {
                if (!isFlashing)
                    StartCoroutine(FlashButton());
            }
        }
        else if (currentIsland == 2)
        {
            if (inventory.stoneCount >= 300 && inventory.woodCount >= 300)
            {
                inventory.stoneCount -= 300;
                inventory.woodCount -= 300;
                UpdateResourceTexts();

                island3.SetActive(true);
                currentIsland = 3;

                panelRes.SetActive(false);
                numOfIslText.gameObject.SetActive(false);
                buyIslandButton.gameObject.SetActive(false);
                finishText.SetActive(true);
            }
            else
            {
                if (!isFlashing)
                    StartCoroutine(FlashButton());
            }
        }
    }

    private IEnumerator FlashButton()
    {
        isFlashing = true;
        Image btnImage = buyIslandButton.GetComponent<Image>();
        Color startColor = btnImage.color;
        float t = 0f;

        while (t < colorLerpTime)
        {
            btnImage.color = Color.Lerp(startColor, errorColor, t / colorLerpTime);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        btnImage.color = errorColor;

        yield return new WaitForSecondsRealtime(1f);

        t = 0f;
        while (t < colorLerpTime)
        {
            btnImage.color = Color.Lerp(errorColor, normalColor, t / colorLerpTime);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        btnImage.color = normalColor;
        isFlashing = false;
    }
}