using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject BuyPanelUI;
    public GameObject PageISL;
    public GameObject RabsPanel;

    [Header("Buttons")]
    public Button IslandsButton;
    public Button RabsButton;

    private bool isBuyPanelActive = false;

    void Start()
    {
        IslandsButton.onClick.AddListener(OnIslandsButtonClick);
        RabsButton.onClick.AddListener(OnRabsButtonClick);

        BuyPanelUI.SetActive(false);
        PageISL.SetActive(false);
        RabsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isBuyPanelActive = !isBuyPanelActive;
            BuyPanelUI.SetActive(isBuyPanelActive);

            if (isBuyPanelActive)
            {
                PageISL.SetActive(true);
            }
            else
            {
                PageISL.SetActive(false);
                RabsPanel.SetActive(false);
            }
        }
    }

    public void OnIslandsButtonClick()
    {
        PageISL.SetActive(true);
        RabsPanel.SetActive(false);
    }

    public void OnRabsButtonClick()
    {
        RabsPanel.SetActive(true);
        PageISL.SetActive(false);
    }
}