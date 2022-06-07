using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Plate : MonoBehaviour
{
    public int value;

    [SerializeField] private TextMeshPro valueText;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject explodeParticles;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    public void SetNewNonZeroValue(int newValue = 1)
    {
        int newValueRand = Random.Range(1, newValue + 1);
        value = newValueRand;

        UpdateTextValue();
        SetColorDependsOnValue();
    }

    private void OnMouseDown()
    {
        TakeDamage(1);
    }

    // private void OnMouseEnter()
    // {
    //     TakeDamage(1);
    // }

    public void TakeDamage(int damageValue)
    {
        value -= damageValue;
        ShakePlate();
        var newParticles = Instantiate(hitParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);

        SFX.Instance.PlaySound(0);

        if (CheckOnZeroValue())
        {
            KillPlate();
        }
        else
        {
            UpdateTextValue();
            SetColorDependsOnValue();
        }
    }

    private void UpdateTextValue()
    {
        valueText.text = value.ToString();
    }

    private void KillPlate()
    {
        value = 0;
        var newParticles = Instantiate(explodeParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);
        SetDefaultColor();
        SFX.Instance.PlayPlateDestroySound();
        ShakePlate();
        DisableText();

        PlatesSpawner.Instance.DecreasePlatesCount(this);
    }

    private bool CheckOnZeroValue()
    {
        return (value <= 0);
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
        var newColor = ColorsHandler.Instance.GetLerpedColor(value);
        var currentMaterial = meshRenderer.material;
        meshRenderer.material = new Material(currentMaterial);
        meshRenderer.material.color = newColor;

        valueText.color = ColorsHandler.Instance.GetTextColor(value);
    }

    private void SetDefaultColor()
    {
        var newColor = ColorsHandler.Instance.GetDefaultColor();
        var currentMaterial = meshRenderer.material;
        meshRenderer.material = new Material(currentMaterial);
        meshRenderer.material.color = newColor;
    }
}