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
        Debug.Log("Game Over screen activated");
        Debug.Log("Game Over");

        string nombre = PlayerPrefs.GetString("PlayerName", "Invitado");
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.databaseRef != null)
        {
            FirebaseManager.Instance.SaveScore(nombre, playerScore);
        }
        else
        {
            Debug.LogWarning("FirebaseManager no está listo. Score no guardado.");
        }

    }

}
