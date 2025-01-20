using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
public class MenuManager : MonoBehaviour
{

    public TMP_Text HighScoreText;
    public TMP_InputField UsernameField;
    public GameObject EnterUsenamePanel;public TMP_Text UsernameText;
    public FirebaseUser User;
    public DatabaseReference reference;
    private void Start()
    {
        //if(PlayerPrefs.GetString("Username")=="")
        //{
        //    EnterUsenamePanel.SetActive(true);
        //}
        HighScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
      //  SetUsernameInLeaderBoard();
    }

    public void LoadSceneNext()
    {
        SceneManager.LoadScene(1);
    }
    public void SetUsername(string username)
    {
        
        PlayerPrefs.SetString("Username", username);
        SetUsernameInLeaderBoard();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetUsernameInLeaderBoard()
    {
        UsernameText.text = PlayerPrefs.GetString("Username");
    }


}
