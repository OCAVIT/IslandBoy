using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ContinueButtonController : MonoBehaviour
{
    public Button continueButton;

    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "resource_inventory.json");
        if (!File.Exists(path))
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }
}