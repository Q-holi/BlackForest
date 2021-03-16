using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;

    public AudioSource btnsource;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(musicsource);
        DontDestroyOnLoad(btnsource);
    }
    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }

    public void SetButtonVolume(float volume)
    {
        btnsource.volume = volume;
    }
    public void OnSfx()
    {
        btnsource.Play();
    }
}
