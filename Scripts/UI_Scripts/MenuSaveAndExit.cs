using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSaveAndExit : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public ResourceInventory inventory;

    public void OnSaveAndExit()
    {
        ResourceInventorySaver.Save(inventory);

        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}