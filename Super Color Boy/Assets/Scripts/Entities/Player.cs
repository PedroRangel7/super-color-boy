using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Variables
    [SerializeField] [Range(200f, 800f)] protected float airMovementSpeed = 500f;   // Player's air movement speed.
    private Vector2 movementInput;      // Movement Input. X is horizontal movement, Y is vertical movement (jump).

    [Header("Jump")]            // Unity Inspector Header.
    [SerializeField] [Range(2f, 8f)] private float jumpForce = 4.25f;           // Player's jump force.
    [SerializeField] [Range(0, 5)] private int maxAirJumps = 0;                 // Player's maximum air jumps (does not include the first, on-ground one).
    [SerializeField] [Range(1f, 4f)] private float holdJumpMultiplier = 2f;     // Jump force multiplier while player is holding the jump key.
    [SerializeField] [Range(0.01f, 1f)] private float maxJumpTime = 0.35f;      // Maximum amount of time that the jump key can be held while jumping in order to increase jump height.
    private int airJumps;               // Current air jumps left.
    private float jumpTimer;            // Current jump timer. Increases from 0 to 'maxJumpTime'.
    private bool isJumping;             // Whether the player is currently jumping (pressed the jump key and is still holding it while 'jumpTimer' is less than 'maxJumpTime').

    [Header("Wall Movement")]       // Unity Inspector Header.
    [SerializeField] [Range(0f, 5f)] private float wallSlideSpeed = 1f;             // Player's wallslide speed.
    [SerializeField] [Range(1f, 3f)] private float slideSpeedLooseMultiplier = 2f;  // Player's wallslide speed multiplier while it's not pushing against the wall.
    [SerializeField] [Range(2f, 8f)] private float wallJumpForce = 6f;              // Player's wall jump force.
    [SerializeField] private Vector2 wallJumpAngle = new Vector2(1f, 3f);           // Player's wall jump angle.
    private bool isWallSliding;         // Whether the player is currently wall sliding.
    
    [Header("Ground / Wall Check")]        // Unity Inspector Header.
    [SerializeField] private Vector2 handSize = new Vector2(0.05f, 0.5f);      // Radius of the circle collider that is created at the player's "hand" position in order to check if it's on a wall.
    [SerializeField] private Transform playersHand;                             // Player's "hand" position.
    [SerializeField] private Vector2 feetSize = new Vector2(0.5f, 0.05f);      // Radius of the circle collider that is created at the player's "feet" position in order to check if it's on a ground.
    [SerializeField] private Transform playersFeet;                             // Player's "feet" position.
    [SerializeField] private LayerMask groundWallLayer;                         // Layer which determines which scenes objects are considered grounds / walls.
    private bool onGround;              // Whether the player is currently on a ground.
    private bool onWall;                // Whether the player is currently on a wall.
    #endregion

    #region Basic
    protected override void Start()
    {
        base.Start();           // Entity script's Start().
        airJumps = 0;           // Set 'airJumps' initial value.
        jumpTimer = 0f;         // Set 'jumpTimer' initial value.
        isJumping = false;      // Set 'isJumping' initial value.
        isWallSliding = false;  // Set 'isWallSliding' initial value.
        onGround = false;       // Set 'onGround' initial value.
        onWall = false;         // Set 'onWall' initial value.
        wallJumpAngle.Normalize();  // Normalizes 'wallJumpAngle' so the module is 1.
    }

    protected override void Update()
    {
        base.Update();      // Entity script's Update().
        CheckGround();      // Check if player is on a ground.
        CheckWall();        // Check if player is on a wall.
        GetInputs();        // Checks if player is sending any input each frame.
        Move(movementInput.x);      // Move the player according to movement input.
        if (movementInput.y > 0f || isJumping) {   // If the player is sending vertical movement input or is currently jumping...
            Jump();                                 // ... Execute the jump function.
            WallJump();                             // Execute the wall jump function.
        }
        WallSlide();        // Execute the wall slide function.
    }

    private void GetInputs() {      // Checks any player input.
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetButton("Jump") ? 1f : Input.GetButton("Stomp") ? -1f : 0f);    // Gets movement input (horizontal and jump).
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;       // Sets the color.
        Gizmos.DrawCube(playersFeet.position, feetSize);    // Draws a rectangle on the Unity Editor representing the player's "feet" area when the player is selected.
        Gizmos.color = Color.blue;      // Sets the color.
        Gizmos.DrawCube(playersHand.position, handSize);    // Draws a rectangle on the Unity Editor representing the player's "hand" area when the player is selected.
    }
    #endregion

    #region Movement
    protected override void Move(float direction) {
        if (onGround)                                                                                                                   // If player is on ground...
            rigidBody.velocity = new Vector2(Mathf.Clamp(direction, -1f, 1f) * movementSpeed * Time.deltaTime, rigidBody.velocity.y);   // ... Adds X velocity depending on direction and 'movementSpeed', maintains Y velocity.
        else if (!isWallSliding) {                                                                                      // Else, if the player is also not wall sliding...
            rigidBody.AddForce(new Vector2(Mathf.Clamp(direction, -1f, 1f) * airMovementSpeed * Time.deltaTime, 0f));   // ... Move it on air with 'airMovementSpeed'.
            if (Mathf.Abs(rigidBody.velocity.x) > movementSpeed * Time.deltaTime)                                                               // If the player is faster than it should be...
                rigidBody.velocity = new Vector2(Mathf.Clamp(direction, -1f, 1f) * movementSpeed * Time.deltaTime, rigidBody.velocity.y);       // ... Set its velocity to normal.
        }

        if (((facingRight && direction < 0f) || (!facingRight && direction > 0f)) && !isWallSliding)      // If the player is moving to the direction it is not facing...
            Flip();                                                                   // ... Flip player's sprite.
    }

    private void Jump() {       // Does all jump-related stuff.
        if (Input.GetButtonDown("Jump") && (onGround || airJumps > 0)) {        // If the jump button was just pressed and the player is on the ground or has remaining air jumps...
            rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);    // ... Add a vertical force.
            jumpTimer = 0f;                                                     // Reset jump timer;
            isJumping = true;                                                   // Sets 'isJumping' to true;
            if (!onGround)      // If the player is not on ground at the time of the jump...
                airJumps--;     // ... Reduce the remaining air jumps by 1.
        }
        else if (movementInput.y > 0 && jumpTimer < maxJumpTime && isJumping) {     // Else, if the player is already jumping and is still holding the jump button while 'jumpTimer' is less than 'maxJumpTime'...
            rigidBody.AddForce(jumpForce * Time.deltaTime * holdJumpMultiplier * Vector2.up, ForceMode2D.Impulse);      // ... Add a vertical force.
            jumpTimer += Time.deltaTime;        // Increase 'jumpTimer' by the amount of time passed since last update.
        }
        else {                      // Else (if the jump is done either by releasing the jump key or the timer running out)...
            isJumping = false;      // ... Set 'isJumping' to false.
        }
    }

    private void WallSlide() {
        if (onWall && !onGround && rigidBody.velocity.y <= 0f) {    // If the player is on a wall, is not on a ground and is moving downwards...
            isJumping = false;                                      // ... Set 'isJumping' to false.
            isWallSliding = true;                                   // Set 'isWallSliding' to true.
            if((facingRight && movementInput.x <= 0) || (!facingRight && movementInput.x >= 0))                         // If the player is wallsliding while not pushing against the wall...
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -wallSlideSpeed * slideSpeedLooseMultiplier);    // ... Make it wallslide faster.
            else                                                                                                        // Else...
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -wallSlideSpeed);                                // Make it wallslide at the normal speed.
            ResetJumps();       // Reset jumps.
            if (movementInput.y < 0f)   // If the player is pressing the down button...
                Flip();                 // ... Flip it so it falls off the wall.
        }
        else
            isWallSliding = false;
    }

    private void WallJump() {
        if((isWallSliding) && Input.GetButtonDown("Jump")) {    // If the player is wallsliding or on a wall and presses the jump key...
            float wallJumpDirection = facingRight ? -1f : 1f;   // ... Make the jump direction contrary to the direction the player is facing.  
            Vector2 wallJumpVector = new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y);     // Creates a vector contrary to the one the player is facing, with an angle of 'wallJumpAngle' and multiplied by 'wallJumpForce'.
            rigidBody.AddForce(wallJumpVector, ForceMode2D.Impulse);    // Adds 'wallJumpVector' as an impulse force.
        }
    }

    private void ResetJumps() {     // Resets jump-related stuff.
        airJumps = maxAirJumps;     // Resets the remaining air jumps back to the maximum amount of air jumps the player can have.
    }

    private bool CheckGround() {    // Checks if the player is on a ground.
        onGround = Physics2D.OverlapBox(playersFeet.position, feetSize, 0f, groundWallLayer);      // Creates a circle collider at the player's "feet" position with the radius of 'feetRadius' and returns true if it collides with any object on 'groundWallLayer' layer. Assign that value to 'onGround'.
        if(onGround)            // If the player is on a ground...
            ResetJumps();       // ... Reset jump-related stuff.
        return onGround;        // Returns whether the player is on a ground.
    }

    private bool CheckWall() {
        onWall = Physics2D.OverlapBox(playersHand.position, handSize, 0f, groundWallLayer);      // Creates a circle collider at the player's "hand" position with the radius of 'handRadius' and returns true if it collides with any object on 'groundWallLayer' layer. Assign that value to 'onWall'.
        return onWall;        // Returns whether the player is on a wall.
    }
    #endregion
}
