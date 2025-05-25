using UnityEngine;
using System.Text;

[System.Serializable]
public class FirebaseConfig
{
    public string database_url;
}

public static class FirebaseConfigLoader
{
    private const string KEY = "Exilor";

    public static FirebaseConfig Load()
    {
        TextAsset encryptedAsset = Resources.Load<TextAsset>("firebase-config");
        if (encryptedAsset == null)
        {
            Debug.LogError("No se encontró firebase-config.txt en Resources.");
            return null;
        }

        try
        {
            string decrypted = Decrypt(encryptedAsset.text, KEY);
            return JsonUtility.FromJson<FirebaseConfig>(decrypted);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al desencriptar firebase-config: " + e.Message);
            return null;
        }
    }

    private static string Decrypt(string encryptedBase64, string key)
    {
        string xorText = Encoding.UTF8.GetString(System.Convert.FromBase64String(encryptedBase64));
        var sb = new StringBuilder();
        for (int i = 0; i < xorText.Length; i++)
        {
            char c = (char)(xorText[i] ^ key[i % key.Length]);
            sb.Append(c);
        }
        return sb.ToString();
    }
}
