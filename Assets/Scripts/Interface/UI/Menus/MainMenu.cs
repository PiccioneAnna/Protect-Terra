using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Visuals;

public class MainMenu : MonoBehaviour
{
    public ScreenTint screenTint;

    public void Start()
    {
        DontDestroyOnLoad(gameObject.transform.parent);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void StartNewGame()
    {
        screenTint.Tint();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene", LoadSceneMode.Single);
        screenTint.UnTint();

        Destroy(gameObject.transform.parent.gameObject);
    }

    public void ReturnToMainMenu()
    {
        screenTint.Tint();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Persistents");
        screenTint.UnTint();

        Destroy(gameObject.transform.parent.gameObject);
    }
}
