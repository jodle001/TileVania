using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1.0f;
    [SerializeField] uint levelSuccessValue = 25;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel() {

        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check to make sure we don't try to index greater than the number of available scenes
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.AddPointsToScore(levelSuccessValue);

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
