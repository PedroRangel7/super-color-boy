using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]   // Every object with this script (Entity) also needs a Rigidbody2D component.
public abstract class Entity : MonoBehaviour
{
    [Header("Movement")]     // Unity Inspector Header.
    [SerializeField] [Range(200, 1000)] protected float movementSpeed = 700f;     // Entity's horizontal movement speed.
    public EffectApplier effects;       // Entity's effect list.

    protected Rigidbody2D rigidBody;    // Rigidbody component reference.
    protected bool facingRight;         // Whether the entity is facing right (used to flip the sprite according to it's horizontal movement).
    protected bool canMove;             // Whether the entity can move.

    protected virtual void Awake() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {
        effects = new EffectApplier(gameObject);
        facingRight = true;
        canMove = true;
    }

    protected virtual void Update() {
        effects.ApplyEffects();
    }

    protected virtual void Move(float direction) {
        rigidBody.velocity = new Vector2(Mathf.Clamp(direction, -1f, 1f) * movementSpeed * Time.deltaTime, rigidBody.velocity.y);   // Adds X velocity depending on direction and 'movementSpeed', maintains Y velocity.
        if ((facingRight && direction < 0f) || !facingRight && direction > 0f)      // If the entity is moving to the direction it is not facing...
            Flip();                                                                 // ... Flip entity's sprite.
    }

    private void Flip() {   // Flips entity's sprite.
        facingRight = !facingRight;     // Inverts 'facingRight' value.
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);   // Flips entity's sprite.
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }
}
