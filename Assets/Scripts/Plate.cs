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

    [Space(10)]
    [Header("View Settings")]
    public TextMeshPro valueText;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject hitParticles;
    [SerializeField] private GameObject explodeParticles;

    public bool plateIsActive = false;

    [Space(10)]
    [Header("Sound Settings")]
    public int nullDamageSoundID = 0;
    public int damageSoundID = 0;
    public int destroySoundID = 0;

    [Space(10)]
    [Header("View")]
    public Sprite image;
    public Sprite backImage;
    public string info;
    public string infoBottom;

    public void SetNewNonZeroValue(int newValue = 1)
    {
        int newValueRand = Random.Range(1, newValue + 1);
        valueToDestroy = newValueRand;

        UpdateTextValue();
        SetColorDependsOnValue();
    }

    protected virtual void OnMouseDown()
    {
        if (plateIsActive == false)
            return;

        TakeDamage(LevelManager.levelManager.defaultDamage + LevelManager.levelManager.allDamageValue);
    }

    protected virtual void OnMouseEnter()
    {
        if (plateIsActive == false)
            return;

        if (KnifeSystem.knife.readyToUse == false)
            return;

        TakeDamage(LevelManager.levelManager.knifeDamage + LevelManager.levelManager.allDamageValue);
    }

    public bool TakeDamage(int damageValue)
    {
        valueToDestroy -= damageValue;

        if (InfoController.infoController != null)
            InfoController.infoController.touches += damageValue;

        //if (Knife.knife.isOpen)
        //    Knife.knife.AddFillPoint(damageValue);

        ShakePlate();
        var newParticles = Instantiate(hitParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);

        if (PitchManager.pitchManager != null)
            PitchManager.pitchManager.CheckPlate(currentIndex);

        SFX.Instance.PlayDamageSound(damageSoundID);

        if (CheckOnZeroValue())
        {
            CheckAdjacentPlates();
            Invoke("KillPlate", 0.01f);
            return true;
        }
        else
        {
            UpdateTextValue();
            SetColorDependsOnValue();
            return false;
        }
    }

    public virtual void CheckForContinuation()
    {
        CheckAdjacentPlates();
        KillPlate();
    }

    public void UpdateTextValue()
    {
        valueText.text = valueToDestroy.ToString();
    }

    private List<WoodPlate> adjacentWoodPlates = new List<WoodPlate>();
    public void CheckAdjacentPlates()
    {
        adjacentWoodPlates.Clear();
        if (currentIndex.y < PlatesSpawner.Instance.gridSize.y - 1)
            adjacentWoodPlates.Add(FindPlate(new Vector2Int(currentIndex.x, currentIndex.y + 1)));

        if (currentIndex.y > 0)
            adjacentWoodPlates.Add(FindPlate(new Vector2Int(currentIndex.x, currentIndex.y - 1)));

        if (currentIndex.x < PlatesSpawner.Instance.gridSize.x - 1)
            adjacentWoodPlates.Add(FindPlate(new Vector2Int(currentIndex.x + 1, currentIndex.y)));

        if (currentIndex.x > 0)
            adjacentWoodPlates.Add(FindPlate(new Vector2Int(currentIndex.x - 1, currentIndex.y)));

        //ClearWoodList();

        foreach (var plate in adjacentWoodPlates)
        {
            if (plate != null)
            {
                plate.DestroyWoodPlate();
                //plate.KillPlate();
            }
        }
    }

    public void ClearWoodList()
    {
        foreach (var plate in adjacentWoodPlates)
        {
            if (plate == null)
                adjacentWoodPlates.Remove(plate);
        }
    }

    public WoodPlate FindPlate(Vector2Int position)
    {
        int newPosition = position.x + position.y * 5;
        Debug.Log("Position = " + newPosition);

        if (PlatesSpawner.Instance.currentCordPlates[newPosition] != null)
        {
            WoodPlate currentWoodPlate = PlatesSpawner.Instance.currentCordPlates[newPosition].GetComponent<WoodPlate>();

            if (currentWoodPlate != null)
                return currentWoodPlate;
            else
                return null;
        }
        else
            return null;
    }

    protected virtual void KillPlate()
    {
        valueToDestroy = 0;
        var newParticles = Instantiate(explodeParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity, transform.parent.parent);
        SetDefaultColor();
        SFX.Instance.PlayDestroySound(destroySoundID);
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