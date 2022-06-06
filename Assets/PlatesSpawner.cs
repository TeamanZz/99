using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesSpawner : MonoBehaviour
{
    public GameObject platePrefab;
    public GameObject platesParent;

    public int generalValue = 0;

    public float startPlateXPos;
    public float startPlateZPos;

    private float lastPlateXPos;
    private float lastPlateZPos;

    void Start()
    {
        lastPlateXPos = startPlateXPos;
        lastPlateZPos = startPlateZPos;

        for (var i = 0; i < 50; i++)
        {
            SpawnPlatesFieldByGeneralValue();
        }
    }

    public void SpawnPlatesFieldByGeneralValue()
    {
        Vector3 newPlatePosition = new Vector3(lastPlateXPos, 0, lastPlateZPos);
        var newPlate = Instantiate(platePrefab, newPlatePosition, Quaternion.Euler(270, 0, 0), platesParent.transform);
        var plateComponent = newPlate.GetComponent<Plate>();
        plateComponent.SetNewNonZeroValue(generalValue);
        CalculateNewPlatePosition();
        // plateLineComponent.SetPlatesValue(currentPlateValue);
        // lastPlateLineZPos += 2;
        // plateLinesList.Add(plateLineComponent);
    }

    public void CalculateNewPlatePosition()
    {
        lastPlateXPos += 2.5f;

        if (lastPlateXPos == 7.5f)
        {
            lastPlateZPos += 2.5f;
            lastPlateXPos = startPlateXPos;

        }
        Debug.Log(lastPlateXPos + " | " + lastPlateZPos);
    }
}
