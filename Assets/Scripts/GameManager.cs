using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button pauseButton;
    public Button continueButton;
    public Button exitButton;
    public Button replyOver;
    public Button exitButton2;
    public Button exitButton3;
    public Button replyAgain;

    public GameObject pauseDisplay;
    public GameObject gameOverDisplay;
    public GameObject playerWinDisplay;

    public PlayerController playerController;
    public EnemyController enemyController;

    private void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        continueButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExittoLobby);
        replyOver.onClick.AddListener(RePlayMode);
        exitButton2.onClick.AddListener(ExittoLobby);
        replyAgain.onClick.AddListener(RePlayMode);
        exitButton3.onClick.AddListener(ExittoLobby);
    }   
    public void PauseGame()
    {
        SoundManager.Instance.PlaySound(Sounds.Pause);
        pauseDisplay.SetActive(true);
        playerController.isgamePaused = true;
        enemyController.isGameDone = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);
        pauseDisplay.SetActive(false);
        playerController.isgamePaused = false;
        Time.timeScale = 1f;
    }
    private void RePlayMode()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    private void ExittoLobby()
    {
        SoundManager.Instance.PlaySound(Sounds.ExitButtonClick);
        SceneManager.LoadScene(0);
        pauseDisplay.SetActive(false);
        playerController.isgamePaused = false;
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        gameOverDisplay.SetActive(true);
        SoundManager.Instance.PlaySound(Sounds.GameOver);

    }
    public void PlayerWin()
    {
        playerWinDisplay.SetActive(true);
        SoundManager.Instance.PlaySound(Sounds.Celebrate);
    }

}
