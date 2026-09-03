using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPortal : MonoBehaviour
{
    [Header("Pengaturan Level")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private float delayBeforeLoad = 2.5f;

    [Header("Nama Tombol Batal (Input Manager)")]
    [SerializeField] private string cancelInputButton = "Cancel";

    [Header("UI Notifikasi")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float warningDuration = 2f;

    private Coroutine warningCoroutine;

    private bool skeletonInside = false;
    private bool fireInside = false;

    private GameObject savedSkeleton;
    private GameObject savedFire;

    private Coroutine portalCoroutine;
    private bool isCountingDown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ==========================
        // CEK APAKAH SEMUA KOIN SUDAH DIAMBIL
        // ==========================
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager tidak ditemukan!");
            return;
        }

        if (!ScoreManager.Instance.HasAllCoins())
        {
            ShowWarning("⚠ Portal masih terkunci!\nKumpulkan semua koin terlebih dahulu.");
            return;
        }

        // ==========================
        // PLAYER 1 (Skeleton / Golem)
        // ==========================
        PlayerMovement skeleton = collision.GetComponent<PlayerMovement>();
        GolemBlue golem = collision.GetComponent<GolemBlue>();

        if ((skeleton != null || golem != null) && !skeletonInside)
        {
        skeletonInside = true;
        savedSkeleton = collision.gameObject;

        SembunyikanPlayer(savedSkeleton);

        Debug.Log("Player 1 masuk portal.");
        }

        // ==========================
        // PLAYER 2 (Api / Skeleton)
        // ==========================
        api fire = collision.GetComponent<api>();
        Skeleton1 skeleton2 = collision.GetComponent<Skeleton1>();

        if ((fire != null || skeleton2 != null) && !fireInside)
        {
         fireInside = true;
            savedFire = collision.gameObject;

         SembunyikanPlayer(savedFire);

         Debug.Log("Player 2 masuk portal.");
        }

        // ==========================
        // Jika kedua player sudah masuk
        // ==========================
        if (skeletonInside && fireInside && !isCountingDown)
        {
            portalCoroutine = StartCoroutine(PindahLevelSequence());
        }
    }

    private void Update()
    {
        if ((skeletonInside || fireInside) &&
            Input.GetButtonDown(cancelInputButton))
        {
            CancelPortalSequence();
        }
    }

    IEnumerator PindahLevelSequence()
    {
        isCountingDown = true;

        Debug.Log("Semua player masuk portal!");

        yield return new WaitForSeconds(delayBeforeLoad);

       if (!string.IsNullOrEmpty(nextLevelName))
{
    // ==========================
    // Unlock level berikutnya
    // ==========================

    if (nextLevelName == "Level 2")
    {
        PlayerPrefs.SetInt("Level2Unlocked", 1);
    }
    else if (nextLevelName == "Level 3")
    {
        PlayerPrefs.SetInt("Level3Unlocked", 1);
    }

     PlayerPrefs.Save();

        SceneManager.LoadScene(nextLevelName);
    }
    else
    {
        Debug.LogWarning("Nama scene tujuan belum diisi!");
    }
    }

    void CancelPortalSequence()
    {
        if (portalCoroutine != null)
        {
            StopCoroutine(portalCoroutine);
            portalCoroutine = null;
        }

        isCountingDown = false;

        if (skeletonInside && savedSkeleton != null)
        {
            TampilkanPlayer(savedSkeleton);
            skeletonInside = false;
        }

        if (fireInside && savedFire != null)
        {
            TampilkanPlayer(savedFire);
            fireInside = false;
        }

        Debug.Log("Teleport dibatalkan.");
    }

    void SembunyikanPlayer(GameObject player)
    {
        // Pindahkan ke tengah portal
        player.transform.position = transform.position + new Vector3(0f, -0.3f, 0f);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
            col.enabled = false;

        SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
            sr.enabled = false;
    }

    void TampilkanPlayer(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
            col.enabled = true;

        SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
            sr.enabled = true;

        // Keluar sedikit dari portal
        player.transform.position = transform.position + new Vector3(-1.5f, 0.2f, 0f);

        PlayerMovement skeleton = player.GetComponent<PlayerMovement>();

        if (skeleton != null)
            skeleton.ResetStatusAfterPortal();

        api fire = player.GetComponent<api>();

        if (fire != null)
            fire.ResetStatusAfterPortal();
    }

    void ShowWarning(string message)
    {
        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(WarningRoutine(message));
    }

    IEnumerator WarningRoutine(string message)
    {
        if (warningPanel != null)
            warningPanel.SetActive(true);

        if (warningText != null)
            warningText.text = message;

        yield return new WaitForSeconds(warningDuration);

        if (warningPanel != null)
            warningPanel.SetActive(false);
    }
}