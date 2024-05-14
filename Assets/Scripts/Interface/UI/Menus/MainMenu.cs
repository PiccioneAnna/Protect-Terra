using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Loop", LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Persistents", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Persistents");
    }
}
