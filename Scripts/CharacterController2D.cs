using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(CharacterMover2D))]
public class CharacterController2D : MonoBehaviour
{
    CharacterMover2D characterMovement;
    new CapsuleCollider2D collider;

    [Header("Movement")]
    public float speed = 2f;
    public float jumpForce = 8f;

    protected Vector2 movementInput;

    [Header("Collision")]
    public float groundCheckRadius = 0.4f;
    public float groundCheckLength = 0.1f;
    public LayerMask groundLayer;

    protected bool grounded;
    protected Vector2 velocity;

    protected Vector2 groundNormal;
    protected Vector2 groundPosition;
    protected PhysicsMaterial2D groundMaterial;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMover2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        grounded = IsGrounded();

        Move();
    }

    public virtual void Move()
    {
        float deltaTime = Time.fixedDeltaTime;
        Vector3 desiredMovement = movementInput * speed;
        if (grounded)
        {
            desiredMovement = Vector3.ProjectOnPlane(movementInput, groundNormal).normalized*desiredMovement.magnitude;
        }

        if (grounded && velocity.y <= 0)
        {
            velocity = Vector2.zero;

            Vector2 delta = groundPosition - collider.ClosestPoint(groundPosition);
            transform.position += (Vector3)delta;
        }
        else
        {
            velocity += Physics2D.gravity * deltaTime;
        }
        Vector3 movement;
        Vector3 finalVelocity;
        characterMovement.Move((velocity) * deltaTime, out finalVelocity);
        transform.position += finalVelocity;
        velocity = finalVelocity / deltaTime;

        characterMovement.Move(desiredMovement* deltaTime, out movement);
        transform.position += movement;
    }

    public virtual void Jump()
    {
        velocity = Vector2.up * jumpForce;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + (Vector3.up * groundCheckRadius), groundCheckRadius, Vector2.down, groundCheckLength, groundLayer);

        if(hit)
        {
            groundNormal = hit.normal;
            groundMaterial = hit.collider.sharedMaterial;
            groundPosition = hit.point;
            return true;
        }

        return false;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3.up*(groundCheckRadius-groundCheckLength)), groundCheckRadius);
    }
}
