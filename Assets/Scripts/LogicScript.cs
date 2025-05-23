using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    public ConfigManager configManager;

    
    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(1);
        gameOverScreen.SetActive(false);
    }

    public void gotoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void gameOver()
    {

        gameOverScreen.SetActive(true);

        string nombre = PlayerPrefs.GetString("PlayerName", "Invitado");
        FirebaseManager.Instance.GuardarPuntaje(nombre, playerScore);
    }
}
