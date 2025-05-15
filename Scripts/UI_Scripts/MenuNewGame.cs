using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNewGame : MonoBehaviour
{
    public string gameSceneName = "Game";
    public ResourceInventory inventory;

    public void OnNewGame()
    {
        inventory.SetStone(0);
        inventory.SetWood(0);
        inventory.SetFish(0);
        inventory.ResetSCLADs();

        ResourceInventorySaver.DeleteSave();

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}