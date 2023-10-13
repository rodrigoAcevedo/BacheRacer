using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject Player;

    [SerializeField] private Transform PlayerStart;

    [SerializeField] private float Dimension = 5f;
    [SerializeField] private float YOriginOrdinate = -5f;
    [SerializeField] private int TotalRoadCellsToShow = 3;
    // For now we want this serialized but probably this value will come from the GameManager
    [SerializeField] private float ScrollSpeed = -0.1f; // We want it to go down.
    [SerializeField] private int initialObstacles = 0;
    
    private List<GameObject> roads;

    void Start()
    {
        GameObject playerInstance = Instantiate(Player, transform.parent);
        playerInstance.transform.position = PlayerStart.position;
        
        InstantiateInitialRoads();
    }

    private void FixedUpdate()
    {
        GameObject lastRoadRef = roads[roads.Count - 1];
        float lastRoadDimension = lastRoadRef.GetComponent<RoadController>().RoadDimension;
        float lastRoadYPosition = lastRoadRef.transform.position.y + (lastRoadDimension / 2);
        // TODO: Can this be converted in a method on the CameraUtilities?
        if (lastRoadYPosition < CameraUtilities.CalculateCameraBoundaries().Item2) // TODO: Add a method or property to get this on CameraUtilities.
        {
            InstantiateNewRoad();
        }
    }
    

    private void InstantiateInitialRoads()
    {
        roads = new List<GameObject>();
        for (int i = 0; i < TotalRoadCellsToShow; i++)
        {
            float roadYPosition = ((i * Dimension) + YOriginOrdinate);
            Vector3 roadPosition = new Vector3(0, roadYPosition, 1);
            GameObject roadCellInstance = Instantiate(RoadPrefab, roadPosition, Quaternion.identity, transform.parent);
            roadCellInstance.GetComponent<RoadController>().Setup(initialObstacles, ScrollSpeed);
            roads.Add(roadCellInstance);
        }
    }

    private void InstantiateNewRoad()
    {
        float roadYPosition = ((2 * Dimension) + YOriginOrdinate) + (Dimension / 2);
        Vector3 roadPosition = new Vector3(0, roadYPosition, 1);
        GameObject roadCellInstance = Instantiate(RoadPrefab, roadPosition, Quaternion.identity, transform.parent);
        roadCellInstance.GetComponent<RoadController>().Setup(3, ScrollSpeed); // TODO: Obstacles number should be given by GameManager.
        roads.Add(roadCellInstance);
    }
}
