using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatesSpawner : MonoBehaviour
{
    [Header("Connect Settings")]
    public static PlatesSpawner Instance;
    public LevelManager levelManager;

    [Header("INstatiate Settings")]
    public List<GameObject> platePrefab;
    [SerializeField] private GameObject firstPlatesParent;
    [SerializeField]  private GameObject secondPlatesParent;
    public GameObject currentTopParent;

    //[SerializeField] private int generalValue = 0;

    [SerializeField] private float startPlateXPos;
    [SerializeField] private float startPlateZPos;

    [SerializeField] private bool needSpawnToFirstParent = true;

    [Header("Grid Controller")]
    public Vector2Int gridSize = new Vector2Int(5, 8);

    public List<Plate> currentCordPlates = new List<Plate>();
    [SerializeField] public List<Plate> currentTopPlates = new List<Plate>();
    [SerializeField] private List<Plate> firstContainerElements = new List<Plate>();
    [SerializeField] private List<Plate> secondContainerElements = new List<Plate>();

    private float lastPlateXPos;
    private float lastPlateZPos;

    private Vector2Int grid = new Vector2Int(0, 0);

    [Header("Plates Settings")]
    public List<SelectedPlate> currentRemoteController = new List<SelectedPlate>();
    public List<SelectedPlate> currentReceiver = new List<SelectedPlate>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartWorking()
    {
        SpawnStartPlates();
    }

    public void SpawnNewPlatesField()
    {
        levelManager.AddLevel();
        //levelManager.currentLevel++;
        //levelManager.SaveLevel();

        //generalValue++;
        ResetLastSpawnPoints();
        for (var i = 0; i < 40; i++)
        {
            SpawnPlatesFieldByGeneralValue();
        }

        needSpawnToFirstParent = !needSpawnToFirstParent;
    }

    public void SpawnPlatesFieldByGeneralValue()
    {
        GameObject newPlate;
        Plate plateComponent;
        Vector3 newPlatePosition = new Vector3(lastPlateXPos, 0, lastPlateZPos);

        int number = Random.Range(-8, platePrefab.Count + 1);
        if (platePrefab.Count > 1)
        {
            number = Mathf.Clamp(number, 0, platePrefab.Count - 1);
        }
        else
            number = 0;

        Debug.Log("Number " + number);

        if (needSpawnToFirstParent)
        {
            newPlate = Instantiate(platePrefab[number], newPlatePosition, Quaternion.Euler(270, 0, 0), firstPlatesParent.transform);
            newPlate.transform.localPosition = new Vector3(newPlate.transform.localPosition.x, 0, newPlate.transform.localPosition.z);
            plateComponent = newPlate.GetComponent<Plate>();
            firstContainerElements.Add(plateComponent);
        }
        else
        {
            newPlate = Instantiate(platePrefab[number], newPlatePosition, Quaternion.Euler(270, 0, 0), secondPlatesParent.transform);
            newPlate.transform.localPosition = new Vector3(newPlate.transform.localPosition.x, 0, newPlate.transform.localPosition.z);
            plateComponent = newPlate.GetComponent<Plate>();
            secondContainerElements.Add(plateComponent);
        }

        //  grid
        plateComponent.currentIndex = grid;

        Debug.Log($"Current Level{levelManager.currentLevel}");
        plateComponent.SetNewNonZeroValue(levelManager.currentLevel);
        //plateComponent.SetNewNonZeroValue(generalValue);
        CalculateNewPlatePosition();
    }

    public void DecreasePlatesCount(Plate plate)
    {
        currentTopPlates.Remove(plate);
        Destroy(plate.gameObject);

        if (currentTopPlates.Count == 0)
        {
            currentCordPlates.Clear();
            currentTopPlates.Clear();

            if(PitchManager.pitchManager!=null)
                PitchManager.pitchManager.IndexReset();

            SwapParentsPositions();
            ReinitializeTopObjects();
            HandleNewTopPlatesOnSwap();
            StartCoroutine(SpawnPlatesAfterDelay());
        }

        levelManager.UpdateBottomBar();
    }

    private void HandleNewTopPlatesOnSwap()
    {
        for (int i = 0; i < 40; i++)
        {
            if (currentTopParent.transform.GetChild(i) != null)
            {
                Transform topParentChild = currentTopParent.transform.GetChild(i);
                topParentChild.DOShakeScale(1, 0.1f, 10, 30, true).SetEase(Ease.InOutBack);
                topParentChild.GetComponent<BoxCollider>().enabled = true;
                topParentChild.GetComponent<Plate>().plateIsActive = true;
            }
        }
    }

    private void SwapParentsPositions()
    {
        Vector3 firstParentPosition = firstPlatesParent.transform.position;
        Vector3 secondParentPosition = secondPlatesParent.transform.position;

        if (currentTopParent == firstPlatesParent)
        {
            firstPlatesParent.transform.position = secondParentPosition;
            secondPlatesParent.transform.DOMove(firstParentPosition, 2);
        }
        else
        {
            secondPlatesParent.transform.position = firstParentPosition;
            firstPlatesParent.transform.DOMove(secondParentPosition, 2);
        }
    }

    [ContextMenu("Destroy random")]
    public void DestroyRandomPlate()
    {
        Vector2Int position = new Vector2Int(Random.Range(0, gridSize.x - 1), Random.Range(0, gridSize.y - 1));
        int number = position.x * 5 + position.y + 1;
        Debug.Log(currentCordPlates[number]);

        if (currentCordPlates[number].gameObject != null)
            Destroy(currentCordPlates[number].gameObject);
    }

    private void ReinitializeTopObjects()
    {
        if (currentTopParent == firstPlatesParent)
        {
            grid = Vector2Int.zero;
            currentTopParent = secondPlatesParent;
            currentTopPlates = secondContainerElements;
        }
        else
        {
            grid = Vector2Int.zero;
            currentTopParent = firstPlatesParent;
            currentTopPlates = firstContainerElements;
        }

        currentCordPlates.AddRange(currentTopPlates);

        //selectedPlatesManager.FindElements(currentTopPlates);
    }

    private IEnumerator SpawnPlatesAfterDelay()
    {
        yield return new WaitForSeconds(0);
        SpawnNewPlatesField();
    }

    private void ResetLastSpawnPoints()
    {
        lastPlateXPos = startPlateXPos;
        lastPlateZPos = startPlateZPos;
    }

    public void CalculateNewPlatePosition()
    {
        lastPlateXPos += 2.5f;

        //  grid
        grid.x += 1;

        if (lastPlateXPos == 7.5f)
        {
            //  grid
            grid.x = 0;
            grid.y += 1;

            if (grid.y == 8)
                grid.y = 0;

            lastPlateZPos += 2.5f;
            lastPlateXPos = startPlateXPos;
        }
    }

    public SelectedPlatesManager selectedPlatesManager;
    private void SpawnStartPlates()
    {
        ResetLastSpawnPoints();
        for (var i = 0; i < 40; i++)
        {
            SpawnPlatesFieldByGeneralValue();
            firstPlatesParent.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            firstPlatesParent.transform.GetChild(i).GetComponent<Plate>().plateIsActive = true;
        }

        currentTopPlates = firstContainerElements;
        currentCordPlates.AddRange(currentTopPlates);

        //selectedPlatesManager.FindElements(currentTopPlates);

        needSpawnToFirstParent = !needSpawnToFirstParent;
        SpawnNewPlatesField();
        currentTopParent = firstPlatesParent;
    }
}