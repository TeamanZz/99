using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance;

    public List<AudioClip> damageSoundList = new List<AudioClip>();
    public List<AudioClip> nonDamageList = new List<AudioClip>();

    public List<AudioClip> destroySoundList = new List<AudioClip>();

    public AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayDamageSound(int index)
    {
        if (index > damageSoundList.Count || damageSoundList.Count == 0 || damageSoundList[index] == null)
        {
            Debug.Log($"Index {index} Not Found");
            return;
        }

        Debug.Log($"Damage Sound Index {index}");
        
        source.PlayOneShot(damageSoundList[index]);
    }
    public void PlayNonDamageSound(int index)
    {
        if (index > nonDamageList.Count || nonDamageList.Count == 0 || nonDamageList[index] == null)
        {
            Debug.Log($"Index {index} Not Found");
            return;
        }

        Debug.Log($"Damage Sound Index {index}");

        source.PlayOneShot(nonDamageList[index]);
    }

    public void PlayDestroySound(int index)
    {
        if (index > destroySoundList.Count || destroySoundList.Count == 0 || destroySoundList[index] == null)
        {
            Debug.Log($"Index {index} Not Found");
            return;
        }

        source.PlayOneShot(destroySoundList[index]);
    }
}