using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform spawnPoint;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        // Hentikan gerakan
        if (rb != null)
            rb.velocity = Vector2.zero;

        // Kembali ke spawn
        transform.position = spawnPoint.position;
    }
}