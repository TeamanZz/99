using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatesSpawner : MonoBehaviour
{
    public static PlatesSpawner Instance;

    public GameObject platePrefab;
    public GameObject firstPlatesParent;
    public GameObject secondPlatesParent;

    public int generalValue = 0;

    public float startPlateXPos;
    public float startPlateZPos;

    private float lastPlateXPos;
    private float lastPlateZPos;

    public List<Plate> currentTopPlates = new List<Plate>();

    public bool isFirstParentOnTop = true;
    public bool needSpawnToFirstParent = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetLastSpawnPoints();
        for (var i = 0; i < 50; i++)
        {
            SpawnPlatesFieldByGeneralValue();
            firstPlatesParent.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }

        needSpawnToFirstParent = !needSpawnToFirstParent;

        SpawnNewPlatesField();
    }

    public void SpawnPlatesFieldByGeneralValue()
    {
        GameObject newPlate;
        Plate plateComponent;

        if (needSpawnToFirstParent)
        {
            Vector3 newPlatePosition = new Vector3(lastPlateXPos, 0, lastPlateZPos);
            newPlate = Instantiate(platePrefab, newPlatePosition, Quaternion.Euler(270, 0, 0), firstPlatesParent.transform);
        }
        else
        {
            Vector3 newPlatePosition = new Vector3(lastPlateXPos, 0, lastPlateZPos);
            newPlate = Instantiate(platePrefab, newPlatePosition, Quaternion.Euler(270, 0, 0), secondPlatesParent.transform);
            newPlate.transform.localPosition += new Vector3(0, -4, 0);
        }
        plateComponent = newPlate.GetComponent<Plate>();
        currentTopPlates.Add(plateComponent);
        plateComponent.SetNewNonZeroValue(generalValue);
        CalculateNewPlatePosition();
    }

    public void SpawnNewPlatesField()
    {
        // generalValue++;
        ResetLastSpawnPoints();
        for (var i = 0; i < 50; i++)
        {
            SpawnPlatesFieldByGeneralValue();
        }

        needSpawnToFirstParent = !needSpawnToFirstParent;
    }

    public void DecreasePlatesCount(Plate plate)
    {
        currentTopPlates.Remove(plate);
        Destroy(plate);
        if (currentTopPlates.Count == 0)
        {
            MoveBottomPlatesOnTop();
        }
    }

    private void MoveBottomPlatesOnTop()
    {
        GameObject currentTopParent;
        Vector3 firstParentPosition = firstPlatesParent.transform.position;
        Vector3 secondParentPosition = secondPlatesParent.transform.position;

        firstPlatesParent.transform.DOMove(secondParentPosition, 2);
        secondPlatesParent.transform.DOMove(firstParentPosition, 2);

        if (isFirstParentOnTop)
            currentTopParent = secondPlatesParent;
        else
            currentTopParent = firstPlatesParent;

        for (int i = 0; i < 50; i++)
        {
            var s = DOTween.Sequence();
            s.Append((currentTopParent.transform.GetChild(i).DOShakeScale(1, 0.1f, 10, 30, true).SetEase(Ease.InOutBack)));
            currentTopParent.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
        isFirstParentOnTop = !isFirstParentOnTop;

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
}