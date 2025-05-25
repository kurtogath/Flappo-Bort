using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.STP;


public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public DatabaseReference databaseRef;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError($"Firebase dependency error: {task.Result}");
                return;
            }

            //Auth
            FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWithOnMainThread(authTask =>
            {
                if (authTask.IsFaulted || authTask.IsCanceled)
                {
                    Debug.LogError("Error en autenticación anónima.");
                    return;
                }

                Debug.Log("Usuario autenticado anónimamente.");

                // Load config
                FirebaseConfig config = FirebaseConfigLoader.Load();

                if (config == null || string.IsNullOrEmpty(config.database_url))
                {
                    Debug.LogError("Missing or empty 'database_url' in firebase-config.txt");
                    return;
                }

                databaseRef = FirebaseDatabase.GetInstance(config.database_url).RootReference;
                Debug.Log($"Firebase initialized with URL: {config.database_url}");
            });
        });


    }

    public void SaveScore(string playerName, int score)
    {
        Debug.Log("SaveScore");
        if (databaseRef == null)
        {
            Debug.LogWarning("Tried to save score without Firebase connection.");
            return;
        }

        try
        {
            string id = databaseRef.Push().Key;
            Debug.Log("Vamo a ver...");
            databaseRef.Child("leaderboard").Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(new ScoreEntry(playerName, score)));
            Debug.Log("Guardado?");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar score en Firebase: " + ex.Message);
        }
    }


    [System.Serializable]
    private class ScoreEntry
    {
        public string name;
        public int score;

        public ScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}
