using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Manager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void LoadGameover()
    {
        SceneManager.LoadSceneAsync("GameOver");
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("GamePlay");
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Stop!");
    }
}
