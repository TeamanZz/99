using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPlate : Plate
{
    public int saveValue;
    public Plate platePrefab;

    public void Start()
    {
        saveValue = base.valueToDestroy;
        base.valueToDestroy *= 2;
        UpdateTextValue();
    }


    protected override void OnMouseDown()
    {
        if (TakeDamage(1) == true)
            DestroyShield();
    }

    protected override void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        if (TakeDamage(1) == true)
            DestroyShield();
            
    }

    public void DestroyShield()
    {
        Plate newPlate = Instantiate(platePrefab, transform.position, Quaternion.Euler(270, 0, 0), PlatesSpawner.Instance.currentTopParent.transform);
        newPlate.currentIndex = base.currentIndex;

        PlatesSpawner.Instance.currentTopPlates.Add(newPlate);
        newPlate.GetComponent<BoxCollider>().enabled = true;
        newPlate.plateIsActive = true;

        newPlate.SetNewNonZeroValue(saveValue);
    }
}