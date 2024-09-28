using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float acceleration = .3f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 3.5f;
    [Range(0f, 1f)]
    [SerializeField] private float dragDecay = .7f;
    [SerializeField] BoxCollider2D groundCheck;
    [SerializeField] LayerMask groundMask;
    private float xInput;
    private bool isGrounded = false;

    // Update is called once per frame
    void Update()
    {
        getInput();
        HandleJump();
    }
    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        moveWithInput();
    }
    private void getInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }
    private void moveWithInput()
    {
        float increment = xInput * acceleration;
        float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -speed, speed);
        if (Mathf.Abs(xInput) > 0)
        {
            rb.velocity = new Vector2(newSpeed, rb.velocity.y);
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction*3, 3, 3);
        }

        
    }
    void HandleJump()
    {
        //A changer
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void ApplyFriction()
    {
        if (isGrounded && xInput == 0 && rb.velocity.y <=0)
        {
            rb.velocity *= dragDecay;
        }
    }
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        Debug.Log("Grounded: " + isGrounded);
    }
}
