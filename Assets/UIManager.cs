using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
}
