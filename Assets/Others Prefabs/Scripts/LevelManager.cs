using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Conect Settings")]
    public PlatesSpawner spawner;

    [Header("Prefabs Settings")]
    public List<Plate> platesPrefabs = new List<Plate>();

    [Header("Levels Manager")]
    public int currentLevel;

    [Header("View Settings")]
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        LoadLevel();
        FirstLevelInitialization();
    }

    private void FirstLevelInitialization()
    {
        if (currentLevel >= 3)
            AddPlate();

        if (levelText != null)
            levelText.text = Mathf.Clamp((currentLevel - 1), 1, 999).ToString();

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

    private void SaveLevel()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    private void LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentLevel -= 1;
    }

    [ContextMenu("Remove Level")]
    public void RemoveLevel()
    {
        PlayerPrefs.SetInt("currentLevel", 1);
    }

    private void Start()
    {
        spawner.StartWorking();    
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && platesPrefabs.Count > 0)
        {
            Debug.Log("Add Plate");
            AddPlate();
        }
    }

    public void AddLevel()
    {
        currentLevel++;
        SaveLevel();

        if (levelText != null) 
            levelText.text = Mathf.Clamp((currentLevel - 1), 1, 999).ToString();

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
