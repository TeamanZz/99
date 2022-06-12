using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePlate : Plate
{
    //public Plate currentPlate;
    
    //public ExplosivePlate explosiveInstruction;
    public bool isActive = true;

    public bool isUsed = false;

    protected override void OnMouseDown()
    {
        if (isActive == false)
            return;

        if (TakeDamage(1) == true)
            Explotion();
    }

    protected override void OnMouseEnter()
    {
        if (Knife.knife.readyToUse == false)
            return;

        if (isActive == false)
            return;

        if (TakeDamage(1) == true)
            Explotion();
    }

    public List<Plate> platesFromDestroy = new List<Plate>();
    [ContextMenu("Explotion")]
    public void Explotion()
    {
        if (isUsed)
            return;

        int newPosition = currentPosition.x + currentPosition.y * 5;
        Debug.Log("Current osition = " + newPosition);

        if (currentPosition.y < PlatesSpawner.Instance.gridSize.y - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPosition.x, currentPosition.y + 1)));

        if (currentPosition.y > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPosition.x, currentPosition.y - 1)));
        
        if (currentPosition.x < PlatesSpawner.Instance.gridSize.x - 1)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPosition.x + 1, currentPosition.y)));

        if (currentPosition.x > 0)
            platesFromDestroy.Add(FindPlate(new Vector2Int(currentPosition.x - 1, currentPosition.y)));

        isUsed = true;
        isSolo = true;

        foreach (var plate in platesFromDestroy)
        {
            if (plate != null)
            {
                plate.CheckForContinuation();
            }
        }

        //explosiveInstruction = null;
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
