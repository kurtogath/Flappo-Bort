using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform container;

    private void Start()
    {
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        var db = FirebaseManager.Instance.databaseRef;

        if (db == null)
        {
            Debug.LogWarning("No database reference found.");
            return;
        }

        db.Child("leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.Result == null)
            {
                Debug.LogError("Failed to load leaderboard from Firebase.");
                return;
            }

            List<ScoreEntry> entries = new List<ScoreEntry>();

            foreach (var child in task.Result.Children)
            {
                string name = child.Child("nombre").Value?.ToString() ?? "Unknown";
                int score = int.TryParse(child.Child("puntos").Value?.ToString(), out int s) ? s : 0;
                entries.Add(new ScoreEntry(name, score));
            }

            // Sort descending by score
            entries.Sort((a, b) => b.score.CompareTo(a.score));

            // Display top 5
            for (int i = 0; i < Mathf.Min(entries.Count, 5); i++)
            {
                var entryObj = Instantiate(entryPrefab, container);
                entryObj.GetComponent<TMPro.TextMeshProUGUI>().text = $"{i + 1}. {entries[i].name} - {entries[i].score}";
            }
        });
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

        
    [System.Serializable]
    public class ScoreEntry
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
