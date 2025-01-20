using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
public class Firebaseinitializerr : MonoBehaviour
{
    private bool isFirebaseInitialized = false;

    void Start()
    {
       // InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        // Check and fix Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase is ready to use
                Debug.Log("Firebase is ready to use.");
                isFirebaseInitialized = true;

                // You can now use Firebase services like Auth, Firestore, etc.
            }
            else
            {
                // Firebase could not be initialized
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                isFirebaseInitialized = false;
            }
        });
    }

    public bool IsFirebaseInitialized()
    {
        return isFirebaseInitialized;
    }
}
