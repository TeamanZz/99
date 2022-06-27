using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPlate : Plate
{
    [Space(10)]
    [Header("Armor Settings")]
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
        if (TakeDamage(LevelManager.levelManager.defaultDamage + LevelManager.levelManager.allDamageValue) == true)
            DestroyShield();
    }

    protected override void OnMouseEnter()
    {
        if (KnifeSystem.knife.readyToUse == false)
            return;

        if (TakeDamage(LevelManager.levelManager.knifeDamage + LevelManager.levelManager.allDamageValue) == true)
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

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.IndexReset();

    }
}
