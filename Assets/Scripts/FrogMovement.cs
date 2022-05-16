using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    public WallMovement WallMovementSc;

    public GameObject SummonVineGO;

    public Animator Animator;

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

    public float VineOffsetX;
    public float VineOffsetY;


    private float LastGravityScale;

    private Rigidbody2D rb;

    private Vector2 movement;

    public bool IsJumping = false;
    public bool IsCrouching = false;
    public bool IsGliding = false;
    public bool IsAlive = true;
    public bool IsVineSummonable = true;
    public bool IsOnGround = true;

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
        Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Jump();
        Crouch();
        LongJump();
        Glide();
        SlowTheWall();
        SummonVine();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.D) && IsAlive == true || Input.GetKey(KeyCode.RightArrow) && IsAlive == true)
        {
            Animator.SetBool("IsWalking", true);
            transform.Translate(Vector2.right * (Time.deltaTime * Speed), Space.World);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            Animator.SetBool("IsWalking", true);
        }
        else if (Input.GetKey(KeyCode.A) && IsAlive == true || Input.GetKey(KeyCode.LeftArrow) && IsAlive == true)
        {
            transform.Translate(Vector2.left * (Time.deltaTime * Speed), Space.World);
            Animator.SetBool("IsWalking", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Animator.SetBool("IsWalking", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJumping == false && IsCrouching == false && IsAlive == true)
        {
            IsJumping = true;
            rb.AddForce(new Vector2(rb.velocity.y, NormalJumpForce), ForceMode2D.Impulse);
            Animator.SetBool("IsJumpingUp", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsOnGround = true;
            IsJumping = false;
            IsGliding = false;
            Animator.SetBool("IsJumpingUp", false);
            rb.gravityScale = NormalGravityScale;
            Animator.SetBool("IsFalling", false);
        }
        if (collision.gameObject.CompareTag("Branch"))
        {
            IsOnGround = false;
            IsJumping = false;
            IsGliding = false;
            Animator.SetBool("IsJumpingUp", false);
            rb.gravityScale = NormalGravityScale;
            Animator.SetBool("IsFalling", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && IsJumping == false && IsOnGround == false|| collision.gameObject.tag == "Branch" && IsJumping == false && IsOnGround == false)
        {
            Animator.SetBool("IsFalling", true);
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
            Animator.SetBool("IsJumpingUp", true);
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

    // To Summon and Move The Vine
    GameObject Vine;
    private void SummonVine()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsVineSummonable == true && IsJumping == false && IsGliding == false && IsOnGround == true)
        {
            Vine =  Instantiate(SummonVineGO, new Vector3 (gameObject.transform.position.x + VineOffsetX, gameObject.transform.position.y + VineOffsetY, gameObject.transform.position.z), Quaternion.identity);
            StartCoroutine(MoveToPosition(Vine.transform, new Vector3(gameObject.transform.position.x + VineOffsetX, gameObject.transform.position.y + VineOffsetY + 2.25f, gameObject.transform.position.z), 1f));
        }
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}