using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsHandler : MonoBehaviour
{
    public static ColorsHandler Instance;

    [SerializeField] private Color emptyPlateColor;
    [SerializeField] private List<Color> platesColors = new List<Color>();
    [SerializeField] private List<Color> textColors = new List<Color>();

    private void Awake()
    {
        Instance = this;
    }

    public Color GetLerpedColor(int value)
    {
        var truncatedNumber = (int)Math.Truncate((double)(value / 6));

        if (NeedSpawnRandomColorPlate(truncatedNumber))
        {
            var num = (int)truncatedNumber + 1 - platesColors.Count + 1;
            for (int i = 0; i < num; i++)
                AddNewColorToArray();
        }

        float minValue = (float)truncatedNumber * 6;
        float maxValue = (float)(truncatedNumber + 1) * 6;

        var tval = ((float)value - minValue) / (maxValue - minValue);
        Color lerpedColor = Color.Lerp(platesColors[truncatedNumber], platesColors[truncatedNumber + 1], tval);

        return lerpedColor;
    }

    private bool NeedSpawnRandomColorPlate(double truncatedNumber)
    {
        return (int)truncatedNumber + 1 >= platesColors.Count;
    }

    private void AddNewColorToArray()
    {
        platesColors.Add(platesColors[UnityEngine.Random.Range(0, platesColors.Count)]);
        textColors.Add(textColors[UnityEngine.Random.Range(0, textColors.Count)]);
    }

    private Color GetRandomPlateColor()
    {
        return platesColors[UnityEngine.Random.Range(1, platesColors.Count)];
    }

    public Color GetTextColor(int value)
    {
        var truncatedNumber = (int)Math.Truncate((double)(value / 6));

        float minValue = (float)truncatedNumber * 6;
        float maxValue = (float)(truncatedNumber + 1) * 6;

        var tval = ((float)value - minValue) / (maxValue - minValue);
        Color lerpedColor = Color.Lerp(textColors[truncatedNumber], textColors[truncatedNumber + 1], tval);

        return lerpedColor;
    }

    public Color GetDefaultColor()
    {
        return emptyPlateColor;
    }
}