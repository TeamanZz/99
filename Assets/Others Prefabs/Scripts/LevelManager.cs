using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Conect Settings")]
    public PlatesSpawner spawner;

    public List<Plate> platesPrefabs = new List<Plate>();
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && platesPrefabs.Count > 0)
        {
            Debug.Log("Add Plate");
            AddPlate();
        }
    }

    public void AddPlate()
    {
        spawner.platePrefab.Add(platesPrefabs[0].gameObject);
        platesPrefabs.RemoveAt(0);
    }
}
