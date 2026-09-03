using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [Header("Nama Scene Tujuan")]
    public string nextScene = "Level 3";

    private bool loaded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loaded) return;

        if (other.CompareTag("Player"))
        {
            loaded = true;

            // Unlock Level 3
            if (nextScene == "Level 3")
            {
                PlayerPrefs.SetInt("Level3Unlocked", 1);
                PlayerPrefs.Save();
            }

            SceneManager.LoadScene(nextScene);
        }
    }
}