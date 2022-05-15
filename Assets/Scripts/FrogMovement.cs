using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    public WallMovement WallMovementSc;

    private float Speed = 4f;
    public float NormalSpeed = 10f;
    public float GlidingSpeed = 5.5f;

    public float NormalJumpForce = 7f;
    public float LongJumpForce = 10f;

    public float NormalGravityScale = 1.25f;
    public float LongJumpGravityScale = 1.5f;
    public float GlidingGravityScale = 1.1f;

    public float WallSlowTime;
    public float WallSlowCoolDown;

    private float LastGravityScale;

    private Rigidbody2D rb;

    private Vector2 movement;

    public bool IsJumping = false;
    public bool IsCrouching = false;
    public bool IsGliding = false;
    public bool IsAlive = true;

    public CircleCollider2D NormalCollider;
    public CircleCollider2D CrouchedCollider;

    private void Start()
    {
        NormalCollider.enabled = true;
        CrouchedCollider.enabled = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = NormalGravityScale;
        WallMovementSc = GameObject.Find("Wall").GetComponent<WallMovement>();
        IsAlive = true;
    }

    void Update()
    {
        Movement();
        Jump();
        Crouch();
        LongJump();
        Glide();
        SlowTheWall();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.D) && IsAlive == true || Input.GetKey(KeyCode.RightArrow) && IsAlive == true)
        {
            transform.Translate(Vector2.right * (Time.deltaTime * Speed), Space.World);
        }
        if (Input.GetKey(KeyCode.A) && IsAlive == true || Input.GetKey(KeyCode.LeftArrow) && IsAlive == true)
        {
            transform.Translate(Vector2.left * (Time.deltaTime * Speed), Space.World);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJumping == false && IsCrouching == false && IsAlive == true)
        {
            IsJumping = true;
            rb.AddForce(new Vector2(rb.velocity.y, NormalJumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
            IsGliding = false;
            rb.gravityScale = NormalGravityScale;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && IsCrouching == false && IsJumping == false && IsAlive == true)
        {
            IsCrouching = true;
            NormalCollider.enabled = false;
            CrouchedCollider.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && IsCrouching == true && IsAlive == true)
        {
            IsCrouching = false;
            NormalCollider.enabled = true;
            CrouchedCollider.enabled = false;
        }
    }

    private void LongJump()
    {
        if (IsCrouching == true && Input.GetKeyDown(KeyCode.Space) && IsAlive == true)
        {
            IsJumping = true;
            rb.gravityScale = LongJumpGravityScale;
            rb.AddForce(new Vector2(rb.velocity.y, LongJumpForce), ForceMode2D.Impulse);
        }
    }

    private void Glide()
    {
        if (IsJumping == true && Input.GetKeyDown(KeyCode.LeftShift) && IsGliding == false && IsAlive == true)
        {
            IsGliding = true;
            LastGravityScale = rb.gravityScale;
            rb.gravityScale = GlidingGravityScale;
            Speed = GlidingSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsGliding = false;
            rb.gravityScale = LastGravityScale;
            Speed = NormalSpeed;
        }
    }

    private void SlowTheWall()
    {
        if (Input.GetKey(KeyCode.Q) && IsAlive == true)
        {
            StartCoroutine(TimeCounter());
        }
    }
    private IEnumerator TimeCounter()
    {
        float countdown = 5f;
        WallMovementSc.WallIsSlowed = true;
        yield return new WaitForSeconds(countdown);
        WallMovementSc.WallIsSlowed = false;
    }
}