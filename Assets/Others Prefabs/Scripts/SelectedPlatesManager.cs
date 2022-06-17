using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlatesManager : MonoBehaviour
{
    public static SelectedPlatesManager selectedPlatesManager;
    public PlatesSpawner platesSpawner;
    
    public List<SelectedPlate> selectedPlates = new List<SelectedPlate>();

    public void Awake()
    {
        selectedPlatesManager = this;
    }

    public void UpdateAllPlatesUI()
    {
        if(platesSpawner.currentRemoteController.Count > 0)
        {
            foreach (var plate in selectedPlates)
            {
                plate.UpdatePlateUI();
            }
        }
        else
        {
            foreach (var plate in selectedPlates)
            {
                plate.recieverIsActive = true;
                plate.UpdatePlateUI();
                plate.ReciverViewState();
            }
        }
       
    }
    public void ClearList()
    {
        if (selectedPlates.Count == 0)
            return;

        foreach (var plate in selectedPlates)
        {
            if (plate == null)
                selectedPlates.Remove(plate);
        }
    }

    public List<SelectedPlate> FindElements(List<Plate> plates)
    {
        selectedPlates.Clear();
        List<Plate> tempPLates = new List<Plate>();
        tempPLates.AddRange(plates);

        foreach (var plate in tempPLates)
        {
            if (plate is SelectedPlate)
                selectedPlates.Add(plate.gameObject.GetComponent<SelectedPlate>());            
        }

        if (selectedPlates.Count == 0)
            return null;

        platesSpawner.currentRemoteController.Clear();
        platesSpawner.currentReceiver.Clear();

        foreach (var selectedPlate in selectedPlates)
        {
            if (selectedPlate.currentType == SelectedPlate.SelectedPlateType.RemoteController)
                platesSpawner.currentRemoteController.Add(selectedPlate);
            else
                platesSpawner.currentReceiver.Add(selectedPlate);
        }

        if (selectedPlates.Count > 2)
        {
            if (platesSpawner.currentReceiver.Count == 0)
            {
                int number = Random.Range(0, selectedPlates.Count);
                selectedPlates[number].currentType = SelectedPlate.SelectedPlateType.Receiver;
                selectedPlates[number].Initialization();
            }

            if (platesSpawner.currentRemoteController.Count == 0)
            {
                int number = Random.Range(0, selectedPlates.Count);
                selectedPlates[number].currentType = SelectedPlate.SelectedPlateType.RemoteController;
                selectedPlates[number].Initialization();
            }
        }
        else
        {
            selectedPlates[0].currentType = SelectedPlate.SelectedPlateType.RemoteController;
            selectedPlates[0].Initialization();
        }


        return null;
    }
}
