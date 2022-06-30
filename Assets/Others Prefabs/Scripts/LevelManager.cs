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
    [SerializeField] private bool isInitialization = false;

    [Header("Prefabs Settings")]
    public List<Plate> platesPrefabs = new List<Plate>();
    public Plate nextPlate;

    [Header("Levels Manager")]
    public int currentLevel;

    [Header("View Bar Settings")]
    [Space]
    [Header("Bar")]
    public GameObject fillViewGroup;
    public Image fillImage;
    public int saveIntState = 0;
    public float coefficient;

    [Space]
    [Header("View Plate")]
    public GameObject viewPlateGroup;
    public Image viewPlateImage;
    public TextMeshProUGUI viewPlateInfo;

    public Image leftView;

    [Space]
    [Header("View Callback Settings")]
    [SerializeField] private bool callBackIsActive = false;
    [SerializeField] private float callbackTime = 0.35f;

    [Space]
    [Header("Levels View")]
    public Vector2Int levels;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI levelText;

    [Header("Main Settings")]
    public int defaultDamage = 1;
    public int knifeDamage = 1;

    public int allDamageValue = 0;

    private void Awake()
    {
        isInitialization = false;

        levelManager = this;
        viewPlateGroup.SetActive(false);

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

        if (currentLevel >= 30)
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
        isInitialization = true;
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
        levelText.text = (currentLevel - 1).ToString();
        if (callBackIsActive != true)
        {
            if (currentLevel > 30)
            {
                fillViewGroup.SetActive(false);
                nextPlate = null;
                return;
            }
            else
                fillViewGroup.SetActive(true);
        }

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

        if (currentLevel - 1 < 30)
        {
            BarProcessing(20, 30);
            return;
        }
    }

    public void BarProcessing(int startLevel, int endLevel)
    {
        levels = new Vector2Int(startLevel, endLevel);
        Debug.Log($"Bar Levels {levels}");

        coefficient = 1f / ((levels.y - levels.x) * 40);
        fillImage.fillAmount = Mathf.Clamp(((saveIntState * coefficient * 40) + (40 - spawner.currentTopPlates.Count) * coefficient), 0, 99);
        Debug.Log($"Bar Fill Value {fillImage.fillAmount}; Fill Coefficient {coefficient}");

        valueText.text = Mathf.RoundToInt(fillImage.fillAmount * 100).ToString() + "%";

        leftView.sprite = platesPrefabs[0].image;
    }

    public void UpdateBottomBar()
    {
        if (callBackIsActive == true)
            return;

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
        //fillImage.fillAmount = 0;

        coefficient = 0;
        saveIntState = 0;

        OpenViewPanel();
        InitializationBar();
    }

    public void OpenViewPanel()
    {
        if (nextPlate == null || nextPlate.plateIsActive == false)
            return;

        StartCoroutine(StartCallback());

        fillViewGroup.SetActive(false);
        viewPlateGroup.SetActive(true);
        valueText.gameObject.SetActive(false);

        string endString = "Congratulations!\n You opened ";
        viewPlateInfo.text = endString + nextPlate.info.ToString();

        viewPlateImage.sprite = nextPlate.image;
        fillImage.fillAmount = 1f;

        // nextPlate = null;
    }

    public IEnumerator StartCallback()
    {
        callBackIsActive = true;
        yield return new WaitForSeconds(callbackTime);
        callBackIsActive = false;
    }

    public void ClosedViewPanel()
    {
        if (callBackIsActive == true)
            return;

        fillImage.fillAmount = 0;

        viewPlateGroup.SetActive(false);
        valueText.gameObject.SetActive(true);

        if(platesPrefabs.Count > 1)
        leftView.sprite = platesPrefabs[0].image;

        UpdateBottomBar();
        InitializationBar();
    }

    [ContextMenu("Add Level")]
    public void AddLevel()
    {
        currentLevel++;
        saveIntState++;
        SaveLevel();

        Invoke(nameof(CheckBar), 0.05f);

        levelText.text = (currentLevel - 1).ToString();

        if (currentLevel == 3)
            AddPlate();

        if (currentLevel == 8)
            AddPlate();

        if (currentLevel == 12)
            AddPlate();

        if (currentLevel == 15)
            AddPlate();

        if (currentLevel == 20)
            AddPlate();

        if (currentLevel == 30)
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
        //Plate newPlate = new Plate();;
        //nextPlate = newPlate;

        nextPlate.plateIsActive = isInitialization;

        nextPlate.image = platesPrefabs[0].image;
        nextPlate.info = platesPrefabs[0].info;

        if (isInitialization == true && platesPrefabs.Count > 0)
            leftView.sprite = platesPrefabs[0].image;

        Debug.Log($"Instatiate New plate {nextPlate} Is Active - {nextPlate.plateIsActive} | Info - {nextPlate.image} && {nextPlate.info}");

        spawner.platePrefab.Add(platesPrefabs[0].gameObject);
        platesPrefabs.RemoveAt(0);
    }
}
