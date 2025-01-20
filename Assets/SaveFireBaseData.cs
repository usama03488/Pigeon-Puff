using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class SaveFireBaseData : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private FirebaseDatabase instance;
    void Start()
    {
        // Initialize the Firebase Database referenceFirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
      //  Invoke(nameof(SetRef), 2f);
        //instance = FirebaseDatabase.DefaultInstance;
        //databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void SetRef()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized successfully.");
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }

    // Call this method on your button click
    public void SignUpUser(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username cannot be empty!");
            return;
        }

        // Create a unique ID for the player
        string userId = System.Guid.NewGuid().ToString();

        // Create a new user object
        PlayerData newPlayer = new PlayerData
        {
            Username = username,
            IsConnected = true
        };
        SetRef();
        // Convert the user object to JSON and push to Firebase
        string json = JsonUtility.ToJson(newPlayer);
        if (databaseReference != null)
        {
            databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log($"User {username} successfully signed up with ID: {userId}");
                }
                else
                {
                    Debug.LogError($"Error signing up user: {task.Exception}");
                }
            });
        }
        else
        {
            Debug.Log("Reffrence is null");
        }
      
    }

    // Define a class to structure player data
    [System.Serializable]
    public class PlayerData
    {
        public string Username;
        public bool IsConnected;
    }
}
