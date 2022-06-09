using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePlate : MonoBehaviour
{
    public Plate currentPlate;
    public bool isActive = true;

    public bool isUsed = false;

    private void OnMouseDown()
    {
        if (isActive == false)
            return;

        if (currentPlate.TakeDamage(1) == true)
            Explotion();
    }

    private void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        if (isActive == false)
            return;

        if (currentPlate.TakeDamage(1) == true)
            Explotion();
    }
    public List<Plate> platesFromDestroy = new List<Plate>();
    [ContextMenu("Explotion")]
    public void Explotion()
    {
        if (isUsed)
            return;

        int newPosition = currentPlate.currentPosition.x + currentPlate.currentPosition.y * 5;
        Debug.Log("Current osition = " + newPosition);

        if (currentPlate.currentPosition.y < PlatesSpawner.Instance.gridSize.y - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPlate.currentPosition.x, currentPlate.currentPosition.y + 1)));

        if (currentPlate.currentPosition.y > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPlate.currentPosition.x, currentPlate.currentPosition.y - 1)));
        
        if (currentPlate.currentPosition.x < PlatesSpawner.Instance.gridSize.x - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPlate.currentPosition.x + 1, currentPlate.currentPosition.y)));

        if (currentPlate.currentPosition.x > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPlate.currentPosition.x - 1, currentPlate.currentPosition.y)));

        isUsed = true;
        currentPlate.isSolo = true;
        foreach (var plate in platesFromDestroy)
        {
            if (plate != null)
            {
                plate.CheckForContinuation();
            }
        }
        currentPlate.explosiveInstruction = null;
    }

    public Plate FindPlate(Vector2Int position)
    {
        int newPosition = position.x + position.y * 5;
        Debug.Log("Position = " + newPosition);
        

        if (PlatesSpawner.Instance.currentCordPlates[newPosition] != null)
               return PlatesSpawner.Instance.currentCordPlates[newPosition];
        else
            return null;
    }
}
