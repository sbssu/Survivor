using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSe : MonoBehaviour
{
    [SerializeField] AudioSource source;
    
    public void Play(AudioClip clip, float volumn)
    {
        gameObject.SetActive(true);
        source.clip = clip;
        source.volume = volumn;
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying)
            Destroy(gameObject);
    }
}
