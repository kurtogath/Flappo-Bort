using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

[System.Serializable]
public class FirebaseConfig
{
    public string database_url;
}

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

            string configPath = Path.Combine(Application.streamingAssetsPath, "google-services-desktop.json");

            if (!File.Exists(configPath))
            {
                Debug.LogError($"Configuration file not found at: {configPath}");
                return;
            }

            string json = File.ReadAllText(configPath);
            FirebaseConfig config = JsonUtility.FromJson<FirebaseConfig>(json);

            if (string.IsNullOrEmpty(config.database_url))
            {
                Debug.LogError("Missing or empty 'database_url' in google-services-desktop.json");
                return;
            }

            databaseRef = FirebaseDatabase.GetInstance(config.database_url).RootReference;
            Debug.Log($"Firebase initialized with URL: {config.database_url}");
        });
    }

    public void SaveScore(string playerName, int score)
    {
        if (databaseRef == null)
        {
            Debug.LogWarning("Tried to save score without Firebase connection.");
            return;
        }

        string id = databaseRef.Push().Key;
        databaseRef.Child("leaderboard").Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(new ScoreEntry(playerName, score)));
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
