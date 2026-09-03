using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Batas Jarak Antar Player")]
    public Transform otherPlayer;
    public float maxDistance = 30f;

    [Header("Sistem Fall Damage")]
    public float fallDamageThreshold = 3f;
    public int health = 30;

    [Header("Death")]
    public float deadAnimationDuration = 1.2f;

    private bool isGrounded;
    private bool isDead;
    private float highestYPosition;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        highestYPosition = transform.position.y;
    }

    void Update()
    {
        if (isDead) return;

        //-------------------------
        // Movement (Arrow Keys)
        //-------------------------
        float move = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
            move = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            move = -1f;

        MovePlayer(move);

        //-------------------------
        // Jump (Up Arrow)
        //-------------------------
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        //-------------------------
        // Fall Detection
        //-------------------------
        if (!isGrounded)
        {
            if (transform.position.y > highestYPosition)
                highestYPosition = transform.position.y;
        }

        //-------------------------
        // Animation
        //-------------------------
        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(move));

        //-------------------------
        // Flip Character
        //-------------------------
        if (move > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void MovePlayer(float move)
    {
        bool canMove = true;

        if (otherPlayer != null)
        {
            float distance = Vector2.Distance(transform.position, otherPlayer.position);

            if (distance >= maxDistance)
            {
                bool movingAway =
                    (move > 0 && transform.position.x > otherPlayer.position.x) ||
                    (move < 0 && transform.position.x < otherPlayer.position.x);

                if (movingAway)
                    canMove = false;
            }
        }

        if (canMove)
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void ResetStatusAfterPortal()
    {
        isGrounded = true;
        highestYPosition = transform.position.y;

        if (anim != null)
            anim.SetFloat("Speed", 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                if (!isGrounded)
                {
                    float fallDistance = highestYPosition - transform.position.y;

                    if (fallDistance >= fallDamageThreshold)
                        TakeFallDamage();
                }

                isGrounded = true;
                highestYPosition = transform.position.y;
                break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isDead) return;

        isGrounded = false;
        highestYPosition = transform.position.y;
    }

    void TakeFallDamage()
    {
        if (isDead) return;

        health -= 10;

        if (health <= 0)
        {
            health = 0;
            StartCoroutine(DeadRoutine());
        }
        else
        {
            if (anim != null)
            {
                anim.ResetTrigger("Dead");
                anim.SetTrigger("Hit");
            }
        }
    }

    IEnumerator DeadRoutine()
    {
        isDead = true;

        rb.velocity = new Vector2(0, rb.velocity.y);

        if (anim != null)
        {
            anim.ResetTrigger("Hit");
            anim.SetTrigger("Dead");
        }

        yield return new WaitForSeconds(deadAnimationDuration);

        Time.timeScale = 0f;
    }
}