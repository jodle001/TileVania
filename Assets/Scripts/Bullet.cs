using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidBody;
    PlayerMovement playerMovement;
    float xSpeed;

    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    void Update() {
        myRigidBody.velocity = new Vector2(xSpeed, 0.0f);
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            //var enemy = collision.gameObject;
            //GameSession gameSession = FindObjectOfType<GameSession>();
            //uint value = enemy.GetComponent<GooberMovement>().GetValue();
            //Debug.Log("enemy value: " + value);
            //gameSession.AddPointsToScore(value);
            //Destroy(collision.gameObject);
            return;
        }
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            var enemy = collision.gameObject;
            GameSession gameSession = FindObjectOfType<GameSession>();
            uint value = enemy.GetComponent<GooberMovement>().GetValue();
            Debug.Log("enemy value: " + value);
            gameSession.AddPointsToScore(value);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
