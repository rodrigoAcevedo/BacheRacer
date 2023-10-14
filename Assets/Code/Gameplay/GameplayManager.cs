using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject Player;

    [SerializeField] private Transform PlayerStart;

    [SerializeField] private float Dimension = 5f;
    [SerializeField] private float YOriginOrdinate = -5f;
    [SerializeField] private int TotalRoadCellsToShow = 3;
    // For now we want this serialized but probably this value will come from the GameManager
    [SerializeField] private float ScrollSpeed = 2;
    [FormerlySerializedAs("initialObstacles")] [SerializeField] private int InitialObstacles = 0;
    [SerializeField] private float TotalTrackToWin = 100;
    
    [SerializeField]
    private List<RoadController> roads;

    private float Mileage = 0f;
    private float Timer = 0f;
    private bool IsRiding = true;

    private void OnEnable()
    {
        Events.OnRemoveFromRoadList.Subscribe(OnRemoveFromRoadList);
        Events.OnLoseGame.Subscribe(OnLoseGame);
    }

    private void OnDisable()
    {
        Events.OnRemoveFromRoadList.Unsubscribe(OnRemoveFromRoadList);
        Events.OnLoseGame.Unsubscribe(OnLoseGame);
    }

    void Start()
    {
        GameObject playerInstance = Instantiate(Player, transform.parent);
        playerInstance.transform.position = PlayerStart.position;
        
        InstantiateInitialRoads();
    }

    private void FixedUpdate()
    {
        TrackMileageDone();
        
        GameObject lastRoadRef = roads[roads.Count - 1].gameObject;
        float lastRoadDimension = lastRoadRef.GetComponent<RoadController>().RoadDimension;
        float lastRoadYPosition = lastRoadRef.transform.position.y + (lastRoadDimension / 2);
        // TODO: Can this be converted in a method on the CameraUtilities?
        if (lastRoadYPosition < CameraUtilities.CalculateCameraBoundaries().Item2) // TODO: Add a method or property to get this on CameraUtilities.
        {
            float roadYPosition = ((2 * Dimension) + YOriginOrdinate) + (Dimension / 2); 
            InstantiateRoad(roadYPosition, 3);
        }
    }

    private void TrackMileageDone()
    {
        if (IsRiding)
        {
            Timer += Time.deltaTime;

            if (Timer >= 1.0f) // One second
            {
                Timer = 0f;
                Mileage += ScrollSpeed;
            }

            if (Mileage >= TotalTrackToWin)
            {
                IsRiding = false;
                Events.OnMessage.Dispatch(MessageType.Principal, "Ganaste!");
                Events.OnWinLevel.Dispatch();
                GameManager.Instance.TotalKilometersRan++;
                Invoke(nameof(TransitionToIntermission), 5f);
            }
        }
    }


    private void InstantiateInitialRoads()
    {
        roads = new List<RoadController>();
        for (int i = 0; i < TotalRoadCellsToShow; i++)
        {
            float roadYPosition = ((i * Dimension) + YOriginOrdinate);
            InstantiateRoad(roadYPosition, InitialObstacles);
        }
    }

    // TODO: Can this be moved to a Factory like RoadFactory and leave this class to manage only gameplay?
    private void InstantiateRoad(float yPosition, int obstaclesAmount)
    {
        Vector3 roadPosition = new Vector3(0, yPosition, 1);
        GameObject roadCellInstance = Instantiate(RoadPrefab, roadPosition, Quaternion.identity, transform.parent);
        RoadController road = roadCellInstance.GetComponent<RoadController>();
        road.Setup(obstaclesAmount, ScrollSpeed * (-1f));
        roads.Add(road);
    }

    private void OnRemoveFromRoadList(RoadController road)
    {
        roads.Remove(road);
    }

    private void OnLoseGame()
    {
        foreach (RoadController road in roads)
        {
            road.SetScrollSpeed(0);
        }
        Events.OnMessage.Dispatch(MessageType.Principal, "Has perdido");
        Invoke(nameof(TransitionToMainMenu), 5f);
    }
    
    private void TransitionToIntermission()
    {
        SceneManager.LoadScene(sceneName: "Intermission");
    }

    private void TransitionToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
