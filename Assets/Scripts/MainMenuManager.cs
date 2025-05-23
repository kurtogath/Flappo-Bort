using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text playerNameText;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Invitado");
        playerNameText.text = $"Jugando como: {playerName}";
    }
}
