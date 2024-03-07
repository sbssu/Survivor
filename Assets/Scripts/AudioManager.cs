using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AUDIO_STATE
{
    PLAY,
    PAUSE,
    UNPAUSE,
    STOP,
}
public class AudioManager : Singleton<AudioManager>
{

    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSe sePrefab;
    [SerializeField] AudioClip[] clips;

    public void PlayBGM(AUDIO_STATE state = 0)
    {
        switch (state)
        {
            case AUDIO_STATE.PLAY:
                bgmSource.Play();
                break;
            case AUDIO_STATE.PAUSE:
                bgmSource.Pause();
                break;
            case AUDIO_STATE.UNPAUSE:
                bgmSource.UnPause();
                break;
            case AUDIO_STATE.STOP:
                bgmSource.Stop();
                break;
        }
    }
    public void PlaySe(string name, float volumn = 1.0f)
    {
        AudioClip clip = System.Array.Find(clips, clip => clip.name.ToLower() == name.ToLower());
        if (clip != null)
            Instantiate(sePrefab, transform).Play(clip, volumn);
    }

}
