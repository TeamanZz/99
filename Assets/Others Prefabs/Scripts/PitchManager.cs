using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchManager : MonoBehaviour
{
    public static PitchManager pitchManager;
    public SFX sfx;
    
    [Header("Sound Settings")]
    [SerializeField] private float minPitch = 1;
    [SerializeField] private float maxPitch = 3;

    [SerializeField] private float pitchCoefficient = 0.1f;

    [Header("Connext Settings")]
    public Vector2Int lastPlateIndex = new Vector2Int(-1, -1);

    public void Awake()
    {
        pitchManager = this;
    }

    public void CheckPlate(Vector2Int currentPlateIndex)
    {
        if(currentPlateIndex != lastPlateIndex)
        {
            lastPlateIndex = currentPlateIndex;
            sfx.source.pitch = minPitch;
        }
        else
            sfx.source.pitch = Mathf.Clamp(sfx.source.pitch + pitchCoefficient, minPitch, maxPitch);
    }

    public void IndexReset()
    {
        sfx.source.pitch = 1; 
        lastPlateIndex = new Vector2Int(-1, -1);
    }
}
