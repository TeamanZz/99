using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [Header("Conect Settings")]
    public PlatesSpawner spawner;

    [Header("Prefabs Settings")]
    public List<Plate> platesPrefabs = new List<Plate>();
    public List<int> platesLevels = new List<int>();

    [Header("Levels Manager")]
    public int currentLevel;

    [Header("View Settings")]
    public TextMeshProUGUI leftBarText;
    public TextMeshProUGUI rightBarText;
    public Image levelFillImage;


    private void Awake()
    {
        LoadLevel();
        FirstLevelInitialization();
    }

    private void FirstLevelInitialization()
    {
        if (currentLevel >= 3)
            AddPlate();

        if (leftBarText != null)
            leftBarText.text = Mathf.Clamp((currentLevel - 1), 1, 999).ToString();

        if (currentLevel >= 8)
        {
            Knife.knife.isOpen = true;
            Knife.knife.CheckState();
        }

        if (currentLevel >= 12)
            AddPlate();

        if (currentLevel >= 15)
            AddPlate();

        if (currentLevel >= 20)
            AddPlate();
    }


    public void UpdateBottomBar()
    {
        leftBarText.text = (currentLevel - 1).ToString();
        rightBarText.text = (currentLevel).ToString();
        float coefficient = 1f / 40;
        int fillCount = 40 - spawner.currentTopPlates.Count; 
        levelFillImage.DOFillAmount(coefficient * fillCount, 0.1f);
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    private void LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentLevel -= 1;
        currentLevel = Mathf.Clamp(currentLevel, 1, 999);
    }

    [ContextMenu("Remove Level")]
    public void RemoveLevel()
    {
        PlayerPrefs.SetInt("currentLevel", 2);
    }

    private void Start()
    {
        spawner.StartWorking();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && platesPrefabs.Count > 0)
        {
            Debug.Log("Add Plate");
            AddPlate();
        }

        UpdateBottomBar();
    }

    public void AddLevel()
    {
        currentLevel++;
        SaveLevel();

        if (leftBarText != null)
            leftBarText.text = Mathf.Clamp((currentLevel - 1), 1, 999).ToString();

        if (currentLevel == 3)
            AddPlate();

        if (currentLevel == 8)
        {
            Knife.knife.isOpen = true;
            Knife.knife.CheckState();
        }

        if (currentLevel == 12)
            AddPlate();

        if (currentLevel == 15)
            AddPlate();

        if (currentLevel == 20)
            AddPlate();
    }

    private void AddPlate()
    {
        spawner.platePrefab.Add(platesPrefabs[0].gameObject);
        platesPrefabs.RemoveAt(0);
    }
}
