using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoController : MonoBehaviour
{
    [Header("Connect Settings")]
    public static InfoController infoController;

    [Header("View Settings")]
    public Image mainImage;
    public bool isActive = false;

    [Header("Timer Settings")]
    public double timer = 0f;
    public TextMeshProUGUI timerText;

    [Header("Contact Settings")]
    public long touches;
    public TextMeshProUGUI touchesText;

    public void Awake()
    {
        infoController = this;
        mainImage.gameObject.SetActive(isActive); 
        LoadData();
    }

    public void OpenClosedWindow()
    {
        isActive = !isActive;
        mainImage.gameObject.SetActive(isActive);
    }

    private void SaveTimer()
    {
        PlayerPrefs.SetFloat("timer", (float)timer);
        PlayerPrefs.SetInt("touches", (int)touches);
    }

    private void LoadData()
    {
        timer = PlayerPrefs.GetFloat("timer");
        touches = PlayerPrefs.GetInt("touches");
    }

    [ContextMenu("Remove Level")]
    public void RemoveTimer()
    {
        PlayerPrefs.SetFloat("timer", 0);
        PlayerPrefs.SetInt("touches", 0);
        
        timer = 0;
        touches = 0;
    }

    public void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        timerText.text = FormatDayTime((int)timer);
        touchesText.text = "Touches: " + touches.ToString();

        SaveTimer();
    }

    public string FormatDayTime(int totalSeconds)
    {
        int days = (totalSeconds / 3600) / 24;
        string dd = days < 10 ? "0" + days : days.ToString();
        int hours = (totalSeconds / 3600) - (days * 24);
        string hh = hours < 10 ? "0" + hours : hours.ToString();
        int minutes = (totalSeconds - (hours * 3600) - (days * 86400)) / 60;
        string mm = minutes < 10f ? "0" + minutes : minutes.ToString();
        int seconds = totalSeconds - (hours * 3600) - (minutes * 60) - (days * 86400);
        string ss = seconds < 10 ? "0" + seconds : seconds.ToString();
        return string.Format("{0}:{1}:{2}:{3}", dd, hh, mm, ss);
    }
}
