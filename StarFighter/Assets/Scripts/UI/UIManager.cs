using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private Slider lifeBar;
    [SerializeField] private Slider shieldBar;
    [Space] 
    [SerializeField] private GameObject pauseMenu;

    private bool isPause;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }


    public void TogglePause()
    {
        if(isPause)Unpause();
        else Pause();
    }
    
    public void Pause()
    {
        isPause = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    } 
    
    public void Unpause()
    {
        isPause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }


    #region Buttons Func

    public void Resume()
    {
        Unpause();
    }

    public void BackToMenu()
    {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
