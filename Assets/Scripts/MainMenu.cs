using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void loadScoreboard()
    {
        SceneManager.LoadScene(2);
    }

    public void loadConfig()
    {
        SceneManager.LoadScene(3);
    }

    public void quit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); 
    }

}
