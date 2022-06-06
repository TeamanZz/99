using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance;

    public List<AudioClip> soundsList = new List<AudioClip>();
    public List<AudioClip> soundsBlockDestroyList = new List<AudioClip>();
    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        source.PlayOneShot(soundsList[index]);
    }

    public void PlayPlateDestroySound()
    {
        source.PlayOneShot(soundsBlockDestroyList[Random.Range(0, soundsBlockDestroyList.Count)]);
    }
}