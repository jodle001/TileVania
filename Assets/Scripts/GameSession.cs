using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] float levelLoadDelay = 1.0f;
    uint playerScore = 0;


    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;


    void Awake() {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1 ) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath() {
        if (playerLives > 1) {
            StartCoroutine(TakeLife());
        } else {
            StartCoroutine(ResetGameSession());
        }
    }

    public void AddPointsToScore(uint value) {
        playerScore += value;
        scoreText.text = playerScore.ToString();
    }

    IEnumerator TakeLife() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        playerLives--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        livesText.text = playerLives.ToString();
    }

    IEnumerator ResetGameSession() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
