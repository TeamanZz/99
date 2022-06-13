using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePlate : Plate
{
    [Header("Explote Settings")]
    public bool isUsed = false;
    public List<Plate> platesFromDestroy = new List<Plate>();

    protected override void OnMouseDown()
    {  
        if (TakeDamage(1) == true)
            Explotion();
    }

    protected override void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        if (TakeDamage(1) == true)
            Explotion();
    }

    [ContextMenu("Explotion")]
    public void Explotion()
    {
        if (isUsed)
            return;

        int newPosition = currentIndex.x + currentIndex.y * 5;
        Debug.Log("Current osition = " + newPosition);

        if (currentIndex.y < PlatesSpawner.Instance.gridSize.y - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentIndex.x, currentIndex.y + 1)));

        if (currentIndex.y > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentIndex.x, currentIndex.y - 1)));
        
        if (currentIndex.x < PlatesSpawner.Instance.gridSize.x - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentIndex.x + 1, currentIndex.y)));

        if (currentIndex.x > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentIndex.x - 1, currentIndex.y)));

        isUsed = true;
        
        foreach (var plate in platesFromDestroy)
        {
            if (plate != null)
            {
                plate.CheckForContinuation();
            }
        }
    }

    public override void CheckForContinuation()
    {
        Explotion();
        base.CheckForContinuation();
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
