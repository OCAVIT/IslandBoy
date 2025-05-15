using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AnimalUpgradeButton : MonoBehaviour
{
    public AnimalType animalType;
    public ResourceInventory inventory;

    public Button upgradeButton;
    public TMP_Text costText;
    public TMP_Text strengthText;
    public TMP_Text sledText;

    public TMP_Text stoneText;
    public TMP_Text woodText;
    public TMP_Text fishText;

    public Color normalColor = Color.white;
    public Color notEnoughColor = Color.red;

    private bool isFlashing = false;

    private List<AnimalTame> animalsOfType = new List<AnimalTame>();

    void Start()
    {
        RefreshAnimalsOfType();
        UpdateUI();
        upgradeButton.onClick.AddListener(OnUpgradeClick);
        UpdateResourceUI();
    }

    void RefreshAnimalsOfType()
    {
        animalsOfType.Clear();
        foreach (var animal in FindObjectsByType<AnimalTame>(FindObjectsSortMode.None))
        {
            if (animal.animalType == animalType)
                animalsOfType.Add(animal);
        }
    }

    int GetCurrentLevel()
    {
        if (animalsOfType.Count == 0) return 1;
        int minLevel = animalsOfType[0].level;
        foreach (var animal in animalsOfType)
            if (animal.level < minLevel) minLevel = animal.level;
        return minLevel;
    }

    int GetMaxLevel()
    {
        if (animalsOfType.Count == 0) return 5;
        return animalsOfType[0].maxLevel;
    }

    int GetUpgradeCost()
    {
        return 10 * GetCurrentLevel();
    }

    bool CanUpgrade()
    {
        return GetCurrentLevel() < GetMaxLevel();
    }

    string GetResourceName()
    {
        switch (animalType)
        {
            case AnimalType.StoneMushroom:
                return "Камня";
            case AnimalType.WoodMushroom:
                return "Рыбы";
            case AnimalType.Bober:
                return "Дерева";
            default:
                return "";
        }
    }

    void UpdateUI()
    {
        int level = GetCurrentLevel();
        int maxLevel = GetMaxLevel();

        if (CanUpgrade())
            costText.text = $"{GetUpgradeCost()} {GetResourceName()}";
        else
            costText.text = "";

        strengthText.text = $"Сила {level}";

        if (level < maxLevel)
            sledText.text = $"След. ур - Сила {level + 1}";
        else
            sledText.text = "Max";

        bool atMax = level >= maxLevel;
        upgradeButton.interactable = !atMax && animalsOfType.Count > 0;
        costText.gameObject.SetActive(!atMax);
        sledText.gameObject.SetActive(true);

        UpdateResourceUI();
    }

    void UpdateResourceUI()
    {
        if (stoneText != null)
            stoneText.text = inventory.stoneCount.ToString();
        if (woodText != null)
            woodText.text = inventory.woodCount.ToString();
        if (fishText != null)
            fishText.text = inventory.fishCount.ToString();
    }

    public void OnUpgradeClick()
    {
        RefreshAnimalsOfType();

        if (animalsOfType.Count == 0)
            return;

        if (!CanUpgrade())
            return;

        int cost = GetUpgradeCost();
        bool enough = false;

        switch (animalType)
        {
            case AnimalType.StoneMushroom:
                if (inventory.stoneCount >= cost)
                {
                    inventory.stoneCount -= cost;
                    enough = true;
                }
                break;
            case AnimalType.WoodMushroom:
                if (inventory.fishCount >= cost)
                {
                    inventory.fishCount -= cost;
                    enough = true;
                }
                break;
            case AnimalType.Bober:
                if (inventory.woodCount >= cost)
                {
                    inventory.woodCount -= cost;
                    enough = true;
                }
                break;
        }

        UpdateResourceUI();

        if (enough)
        {
            foreach (var animal in animalsOfType)
            {
                animal.Upgrade();
            }
            RefreshAnimalsOfType();
            UpdateUI();
            foreach (var animal in animalsOfType)
                animal.UpdateResourceTexts();
        }
        else
        {
            if (!isFlashing)
                StartCoroutine(FlashButton());
            UpdateUI();
        }
    }

    IEnumerator FlashButton()
    {
        isFlashing = true;
        var colors = upgradeButton.colors;
        Color originalNormal = colors.normalColor;
        Color originalHighlighted = colors.highlightedColor;
        Color originalPressed = colors.pressedColor;
        Color originalSelected = colors.selectedColor;

        colors.normalColor = notEnoughColor;
        colors.highlightedColor = notEnoughColor;
        colors.pressedColor = notEnoughColor;
        colors.selectedColor = notEnoughColor;
        upgradeButton.colors = colors;

        yield return new WaitForSeconds(1f);

        colors.normalColor = originalNormal;
        colors.highlightedColor = originalHighlighted;
        colors.pressedColor = originalPressed;
        colors.selectedColor = originalSelected;
        upgradeButton.colors = colors;
        isFlashing = false;
    }

    public void Refresh()
    {
        RefreshAnimalsOfType();
        UpdateUI();
    }
}