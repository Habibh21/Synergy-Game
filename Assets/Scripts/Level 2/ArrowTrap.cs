using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour
{
    [Header("Daftar Panah")]
    public Rigidbody2D[] arrows;

    [Header("Timing")]
    public float startDelay = 1f;
    public float delayEachArrow = 0.25f;
    public float respawnDelay = 2f;

    private Vector3[] startPos;
    private Quaternion[] startRot;

    void Start()
    {
        startPos = new Vector3[arrows.Length];
        startRot = new Quaternion[arrows.Length];

        for (int i = 0; i < arrows.Length; i++)
        {
            startPos[i] = arrows[i].transform.position;
            startRot[i] = arrows[i].transform.rotation;

            // Awalnya tidak jatuh
            arrows[i].bodyType = RigidbodyType2D.Kinematic;
        }

        StartCoroutine(ArrowLoop());
    }

    IEnumerator ArrowLoop()
    {
        while (true)
        {
            // Tunggu sebelum mulai
            yield return new WaitForSeconds(startDelay);

            // Panah jatuh satu per satu
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].bodyType = RigidbodyType2D.Dynamic;
                yield return new WaitForSeconds(delayEachArrow);
            }

            // Tunggu sampai semua jatuh
            yield return new WaitForSeconds(respawnDelay);

            // Respawn semua panah
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].bodyType = RigidbodyType2D.Kinematic;

                arrows[i].velocity = Vector2.zero;
                arrows[i].angularVelocity = 0f;

                arrows[i].transform.position = startPos[i];
                arrows[i].transform.rotation = startRot[i];
            }
        }
    }
}