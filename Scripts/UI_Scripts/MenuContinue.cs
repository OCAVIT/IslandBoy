using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuContinue : MonoBehaviour
{
    public string gameSceneName = "Game";

    public void OnContinue()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}