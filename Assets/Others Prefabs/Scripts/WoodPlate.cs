using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlate : Plate
{
    [Header("Wood Settings")]
    public Plate mainPlatePrefab;
    public int saveValue;

    public void Start()
    {
        saveValue = base.valueToDestroy;
    }

    public void DestroyWoodPlate()
    {
        Plate newPlate = Instantiate(mainPlatePrefab, transform.position, Quaternion.Euler(270, 0, 0), PlatesSpawner.Instance.currentTopParent.transform);
        newPlate.currentIndex = base.currentIndex;

        PlatesSpawner.Instance.currentTopPlates.Add(newPlate);
        newPlate.GetComponent<BoxCollider>().enabled = true;
        newPlate.plateIsActive = true;

        newPlate.SetNewNonZeroValue(saveValue);
        KillPlate();
    }
}
