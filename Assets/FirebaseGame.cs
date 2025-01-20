using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;

public class FirebaseGame : MonoBehaviour
{
    public static FirebaseGame instance;
    DatabaseReference reference;
    public FirebaseAuth Auth;
    public FirebaseUser User;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void AddScore(string username, int score)
    {
        string key = reference.Child("leaderboard").Push().Key;
       PlayerData entry = new PlayerData(username, score);
        string json = JsonUtility.ToJson(entry);

        reference.Child("leaderboard").Child(key).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Score added successfully.");
            }
            else
            {
                Debug.LogError("Failed to add score: " + task.Exception);
            }
        });
    }
    public void upload_Score(int score)
    {

       

        if (User != null)
        {
            string userId = User.UserId;

            // Update the score in the database
            //  DatabaseReference userScoreRef = reference.Child("users").Child(User.UserId).Child("Score").SetValueAsync(score);
            reference.Child("users").Child(userId).Child("Score").SetValueAsync(score)
                 .ContinueWith(task =>
                 {
                     if (task.IsFaulted)
                     {
                         Debug.LogError($"Failed to update score: {task.Exception}");
                     }
                     else if (task.IsCompleted)
                     {
                         Debug.Log("Score updated successfully!");
                     }
                 });
        }
      
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public int score;

        public PlayerData(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }
}
