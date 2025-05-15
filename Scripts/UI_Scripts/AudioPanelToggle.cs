using UnityEngine;

public class AudioPanelToggle : MonoBehaviour
{
    public GameObject audioPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (audioPanel != null)
            {
                audioPanel.SetActive(!audioPanel.activeSelf);
            }
        }
    }
}