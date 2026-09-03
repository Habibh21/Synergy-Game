using UnityEngine;

public class Player1Switch : MonoBehaviour
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
        // Shift kiri atau kanan
        if (Input.GetKeyDown(KeyCode.LeftShift) ||
            Input.GetKeyDown(KeyCode.RightShift))
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        // Simpan posisi & rotasi karakter aktif
        Vector3 pos = characters[currentIndex].transform.position;
        Quaternion rot = characters[currentIndex].transform.rotation;

        // Nonaktifkan karakter sekarang
        characters[currentIndex].SetActive(false);

        // Pindah ke karakter berikutnya
        currentIndex++;

        if (currentIndex >= characters.Length)
            currentIndex = 0;

        // Karakter baru muncul di posisi karakter lama
        characters[currentIndex].transform.position = pos;
        characters[currentIndex].transform.rotation = rot;

        // Aktifkan karakter baru
        characters[currentIndex].SetActive(true);
    }
}