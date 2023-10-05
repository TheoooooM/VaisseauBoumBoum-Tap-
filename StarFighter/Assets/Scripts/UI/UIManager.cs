using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image shieldBar;
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

    public void UpdateStats(int currentLife, int currentShield, int maxLife)
    {
        UpdateLife(currentLife, maxLife);
        UpdateShield(currentShield, maxLife);
    }
    public void UpdateLife(int currentLife, int maxLife) => lifeBar.fillAmount = (float)currentLife / maxLife;
    public void UpdateShield(int currentShield, int maxLife) => shieldBar.fillAmount = Mathf.Clamp((float)currentShield / maxLife,0,1);


    #region Pause
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
    #endregion

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
