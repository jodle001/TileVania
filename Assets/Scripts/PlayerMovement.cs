using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    LayerMask myLayerMask;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool hasGrabbedClimbingObject;
    float speedOfClimbingAnimatorAtStart;
    bool isAlive;

    [SerializeField] float playerSpeed = 10.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float climbSpeed = 5.0f;
    [SerializeField] Vector2 deathKick;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] float jumpVolume = 0.2f;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] float shootVolume = 1.0f;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] float deathVolume = 1.0f;

    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        hasGrabbedClimbingObject = false;
        speedOfClimbingAnimatorAtStart = myAnimator.GetFloat("climbingMultiplier");
        isAlive = true;
    }

    void FixedUpdate() {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }   

    void Die() {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) ||
            myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) {
            AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathVolume);
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnMove(InputValue value) {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    } 

    void OnJump(InputValue value) {
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        if (value.isPressed) {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
            AudioSource.PlayClipAtPoint(jumpAudio, Camera.main.transform.position, jumpVolume);
        }
    }

    void Run() {
        if (!isAlive) { return; }
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    
    void FlipSprite() {
        if (!isAlive) { return; }
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
     
    void OnFire(InputValue value) {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
        AudioSource.PlayClipAtPoint(shootAudio, Camera.main.transform.position, shootVolume);
    }

    void ClimbLadder() {
        if (!isAlive) { return; }
        // If the player is not touching a climbing layer object
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            hasGrabbedClimbingObject = false;
            return; 
        }

        // Get all the control booleans for the logic in this section.
        bool isTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isMovingInY = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        hasGrabbedClimbingObject = hasGrabbedClimbingObject || isMovingInY;

        // Set the climbing animation after player has grabbed the climbing objects, 
        // but not while the player is still touching the ground.
        myAnimator.SetBool("isClimbing", !isTouchingGround && hasGrabbedClimbingObject);

        // Stop the animation if the player isn't moving up or down.
        if (isMovingInY) {
            myAnimator.SetFloat("climbingMultiplier", speedOfClimbingAnimatorAtStart);
        }else {
            myAnimator.SetFloat("climbingMultiplier", 0f);
        }

        // Only have the character move and change the gravity scale if the player had 
        // grabbed the climbing object first. This stops the character from grabbing the
        // ladder automatically while just jumping by it.
        if (hasGrabbedClimbingObject) {
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0f;
        }


    }
}
