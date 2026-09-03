using UnityEngine;
using System.Collections;

public class PlatformLoop : MonoBehaviour
{
    [Header("Platform")]
    public GameObject[] platforms;

    [Header("Timing")]
    public float startDelay = 1f;
    public float delayEachPlatform = 0.25f;

    public float blinkDuration = 0.5f;
    public float hiddenTime = 1f;

    void Start()
    {
        StartCoroutine(PlatformRoutine());
    }

    IEnumerator PlatformRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(startDelay);

            // Hilang satu-persatu
            for (int i = 0; i < platforms.Length; i++)
            {
                yield return StartCoroutine(BlinkPlatform(platforms[i]));

                platforms[i].SetActive(false);

                yield return new WaitForSeconds(delayEachPlatform);
            }

            // Semua hilang
            yield return new WaitForSeconds(hiddenTime);

            // Muncul lagi satu-persatu
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].SetActive(true);

                SpriteRenderer sr = platforms[i].GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = 0;
                    sr.color = c;

                    float t = 0;

                    while (t < 0.3f)
                    {
                        t += Time.deltaTime;

                        c.a = Mathf.Lerp(0, 1, t / 0.3f);
                        sr.color = c;

                        yield return null;
                    }

                    c.a = 1;
                    sr.color = c;
                }

                yield return new WaitForSeconds(delayEachPlatform);
            }
        }
    }

    IEnumerator BlinkPlatform(GameObject platform)
    {
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();

        if (sr == null)
            yield break;

        float timer = 0f;

        while (timer < blinkDuration)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.08f);

            sr.enabled = true;
            yield return new WaitForSeconds(0.08f);

            timer += 0.16f;
        }

        sr.enabled = true;
    }
}