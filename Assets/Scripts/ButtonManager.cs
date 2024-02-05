using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject TutorialMenu;
    [SerializeField] private GameObject CreditsMenu;

    [SerializeField] private GameObject WinScreenCanvas;
    [SerializeField] private GameObject DrawSrceen;
    [SerializeField] private GameObject TimerUI;

    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource gameMusic;

    private int timerMinutes;
    private float timerSeconds;

    private int currentLevel;

    private bool isMenuOpen;

    private void Awake()
    {
        timerMinutes = 1;
        timerSeconds = 30;
        TimerUI.GetComponent<TextMeshProUGUI>().text = "Time Remaining: " + timerMinutes.ToString() + ":" + Mathf.RoundToInt(timerSeconds);
        if(PauseMenu.active)
        {
            Time.timeScale = 0;
        }
        currentLevel = 1;
        menuMusic.Play();
        gameMusic.Pause();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(isMenuOpen)
            {
                ResumeGame();
            }
            else
            {                
                PauseGame();
            }
            
        }
    }

    private void FixedUpdate()
    {
        
        if(timerMinutes >= 0)
        {
            timerSeconds -= Time.deltaTime;
            TimerUI.GetComponent<TextMeshProUGUI>().text = "Time Remaining: " + timerMinutes.ToString() + ":" + (timerSeconds < 10 ? "0" : "") + Mathf.RoundToInt(timerSeconds);
            if (timerSeconds <=0)
            {
                timerMinutes -= 1;                
                
                if (timerSeconds < 0 && timerMinutes < 0)
                {
                    TimerUI.GetComponent<TextMeshProUGUI>().text = "Time Remaining: 0:00";
                    WinScreenCanvas.SetActive(true);
                    DrawSrceen.SetActive(true);
                    
                    Time.timeScale = 0;
                }
                timerSeconds = 59f;
            }
        }
    }

    public void PauseGame()
    {
        isMenuOpen = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        menuMusic.Play();
        gameMusic.Pause();
    }
    public void ResumeGame()
    {
        isMenuOpen = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        menuMusic.Pause();
        gameMusic.Play();
    }

    public void OpenTutorial()
    {
        TutorialMenu.SetActive(true);
    }

    public void CloseTutorial()
    {
        TutorialMenu.SetActive(false);
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsMenu.SetActive(false);
    }

    public void RestLevel()
    {
        if (SceneManager.GetActiveScene().name == ("Level1"))
        {
            SceneManager.LoadScene("Level1");
            timerMinutes = 1;
            timerSeconds = 30;
            ResumeGame();
        }
        /*else if (SceneManager.GetActiveScene().name == ("Level2"))
        {
            SceneManager.LoadScene("Level2");
            timerMinutes = 2;
            timerSeconds = 00;
        }
        else */if (SceneManager.GetActiveScene().name == ("Level3"))
        {
            SceneManager.LoadScene("Level3");
            timerMinutes = 3;
            timerSeconds = 00;
        }
        Time.timeScale = 1;
        menuMusic.Pause();
        gameMusic.Play();
    }

    public void NextLevel()
    {
        currentLevel++;
        if(currentLevel > 3)
        {
            currentLevel = 1;
        }
        
        if(SceneManager.GetActiveScene().name == ("Level1"))
        {
            SceneManager.LoadScene("Level3");
            timerMinutes = 2;
            timerSeconds = 00;
        }
        else if (SceneManager.GetActiveScene().name == ("Level2"))
        {
            SceneManager.LoadScene("Level3");
            timerMinutes = 3;
            timerSeconds = 00;
        }
        else if (SceneManager.GetActiveScene().name == ("Level3"))
        {
            SceneManager.LoadScene("Level1");
            timerMinutes = 1;
            timerSeconds = 30;
            ResumeGame();
            
        }

        Time.timeScale = 1;
        menuMusic.Pause();
        gameMusic.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
