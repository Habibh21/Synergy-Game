using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip menuMusic;

    void Start()
    {
        BackgroundMusic bgm = FindFirstObjectByType<BackgroundMusic>();

        if (bgm != null)
        {
            AudioSource source = bgm.GetComponent<AudioSource>();

            if (source.clip != menuMusic)
            {
                source.Stop();
                source.clip = menuMusic;
                source.Play();
            }
        }
    }
}