using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooberMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] uint killGooberValue = 10;
    Rigidbody2D myRigidBody;

    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        myRigidBody.velocity = new Vector2(moveSpeed, 0.0f);
    }

    public uint GetValue() {
        return killGooberValue;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // Check the tag of the colliding object
        if (collision.CompareTag("Ground")) {
            moveSpeed = -moveSpeed;
            FlipEnemyDirection();
        }
    }

    private void FlipEnemyDirection() {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
