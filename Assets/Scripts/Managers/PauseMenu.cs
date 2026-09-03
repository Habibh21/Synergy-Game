using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Tujuan")]
    [SerializeField] private string levelMenuScene = "Level Menu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToLevelMenu();
        }
    }

    public void BackToLevelMenu()
    {
        // Mengembalikan timeScale jika sebelumnya pernah di-pause
        Time.timeScale = 1f;

        SceneManager.LoadScene(levelMenuScene);
    }
}