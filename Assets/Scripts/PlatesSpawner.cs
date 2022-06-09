using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatesSpawner : MonoBehaviour
{
    public static PlatesSpawner Instance;

    [SerializeField] private GameObject platePrefab;
    [SerializeField] private GameObject firstPlatesParent;
    [SerializeField] private GameObject secondPlatesParent;
    [SerializeField] private GameObject currentTopParent;

    [SerializeField] private int generalValue = 0;

    [SerializeField] private float startPlateXPos;
    [SerializeField] private float startPlateZPos;

    [SerializeField] private bool needSpawnToFirstParent = true;

    [SerializeField] private List<Plate> currentTopPlates = new List<Plate>();
    [SerializeField] private List<Plate> firstContainerElements = new List<Plate>();
    [SerializeField] private List<Plate> secondContainerElements = new List<Plate>();
    
    private float lastPlateXPos;
    private float lastPlateZPos;

    private Vector2Int cellPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnStartPlates();
    }

    public void SpawnNewPlatesField()
    {
        generalValue++;
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
        Debug.Log("X pos " + lastPlateXPos + " | Z pos " + lastPlateZPos);

        if (needSpawnToFirstParent)
        {
            newPlate = Instantiate(platePrefab, newPlatePosition, Quaternion.Euler(270, 0, 0), firstPlatesParent.transform);
            newPlate.transform.localPosition = new Vector3(newPlate.transform.localPosition.x, 0, newPlate.transform.localPosition.z);
            plateComponent = newPlate.GetComponent<Plate>();
            firstContainerElements.Add(plateComponent);
        }
        else
        {
            newPlate = Instantiate(platePrefab, newPlatePosition, Quaternion.Euler(270, 0, 0), secondPlatesParent.transform);
            newPlate.transform.localPosition = new Vector3(newPlate.transform.localPosition.x, 0, newPlate.transform.localPosition.z);
            plateComponent = newPlate.GetComponent<Plate>();
            secondContainerElements.Add(plateComponent);
        }

        plateComponent.SetNewNonZeroValue(generalValue);
        CalculateNewPlatePosition();
    }

    public void DecreasePlatesCount(Plate plate)
    {
        currentTopPlates.Remove(plate);
        Destroy(plate.gameObject);

        if (currentTopPlates.Count == 0)
        {
            SwapParentsPositions();
            ReinitializeTopObjects();
            HandleNewTopPlatesOnSwap();
            StartCoroutine(SpawnPlatesAfterDelay());
        }
    }

    private void HandleNewTopPlatesOnSwap()
    {
        for (int i = 0; i < 40; i++)
        {
            Transform topParentChild = currentTopParent.transform.GetChild(i);
            topParentChild.DOShakeScale(1, 0.1f, 10, 30, true).SetEase(Ease.InOutBack);
            topParentChild.GetComponent<BoxCollider>().enabled = true;
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

    private void ReinitializeTopObjects()
    {
        if (currentTopParent == firstPlatesParent)
        {
            currentTopParent = secondPlatesParent;
            currentTopPlates = secondContainerElements;
        }
        else
        {
            currentTopParent = firstPlatesParent;
            currentTopPlates = firstContainerElements;
        }
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

        if (lastPlateXPos == 7.5f)
        {
            lastPlateZPos += 2.5f;
            lastPlateXPos = startPlateXPos;
        }
    }

    private void SpawnStartPlates()
    {
        ResetLastSpawnPoints();
        
        for (var i = 0; i < 40; i++)
        {
            SpawnPlatesFieldByGeneralValue();
            firstPlatesParent.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
        currentTopPlates = firstContainerElements;
        needSpawnToFirstParent = !needSpawnToFirstParent;

        SpawnNewPlatesField();

        currentTopParent = firstPlatesParent;
    }
}