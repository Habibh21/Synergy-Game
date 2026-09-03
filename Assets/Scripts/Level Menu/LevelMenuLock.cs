using UnityEngine;
using UnityEngine.UI;

public class LevelMenuLock : MonoBehaviour
{
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        bool level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
        bool level3Unlocked = PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;

        level2Button.interactable = level2Unlocked;
        level3Button.interactable = level3Unlocked;
    }
}