using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public Transform playerTransform; // Reference to player's transform
    public Transform cameraTransform; // Reference to the main camera
    public TMP_Text GameOverText;
    public GameObject GameOverPanel;
    public Collider2D playerCollider;
    int CurrentScore;
    void Start()
    {
        playerTransform = transform;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float bottomLimitY = cameraTransform.position.y - cameraTransform.GetComponent<Camera>().orthographicSize; // Account for orthographic camera
        float playerBottomY = playerTransform.position.y - playerCollider.bounds.extents.y; // Adjust for collider

        if(transform.position.y>CurrentScore)
        {
            
            if(PlayerPrefs.GetInt("HighScore") < (int)transform.position.y)
            {
                PlayerPrefs.SetInt("HighScore", (int)transform.position.y);
            }
            CurrentScore =(int)transform.position.y;
            GameOverText.text = "" +CurrentScore;

        }

     
        if (playerBottomY < bottomLimitY)
        {
            GameOver();
           StartCoroutine( Highscores.instance.UploadNewHighscore(PlayerPrefs.GetString("Username"),PlayerPrefs.GetInt("HighScore")));
        }
    }

    void GameOver()
    {
     // StopAllCoroutines();
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
        // Implement your game over logic here
       
        // You can add more actions here like showing a game over screen, stopping the game, etc.
    }
}
