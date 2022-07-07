using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}