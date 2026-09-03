using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject[] characters;
    public int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == currentIndex);
        }
    }

    void Update()
    {
        // Tab Keyboard atau RB Controller
        if (Input.GetKeyDown(KeyCode.Tab) ||
            Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        Vector3 pos = characters[currentIndex].transform.position;
        Quaternion rot = characters[currentIndex].transform.rotation;

        characters[currentIndex].SetActive(false);

        currentIndex++;
        if (currentIndex >= characters.Length)
            currentIndex = 0;

        characters[currentIndex].transform.position = pos;
        characters[currentIndex].transform.rotation = rot;

        characters[currentIndex].SetActive(true);
    }
}