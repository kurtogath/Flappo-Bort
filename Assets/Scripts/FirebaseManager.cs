using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public DatabaseReference dbRef;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.GetInstance("https://flappo-bort-default-rtdb.europe-west1.firebasedatabase.app").RootReference;
                Debug.Log("Firebase inicializado correctamente");
            }
            else
            {
                Debug.LogError("Error inicializando Firebase: " + dependencyStatus);
            }
        });
    }

    public void GuardarPuntaje(string nombre, int score)
    {
        string id = dbRef.Push().Key;
        dbRef.Child("leaderboard").Child(id).Child("nombre").SetValueAsync(nombre);
        dbRef.Child("leaderboard").Child(id).Child("puntos").SetValueAsync(score);
    }
}
