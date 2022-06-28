using DG.Tweening;
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
        LowShakePlate();

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.IndexReset();
    }

    protected override void OnMouseEnter()
    {
        if (KnifeSystem.knife.readyToUse == false)
            return;

        SFX.Instance.PlayNonDamageSound(nullDamageSoundID);
        LowShakePlate();

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.IndexReset();
    }

    private void LowShakePlate()
    {
        transform.DOShakePosition(0.1f, 0.2f, 5, 90);
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
