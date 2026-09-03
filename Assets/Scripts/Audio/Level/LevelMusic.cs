using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip levelMusic;

    void Start()
    {
        BackgroundMusic bgm = FindFirstObjectByType<BackgroundMusic>();

        if (bgm != null)
        {
            AudioSource source = bgm.GetComponent<AudioSource>();

            if (source.clip != levelMusic)
            {
                source.Stop();
                source.clip = levelMusic;
                source.Play();
            }
        }
    }
}