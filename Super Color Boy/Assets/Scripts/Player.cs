using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]   // Every object with this script (Player) also needs a Rigidbody2D component.
public class Player : MonoBehaviour
{
    #region Variables
    [Header("Player Movement")]     // Unity Inspector Header.
    [SerializeField] [Range(200, 1000)] private float movementSpeed = 700f;     // Player's horizontal movement speed.
    [SerializeField] [Range(2, 8)] private float jumpForce = 5f;                // Player's jump force (height).
    [SerializeField] [Range(0, 5)] private int maxAirJumps = 0;                 // Player's maximum air jumps (does not include the first, on-ground one).
    [SerializeField] [Range(0.01f, 1f)] private float maxJumpTime = 0.35f;      // Maximum amount of time that the jump key can be held while jumping in order to increase jump height.
    [Header("Ground Check")]        // Unity Inspector Header.
    [SerializeField] [Range(0.2f, 0.5f)] private float feetRadius = 0.45f;      // Radius of the circle collider that is created at the player's "feet" position in order to check if it's on the ground.
    [SerializeField] private Transform playersFeet;                             // Player's "feet" position.
    [SerializeField] private LayerMask groundLayer;                             // Layer which determines which scenes objects are considered "ground".

    private int airJumps;               // Current air jumps left.
    private float jumpTimer;            // Current jump timer. Increases from 0 to 'maxJumpTime'.
    private bool isJumping;             // Whether the player is currently jumping (pressed the jump key and is still holding it while 'jumpTimer' is less than 'maxJumpTime').
    private bool onGround;              // Whether the player is currently on the ground.
    private bool facingRight;           // Whether the player is facing right (used to flip the sprite according to it's horizontal movement).
    private Vector2 movementInput;      // Movement Input. X is horizontal movement, Y is vertical movement (jump).
    private Rigidbody2D rigidBody;      // Rigidbody component reference.
    #endregion

    #region Basic
    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();    // Gets the reference for the Rigidbody component.
    }

    private void Start()
    {
        airJumps = 0;           // Set variables' initial values (start).
        jumpTimer = 0f;         //
        isJumping = false;      // 
        onGround = false;       //
        facingRight = true;     // Set variables' initial values (end).
    }

    private void Update()
    {
        GetInputs();    // Checks if player is sending any input each frame.
        if (movementInput.x != 0f)      // If the player is sending horizontal movement input...
            Move();                     // ... Move it.
        if (movementInput.y != 0f || isJumping)     // If the player is sending vertical movement input or is currently jumping...
            Jump();                                 // ... Execute the jump function.
    }

    private void GetInputs() {      // Checks any player input.
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetButton("Jump") ? 1f : 0f);    // Gets movement input (horizontal and jump).
    }
    #endregion

    #region Movement
    private void Move() {       // Moves the player horizontally.
        rigidBody.velocity = new Vector2(movementInput.x * movementSpeed * Time.deltaTime, rigidBody.velocity.y);   // Adds X velocity depending on player's input and 'movementSpeed', maintains Y velocity.
        if ((facingRight && movementInput.x < 0f) || !facingRight && movementInput.x > 0f)      // If the player is moving to the direction it is not facing...
            Flip();                                                                             // ... Flip player's sprite.
    }

    private void Jump() {       // Does all jump-related stuff.
        CheckGround();      // Check if player is on the ground.
        if (Input.GetButtonDown("Jump") && (onGround || airJumps > 0)) {                            // If the jump button was just pressed and the player is on the ground or has remaining air jumps...
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, movementInput.y * jumpForce);    // ... Add Y velocity, maintain X velocity;
            jumpTimer = 0f;                                                                         // Reset jump timer;
            isJumping = true;                                                                       // Sets 'isJumping' to true;
            if (!onGround)      // If the player is not on ground at the time of the jump...
                airJumps--;     // ... Reduce the remaining air jumps by 1.
        }
        else if (Input.GetButton("Jump") && jumpTimer < maxJumpTime && isJumping) {                 // Else, if the player is already jumping and is still holding the jump button while 'jumpTimer' is less than 'maxJumpTime'...
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, movementInput.y * jumpForce);    // ... Add Y velocity, maintain X velocity;
            jumpTimer += Time.deltaTime;                                                            // Increase 'jumpTimer' by the amount of time passed since last update.
        }
        else {                      // Else (if the jump is done either by releasing the jump key or the timer running out)...
            isJumping = false;      // ... Set 'isJumping' to false.
        }
    }

    private void Flip() {   // Flips player's sprite.
        facingRight = !facingRight;     // Inverts 'facingRight' value.
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);   // Flips player's sprite.
    }

    private void ResetJumps() {     // Resets jump-related stuff.
        airJumps = maxAirJumps;     // Resets the remaining air jumps back to the maximum amount of air jumps the player can have.
    }

    private bool CheckGround() {    // Checks if the player is on the ground.
        onGround = Physics2D.OverlapCircle(playersFeet.position, feetRadius, groundLayer);      // Creates a circle collider at the player's "feet" position with the radius of 'feetRadius' and returns true if it collides with any object on 'groundLayer' layer. Assign that value to 'onGround'.
        if(onGround)            // If the player is on the ground...
            ResetJumps();       // ... Reset jump-related stuff.
        return onGround;        // Returns whether the player is on the ground.
    }
    #endregion
}
