using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    public float Speed = 4f;
    public float NormalJumpForce = 7f;
    public float LongJumpForce = 10f;
    public float NormalGravityScale = 1.25f;
    public float LongJumpGravityScale = 1.5f;

    private Rigidbody2D rb;

    private Vector2 movement;

    //bool jump = false;
    public bool IsJumping = false;
    public bool IsCrouching = false;
    //bool crouch = false;

    public CircleCollider2D NormalCollider;
    public CircleCollider2D CrouchedCollider;

    private void Start()
    {
        NormalCollider.enabled = true;
        CrouchedCollider.enabled = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = NormalGravityScale;
    }

    void Update()
    {
        Movement();
        Jump();
        Crouch();
        LongJump();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * (Time.deltaTime * Speed), Space.World);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * (Time.deltaTime * Speed), Space.World);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJumping == false && IsCrouching == false)
        {
            IsJumping = true;
            rb.AddForce(new Vector2(rb.velocity.y , NormalJumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            rb.gravityScale = NormalGravityScale;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && IsCrouching == false && IsJumping == false)
        {
            IsCrouching = true;
            NormalCollider.enabled = false;
            CrouchedCollider.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && IsCrouching == true)
        {
            IsCrouching = false;
            NormalCollider.enabled = true;
            CrouchedCollider.enabled = false;
        }
    }

    private void LongJump()
    {
        if (IsCrouching == true && Input.GetKeyDown(KeyCode.Space))
        {
            IsJumping = true;
            rb.gravityScale = LongJumpGravityScale;
            rb.AddForce(new Vector2(rb.velocity.y, LongJumpForce), ForceMode2D.Impulse);
        }
    }
}