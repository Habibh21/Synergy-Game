using UnityEngine;
using System.Collections.Generic;

public class CameraFollowTwoPlayers : MonoBehaviour
{
    [Header("Characters")]
    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Transform player4;

    [Header("Movement")]
    public float smoothSpeed = 5f;

    [Header("Zoom")]
    public float minZoom = 5f;
    public float maxZoom = 7f;
    public float zoomLimiter = 10f;

    [Header("Batas Kamera")]
    public float maxPlayerDistance = 12f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        List<Transform> activePlayers = GetActivePlayers();

        if (activePlayers.Count < 2)
            return;

        LimitPlayersDistance(activePlayers);

        Move(activePlayers);

        Zoom(activePlayers);
    }

    List<Transform> GetActivePlayers()
    {
        List<Transform> active = new List<Transform>();

        if (player1 != null && player1.gameObject.activeInHierarchy)
            active.Add(player1);

        if (player2 != null && player2.gameObject.activeInHierarchy)
            active.Add(player2);

        if (player3 != null && player3.gameObject.activeInHierarchy)
            active.Add(player3);

        if (player4 != null && player4.gameObject.activeInHierarchy)
            active.Add(player4);

        return active;
    }

    void Move(List<Transform> players)
    {
        Vector3 center = Vector3.zero;

        foreach (Transform p in players)
            center += p.position;

        center /= players.Count;

        Vector3 target =
            new Vector3(center.x, center.y, transform.position.z);

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            smoothSpeed * Time.deltaTime);
    }

    void Zoom(List<Transform> players)
    {
        float maxDistance = 0f;

        for (int i = 0; i < players.Count; i++)
        {
            for (int j = i + 1; j < players.Count; j++)
            {
                float d = Vector2.Distance(
                    players[i].position,
                    players[j].position);

                if (d > maxDistance)
                    maxDistance = d;
            }
        }

        float targetZoom =
            Mathf.Lerp(minZoom, maxZoom, maxDistance / zoomLimiter);

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            smoothSpeed * Time.deltaTime);
    }

    void LimitPlayersDistance(List<Transform> players)
    {
        if (players.Count != 2)
            return;

        float distance = Vector2.Distance(
            players[0].position,
            players[1].position);

        if (distance > maxPlayerDistance)
        {
            Vector3 center =
                (players[0].position + players[1].position) / 2f;

            Vector3 dir1 =
                (players[0].position - center).normalized;

            Vector3 dir2 =
                (players[1].position - center).normalized;

            players[0].position =
                center + dir1 * (maxPlayerDistance / 2f);

            players[1].position =
                center + dir2 * (maxPlayerDistance / 2f);
        }
    }
}