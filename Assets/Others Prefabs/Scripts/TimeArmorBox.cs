using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeArmorBox : Plate
{
    public GameObject mainObject;
    public float timeToProtection = 1f;
    public float ratationTime = 0.5f;

    public BoxCollider mainCollider;

    public Vector3 debugRotation;

    public void Start()
    {
        mainCollider = GetComponent<BoxCollider>();
        StartCoroutine(StartProtection());
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if(plateIsActive == false)
        {
            SFX.Instance.PlayNonDamageSound(nullDamageSoundID);
            LowShakePlate();

            if (PitchManager.pitchManager != null)
                PitchManager.pitchManager.IndexReset();
        }
    }
    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();

        if (plateIsActive == false)
        {
            SFX.Instance.PlayNonDamageSound(nullDamageSoundID);
            LowShakePlate();

            if (PitchManager.pitchManager != null)
                PitchManager.pitchManager.IndexReset();
        }
    }

    private void LowShakePlate()
    {
        transform.DOShakePosition(0.1f, 0.2f, 5, 90);
    }

    [ContextMenu("Debug Rotation")]
    public void DebugRotate()
    {
        //mainCollider.enabled = false;
        mainObject.transform.DOLocalRotate(debugRotation, ratationTime, RotateMode.Fast);
    }

    public IEnumerator StartProtection()
    {
        //mainCollider.enabled = false;
        plateIsActive = false;
        mainObject.transform.DOLocalRotate(new Vector3(0, 90, -180f), ratationTime, RotateMode.Fast);

        yield return new WaitForSeconds(timeToProtection);
        StartCoroutine(StopProtection());
    }

    public IEnumerator StopProtection()
    {
        mainObject.transform.DOLocalRotate(new Vector3(0, 0, -180f), ratationTime, RotateMode.Fast);
        plateIsActive = true;
        //mainCollider.enabled = true;

        yield return new WaitForSeconds(timeToProtection);
        StartCoroutine(StartProtection());
    }
}
