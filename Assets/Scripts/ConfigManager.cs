using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ConfigManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Text currentNameText;

    void Start()
    {
        string nombreGuardado = PlayerPrefs.GetString("PlayerName", "Invitado");

        if (nameInput != null)
            nameInput.text = nombreGuardado;

    }

    public void GuardarNombre()
    {
        string nuevoNombre = nameInput.text;
        if (string.IsNullOrWhiteSpace(nuevoNombre))
        {
            nuevoNombre = "Invitado";
        }

        PlayerPrefs.SetString("PlayerName", nuevoNombre);
        PlayerPrefs.Save();

        Debug.Log("Nombre guardado: " + nuevoNombre);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetBirdSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("BirdSkin", skinIndex);
        PlayerPrefs.Save();
        Debug.Log("Skin guardada: " + skinIndex);
    }

}
