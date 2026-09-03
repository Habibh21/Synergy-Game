using UnityEngine;

public class Trap : MonoBehaviour
{
    private RespawnPlayer[] players;

    void Start()
    {
        players = FindObjectsOfType<RespawnPlayer>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        foreach (RespawnPlayer player in players)
        {
            player.Respawn();
        }
    }
}