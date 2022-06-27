using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    
    [Header("Conect Settings")]
    public PlatesSpawner spawner;

    [Header("Prefabs Settings")]
    public List<Plate> platesPrefabs = new List<Plate>();

    [Header("Levels Manager")]
    public int currentLevel;

    [Header("View Bar Settings")]
    public Image fillImage;
    public int saveIntState = 0;
    public float coefficient;

    public Vector2Int levels;
    public TextMeshProUGUI valueText;

    [Header("Main Settings")]
    public int defaultDamage = 1;
    public int knifeDamage = 1;

    public int allDamageValue = 0;

    private void Awake()
    {
        levelManager = this;
        LoadLevel();
        FirstLevelInitialization();
    }

    private void FirstLevelInitialization()
    {
        if (currentLevel >= 3)
            AddPlate();

        if (currentLevel >= 8)
            AddPlate();

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
        PlayerPrefs.SetInt("saveIntState", saveIntState);
    }

    private void LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentLevel -= 1;
        currentLevel = Mathf.Clamp(currentLevel, 1, 999);

        saveIntState = PlayerPrefs.GetInt("saveIntState");
        saveIntState -= 1;
        
        InitializationBar();
    }

    [ContextMenu("Remove Level")]
    public void RemoveLevel()
    {
        PlayerPrefs.SetInt("currentLevel", 2);
        PlayerPrefs.SetInt("saveIntState", 0);
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
    }

    public void InitializationBar()
    {
        if (currentLevel > 20)
        {
            fillImage.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
            fillImage.transform.parent.gameObject.SetActive(true);


        if (currentLevel < 3)
        {
            BarProcessing(1, 3);
            return;
        }

        if (currentLevel - 1 < 8)
        {
            BarProcessing(3, 8);
            return;
        }

        if (currentLevel - 1 < 12)
        {
            BarProcessing(8, 12);
            return;
        }

        if (currentLevel - 1 < 15)
        {
            BarProcessing(12, 15);
            return;
        }

        if (currentLevel - 1 < 20)
        {
            BarProcessing(15, 20);
            return;
        }
    }

    public void BarProcessing(int startLevel, int endLevel)
    {
        levels = new Vector2Int(startLevel, endLevel);
        Debug.Log($"Bar Levels {levels}");

        coefficient = 1f / ((levels.y - levels.x) * 40);
        fillImage.fillAmount = (saveIntState * coefficient * 40) + (40 - spawner.currentTopPlates.Count) * coefficient;
        Debug.Log($"Bar Fill Value {fillImage.fillAmount}; Fill Coefficient {coefficient}");

        valueText.text = Mathf.RoundToInt(fillImage.fillAmount * 100).ToString() + "%";
    }

    public void UpdateBottomBar()
    {
        float fillValue = (saveIntState * coefficient * 40) + (40 - spawner.currentTopPlates.Count) * coefficient;
        fillImage.DOFillAmount(fillValue, 0.1f);
        Debug.Log($"Bar Fill Value {fillValue}");

        valueText.text = Mathf.RoundToInt(fillImage.fillAmount * 100).ToString() + "%";
    }

    public void CheckBarToEnd()
    {
        if (currentLevel - 1 < levels.y)
            return;

        Debug.Log("End Levels");
        coefficient = 0;
        saveIntState = 0;
        fillImage.fillAmount = 0;

        InitializationBar();
    }

    [ContextMenu("Add Level")]
    public void AddLevel()
    {
        currentLevel++;
        saveIntState++;
        SaveLevel();

        Invoke(nameof(CheckBar), 0.05f);

        if (currentLevel - 1 == 3)
            AddPlate();

        if (currentLevel - 1== 8)
        {
            AddPlate();
        }

        if (currentLevel - 1 == 12)
            AddPlate();

        if (currentLevel - 1 == 15)
            AddPlate();

        if (currentLevel - 1 == 20)
            AddPlate();
    }

    public void CheckBar()
    {
        CheckBarToEnd();
        UpdateBottomBar(); 
        
        float fillValue = (saveIntState * coefficient * 40) + (40 - spawner.currentTopPlates.Count) * coefficient;
        Debug.Log("Fill Check Value: " + fillValue);
        fillImage.fillAmount = fillValue; 
        valueText.text = Mathf.RoundToInt(fillImage.fillAmount * 100).ToString() + "%";

        SaveLevel();
    }

    private void AddPlate()
    {
        spawner.platePrefab.Add(platesPrefabs[0].gameObject);
        platesPrefabs.RemoveAt(0);
    }
}
