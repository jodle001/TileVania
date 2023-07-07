using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] uint coinValue = 5;

    private bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !wasCollected) {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, 0.5f);
            GameSession gameSession = FindObjectOfType<GameSession>();
            gameSession.AddPointsToScore(coinValue);
            Destroy(gameObject);
        }
    }
}
