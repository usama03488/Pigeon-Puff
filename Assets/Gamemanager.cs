using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Gamemanager Instance { get; private set; }
    public bool Islogin = false;
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Check if the instance already exists and if it does, destroy the duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

    
        Instance = this;

        // Optional: Prevent this object from being destroyed on scene load
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }
    public void Set_Login(bool status)
    {
        Debug.Log("Login Status Set Sucess");
        Islogin = status;
    }
    public bool get_Login()
    {
        Debug.Log("Login get Status call Sucess");
        return Islogin;
    }
    
    void Update()
    {
        
    }
}
