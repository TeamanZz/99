using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Plate : MonoBehaviour
{
    [Header("DATA Settings")]
    public int valueToDestroy;
    public Vector2Int currentIndex;

    [Header("View Settings")]
    [SerializeField] private TextMeshPro valueText;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject explodeParticles;

    public void SetNewNonZeroValue(int newValue = 1)
    {
        int newValueRand = Random.Range(1, newValue + 1);
        valueToDestroy = newValueRand;

        UpdateTextValue();
        SetColorDependsOnValue();
    }

    protected virtual void OnMouseDown()
    {
        TakeDamage(1);
    }

    protected virtual void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        TakeDamage(1);
    }

    public bool TakeDamage(int damageValue)
    {
        valueToDestroy -= damageValue;
        if (Knife.knife.isOpen)
            Knife.knife.AddFillPoint(damageValue);

        ShakePlate();
        var newParticles = Instantiate(hitParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);

        SFX.Instance.PlaySound(0);

        if (CheckOnZeroValue())
        {
            KillPlate();
            return true;
        }
        else
        {
            UpdateTextValue();
            SetColorDependsOnValue();
            return false;
        }
    }

    public void UpdateTextValue()
    {
        valueText.text = valueToDestroy.ToString();
    }

    public virtual void CheckForContinuation()
    {
        KillPlate();
    }

    public void KillPlate()
    {
        valueToDestroy = 0;
        var newParticles = Instantiate(explodeParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);
        SetDefaultColor();
        SFX.Instance.PlayPlateDestroySound();
        ShakePlate();
        DisableText();

        PlatesSpawner.Instance.DecreasePlatesCount(this);
    }

    private bool CheckOnZeroValue()
    {
        return (valueToDestroy <= 0);
    }

    private void DisableText()
    {
        valueText.gameObject.SetActive(false);
    }

    private void ShakePlate()
    {
        transform.DOShakePosition(0.1f, 0.2f, 10, 90);
    }

    private void SetColorDependsOnValue()
    {
        var newColor = ColorsHandler.Instance.GetLerpedColor(valueToDestroy);
        if (meshRenderer != null)
        {
            var currentMaterial = meshRenderer.material;
            meshRenderer.material = new Material(currentMaterial);
            meshRenderer.material.color = newColor;

            valueText.color = ColorsHandler.Instance.GetTextColor(valueToDestroy);
        }
    }

    private void SetDefaultColor()
    {
        var newColor = ColorsHandler.Instance.GetDefaultColor();
        if (meshRenderer != null)
        {
            var currentMaterial = meshRenderer.material;
            meshRenderer.material = new Material(currentMaterial);
            meshRenderer.material.color = newColor;
        }
    }
}