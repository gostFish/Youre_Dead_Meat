using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public int playerNumber;

    private GameObject GameController;
    private PlayerCommon cmn;
    private PlayerFlags flags;
        
    private int animationIndex;

    private GameObject playerWinScreen;

    private Vector3 respawnPoint;
    private GameObject respawnTimerUI;
    private TextMeshProUGUI respawnTimerText;
    private float respawnTime;

    private RawImage[] livesUI;
    private int remainingLives;


    private void Awake()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        cmn = GameController.GetComponent<PlayerCommon>();
        flags = transform.GetComponent<PlayerFlags>();

        
        if(playerNumber == 0) //Temporary until more players are introduced
        {
            playerWinScreen = cmn.playerWinScreens[1];   
        }
        else
        {
            playerWinScreen = cmn.playerWinScreens[0];
        }

        respawnTimerUI = cmn.respawnTimers[playerNumber];
        respawnTimerText = respawnTimerUI.GetComponent<TextMeshProUGUI>();

        livesUI = cmn.lifeIcons[playerNumber].GetComponentsInChildren<RawImage>();

        remainingLives = livesUI.Length;
        animationIndex = 0;

        respawnPoint = cmn.playerSpawnPoints[playerNumber].transform.position;

        InitialiseUI();
    }

    

    private void InitialiseUI()
    {
        respawnTimerUI = cmn.respawnTimers[playerNumber];
        respawnTimerUI.SetActive(false);

        respawnTimerText = respawnTimerUI.GetComponent<TextMeshProUGUI>();

        foreach(RawImage hearts in livesUI)
        {
            hearts.texture = cmn.liveHeartTexture;
        }
    }

    /*
    public IEnumerator AnimateMoving()
    {
        gameObject.transform.GetComponent<SpriteRenderer>().sprite = spriteSheet.;// animationFrames[animationIndex];
        yield return new WaitForSeconds(cmn.animationSpeed);
        animationIndex++;
        if (animationIndex >= animationFrames.Length)
        {
            animationIndex = 0;
        }
        if (flags.animateMoving)
        {
            StartCoroutine(AnimateMoving());
        }
        else
        {
            animationIndex = 0;
        }
    }*/

    public void PlayerKilled()
    {
        if(respawnTime > 0)
        {
            remainingLives--;
            livesUI[remainingLives].texture = cmn.deadHeartTexture;
            if (remainingLives == 0)
            {
                PlayerWins();
            }
        }
        StartCoroutine(DeathTimer());
    }

    public void PlayerWins()
    {
        playerWinScreen.transform.parent.gameObject.SetActive(true);        
        playerWinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public IEnumerator DeathTimer()
    {
        respawnTime = cmn.respawnTime;
        respawnTimerUI.SetActive(true);
        while (respawnTime>0)
        {
            //waiting 1 second in real time and increasing the timer value
            yield return new WaitForSecondsRealtime(0.01f);
            respawnTime -= 0.01f;
            respawnTimerText.text = respawnTime.ToString("F2");
        }
        respawnTimerUI.SetActive(false);
        transform.position = respawnPoint;
    }
}
