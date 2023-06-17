using System;
using TMPro;
using UnityEngine;

public class GrenadeGameManager : MonoBehaviour
{
    public int score = 0;
    public int scoreGoal = 10;
    public float countdownTime = 60.0f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreGoalText;
    public TextMeshProUGUI countdownText;

    private float timeLeft;
    private bool gameStarted = false;
    public GameObject StartMenu;
    public GameObject GameplayUI;
    public GameObject RighthandObject;
    private MenuRaycaster menuRaycaster;

    public GameObject WinScreen;
    public GameObject LoseScreen;

    public AudioClip buttonPressClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    private AudioSource audioSource;
    private AudioManager audioManager;

    void Start()
    {
        GameplayUI.SetActive(false);
        UpdateScoreUI();
        UpdateCountdownUI();

        // Assign ausiosource to the variable audioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        // Get the script MenuRaycaster on rightHandObject
        menuRaycaster = RighthandObject.GetComponent<MenuRaycaster>();

        // Get a reference to the AudioManager component on this game object
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        if (gameStarted)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f || score >= scoreGoal)
            {
                gameStarted = false;
                // Call PlayIntroTrack() on the AudioManager component
                audioManager.PlayIntroTrack();


                if (score >= scoreGoal)
                {
                    onGameWin();
                }
                else
                {
                    OnGameLose();
                }
            }
            else
            {
                UpdateCountdownUI();
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        scoreGoalText.text = "Goal: " + scoreGoal.ToString();
    }

    void UpdateCountdownUI()
    {
        var timeSpan = TimeSpan.FromSeconds(timeLeft);
        countdownText.text = timeSpan.ToString(@"mm\:ss");
    }

    public void StartGame()
    {
        timeLeft = countdownTime;
        gameStarted = true;
        StartMenu.SetActive(false);
        GameplayUI.SetActive(true);
        menuRaycaster.ClearLineRenderer();
        menuRaycaster.enabled = false;
        // Call PlayActionTrack() on the script AudioManager on the GameObject AudioManager
        FindObjectOfType<AudioManager>().PlayActionTrack();
        // if buttonPressClip is not null, play the sound
        if (buttonPressClip != null)
        {
            // Play the sound
            audioSource.PlayOneShot(buttonPressClip);
        }

    }

    private void OnGameLose()
    {
        LoseScreen.SetActive(true);
        menuRaycaster.enabled = true;
        audioSource.PlayOneShot(loseClip);
    }

    private void onGameWin()
    {
        WinScreen.SetActive(true);
        menuRaycaster.enabled = true;
        audioSource.PlayOneShot(winClip);
       

    }

    // Quit the application
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }
}
