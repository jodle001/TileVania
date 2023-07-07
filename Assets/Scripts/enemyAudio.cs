using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAudio : MonoBehaviour
{
    [SerializeField] bool isJumping = false;
    [SerializeField] float maxVolume = 0.2f;
    [SerializeField] float minVolume = 0;
    [SerializeField] float maxDist = 10f;
    [SerializeField] float minDist = 3f;
    AudioSource audioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound() {
        float dist = Mathf.Sqrt(
            Mathf.Pow(Camera.main.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(Camera.main.transform.position.y - transform.position.y, 2));
        if (dist < minDist) {
            audioSource.volume = maxVolume;
        } else if (dist > maxDist) {
            audioSource.volume = minVolume;
        } else {
            audioSource.volume = maxVolume - ((dist - minDist) / (maxDist - minDist));
        }
        audioSource.Play();
    }

    private void FixedUpdate() {
        if (isJumping) {
            PlaySound();
            isJumping = false;
        }
    }
}
