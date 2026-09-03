using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLevelMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Level Menu");
    }
}