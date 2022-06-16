using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlate : Plate
{
    public SelectedPlateType currentType;
    public enum SelectedPlateType
    {
        RemoteController,
        Receiver
    }

    public List<GameObject> selectedViewObjects = new List<GameObject>();
    public List<Renderer> selectedViewRenderer = new List<Renderer>();

    public List<Material> viewMaterials = new List<Material>();

    [Header("Remote Controller Settings")]
    public int number;
    public bool recieverIsActive = false;

    public void Awake()
    {
        currentType = (SelectedPlateType)Mathf.Clamp(Random.Range(-10, 10), 0, 1);

        Debug.Log("Current Type =" + currentType);

        Initialization();
    }

    [ContextMenu("Inititalization")]
    public void Initialization()
    {
        for (int i = 0; i < selectedViewObjects.Count; i++)
        {
            selectedViewObjects[i].SetActive(false);
        }

        for (int i = 0; i < selectedViewRenderer.Count; i++)
        {
            selectedViewRenderer[i].material = viewMaterials[0];
            if (i == 1)
                selectedViewRenderer[i].material = viewMaterials[1];
        }

        if (currentType == SelectedPlateType.Receiver)
        {
            selectedViewObjects[0].SetActive(true);
        }
        else
            selectedViewObjects[1].SetActive(true);

        if (currentType == SelectedPlateType.RemoteController)
            base.valueToDestroy *= 2;
        else
            base.valueToDestroy = 1;

        UpdatePlateUI();
        SelectedPlatesManager.selectedPlatesManager.UpdateAllPlatesUI();
    }

    public void ReciverViewState()
    {
        if (currentType == SelectedPlateType.RemoteController)
            return;
        
        switch(recieverIsActive)
        {
            case false:
                selectedViewRenderer[0].material = viewMaterials[0];
                selectedViewRenderer[1].material = viewMaterials[1];
                GetComponent<BoxCollider>().enabled = false;
                break;

            case true:
                selectedViewRenderer[0].material = viewMaterials[1];
                selectedViewRenderer[1].material = viewMaterials[0];
                GetComponent<BoxCollider>().enabled = true;
                break;
        }
    }

    public void UpdatePlateUI()
    {
        if (currentType == SelectedPlateType.RemoteController)
        {
            UpdateTextValue();
        }
        else
        {
            if (PlatesSpawner.Instance.currentRemoteController.Count > 0)
                valueText.text = PlatesSpawner.Instance.currentRemoteController.Count.ToString();
            else
                valueText.text = "";
        }
    }

    protected virtual void OnMouseDown()
    {
        if (currentType == SelectedPlateType.Receiver && recieverIsActive == false)
            return;

        if (TakeDamage(1) == true)
            RemovePlate();
    }

    protected virtual void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false || (currentType == SelectedPlateType.Receiver && recieverIsActive == false))
            return;

        if (TakeDamage(1) == true)
            RemovePlate();
    }

    public override void CheckForContinuation()
    {
        RemovePlate();
    }

    public void RemovePlate()
    {
        if (currentType == SelectedPlateType.RemoteController)
        {
            SelectedPlatesManager.selectedPlatesManager.selectedPlates.Remove(this);
            PlatesSpawner.Instance.currentRemoteController.Remove(this);
            SelectedPlatesManager.selectedPlatesManager.UpdateAllPlatesUI();
        }
        else
        {
            SelectedPlatesManager.selectedPlatesManager.selectedPlates.Remove(this);
            PlatesSpawner.Instance.currentReceiver.Remove(this);
            SelectedPlatesManager.selectedPlatesManager.UpdateAllPlatesUI();
        }
    }
}
