using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlate : Plate
{
    [Header("Wood Settings")]
    public Plate mainPlatePrefab;
    public int saveValue;
    public bool isActive = false;

    public void Start()
    {
        saveValue = base.valueToDestroy;
        base.valueToDestroy = 9999;
    }

    protected override void OnMouseDown()
    {
        SFX.Instance.PlayNonDamageSound(nullDamageSoundID);

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.IndexReset();
    }

    protected override void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        SFX.Instance.PlayNonDamageSound(nullDamageSoundID);

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.IndexReset();
    }

    public void DestroyWoodPlate()
    {
        if (isActive)
            return;

        Plate newPlate = Instantiate(mainPlatePrefab, transform.position, Quaternion.Euler(270, 0, 0), PlatesSpawner.Instance.currentTopParent.transform);
        newPlate.currentIndex = base.currentIndex;

        PlatesSpawner.Instance.currentTopPlates.Add(newPlate);
        newPlate.GetComponent<BoxCollider>().enabled = true;
        newPlate.plateIsActive = true;

        isActive = true;

        newPlate.SetNewNonZeroValue(saveValue);
        KillPlate();
    }
}
