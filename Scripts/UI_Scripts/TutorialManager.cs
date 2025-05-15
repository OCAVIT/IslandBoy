using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialPanel;
    public GameObject Page1;
    public GameObject Page2;
    public GameObject NextButton;
    public GameObject CloseButton;

    void Start()
    {
        TutorialPanel.SetActive(false);
        Page1.SetActive(false);
        Page2.SetActive(false);
        NextButton.SetActive(false);
        CloseButton.SetActive(false);

        NextButton.GetComponent<Button>().onClick.AddListener(OnNextClicked);
        CloseButton.GetComponent<Button>().onClick.AddListener(OnCloseClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenTutorial();
        }
    }

    void OpenTutorial()
    {
        TutorialPanel.SetActive(true);
        Page1.SetActive(true);
        Page2.SetActive(false);
        NextButton.SetActive(true);
        CloseButton.SetActive(false);
    }

    void OnNextClicked()
    {
        Page1.SetActive(false);
        NextButton.SetActive(false);
        Page2.SetActive(true);
        CloseButton.SetActive(true);
    }

    void OnCloseClicked()
    {
        TutorialPanel.SetActive(false);
    }
}