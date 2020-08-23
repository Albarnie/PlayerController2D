using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterMover2D : MonoBehaviour
{
    new CapsuleCollider2D collider;

    public float normalOffset = 0.005f;

    [Range(0, 1)]
    public float slideAmount = 1f;

    public LayerMask groundLayer;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
    }

    public void Move(Vector3 input, out Vector3 movement)
    {
        Vector3 position = transform.position;
        Vector3 direction = input.normalized;
        float distance = input.magnitude;

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = groundLayer;

        //Perform a sweep test to see if there are any objects in the way
        RaycastHit2D hit = Physics2D.CapsuleCast(transform.position + (Vector3)collider.offset, collider.size, collider.direction, transform.rotation.eulerAngles.z, direction, distance, groundLayer);
        if (hit && !hit.collider.CompareTag("Platform"))
        {
            float normalDistance = distance - hit.distance;

            position += input * hit.distance;
            position += (Vector3)hit.normal * normalOffset;

            //slide along the surface hit
            Vector3 slide = Vector3.ProjectOnPlane(direction * normalDistance * slideAmount, hit.normal);
            position += slide;
        }
        else
        {
            position += input;
        }
        movement = position - transform.position;
        transform.position = position;
    }
}
