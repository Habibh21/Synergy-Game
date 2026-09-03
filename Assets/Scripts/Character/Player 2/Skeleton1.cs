using UnityEngine;
using System.Collections;

public class Skeleton1 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Batas Jarak Antar Player")]
    public Transform otherPlayer;
    public float maxDistance = 30f;

    [Header("Sistem Fall Damage")]
    public float fallDamageThreshold = 3f;
    public int health = 100;

    [Header("Death")]
    public float deadAnimationDuration = 1.2f;

    private bool isGrounded;
    private bool isDead;
    private float highestYPosition;

    private Rigidbody2D rb;
    private Animator anim;
    private RespawnPlayer respawnPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPlayer = GetComponent<RespawnPlayer>();

        highestYPosition = transform.position.y;
    }

    void Update()
    {
        if (isDead) return;

        float move = 0f;

        // Keyboard (Player 2)
        if (Input.GetKey(KeyCode.D))
            move = 1f;
        else if (Input.GetKey(KeyCode.A))
            move = -1f;

        // Controller
        float controllerMove = Input.GetAxis("Horizontal");

        if (Mathf.Abs(controllerMove) > 0.1f)
            move = controllerMove;

        MovePlayer(move);

        // Jump
        if ((Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.JoystickButton1)) &&
             isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        // Hitung tinggi jatuh
        if (!isGrounded)
        {
            if (transform.position.y > highestYPosition)
                highestYPosition = transform.position.y;
        }

        // Animasi
        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(move));

        // Hadap kanan / kiri
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

    private void TakeFallDamage()
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
            // Animasi Hit
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

        // Hentikan gerakan
        rb.velocity = Vector2.zero;

        // Animasi Dead
        if (anim != null)
        {
            anim.ResetTrigger("Hit");
            anim.SetTrigger("Dead");
        }

        // Tunggu animasi selesai
        yield return new WaitForSeconds(deadAnimationDuration);

        // Respawn ke spawn point
        if (respawnPlayer != null)
        {
            respawnPlayer.Respawn();
        }

        // Reset HP
        health = 100;

        // Reset status
        isDead = false;
        isGrounded = true;
        highestYPosition = transform.position.y;

        // Reset animasi
        if (anim != null)
        {
            anim.ResetTrigger("Dead");
            anim.ResetTrigger("Hit");
            anim.SetFloat("Speed", 0f);
        }
    }
}