using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject Player;

    [SerializeField] private Transform PlayerStart;

    [SerializeField] private float Dimension = 5f;
    [SerializeField] private float YOriginOrdinate = -5f;
    [SerializeField] private int TotalRoadCellsToShow = 3;
    [SerializeField] private int InitialObstacles = 0;
    
    [SerializeField]
    private List<RoadController> roads;

    private float ScrollSpeed => GameManager.Instance.BaseParameters.BaseScrollSpeed + GameManager.Instance.TotalKilometersRan; // We want to make each level a bit more difficult adding speed.
    private float NitroScrollSpeed => ScrollSpeed * 1.5f;
    private float NitroTotalTime = 5f; // TODO: Parametrize this on playfab.
    private bool IsNitroActive;
    private float Timer = 0f;
    private float NitroTimer;
    private bool IsRiding = true;

    private void OnEnable()
    {
        Events.OnNitroActivated.Subscribe(OnNitroActivated);
        Events.OnRemoveFromRoadList.Subscribe(OnRemoveFromRoadList);
        Events.OnLoseGame.Subscribe(OnLoseGame);
    }

    private void OnDisable()
    {
        Events.OnNitroActivated.Subscribe(OnNitroActivated);
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
            InstantiateRoad(roadYPosition, GameManager.Instance.BaseParameters.BaseObstaclesAmount, CoinsToInstance(), DiamondsToInstance());
        }
    }

    private int CoinsToInstance()
    {
        int probabilityToSpawn = Random.Range(0, 101);
        int threshold = 30;
        return (int) (probabilityToSpawn <= threshold ? Math.Ceiling((decimal) (probabilityToSpawn / 10)) : 0);
    }

    private int DiamondsToInstance()
    {
        int probabilityToSpawn = Random.Range(0, 101);
        int threshold = 10;
        return (probabilityToSpawn <= threshold ? 1 : 0);
    }

    private void TrackMileageDone()
    {
        if (IsRiding)
        {
            Timer += Time.deltaTime;
            
            if (Timer >= GameManager.Instance.BaseParameters.LevelDurationInSeconds)
            {
                IsRiding = false;
                Events.OnMessage.Dispatch(MessageType.Principal, "Ganaste!");
                Events.OnWinLevel.Dispatch();
                GameManager.Instance.TotalKilometersRan++;
                UserDataUtility.SetPlayerKilometersRan(GameManager.Instance.TotalKilometersRan);
                Invoke(nameof(TransitionToIntermission), 5f);
            }

            if (IsNitroActive)
            {
                NitroTimer += Time.deltaTime;
                if (NitroTimer >= NitroTotalTime)
                {
                    Events.OnNitroActivated.Dispatch(false);
                }
            }
        }
    }


    private void InstantiateInitialRoads()
    {
        roads = new List<RoadController>();
        for (int i = 0; i < TotalRoadCellsToShow; i++)
        {
            float roadYPosition = ((i * Dimension) + YOriginOrdinate);
            InstantiateRoad(roadYPosition, InitialObstacles, 0, 0);
        }
    }

    // TODO: Can this be moved to a Factory like RoadFactory and leave this class to manage only gameplay?
    private void InstantiateRoad(float yPosition, int obstaclesAmount, int coinsAmount, int diamondsAmount)
    {
        Vector3 roadPosition = new Vector3(0, yPosition, 1);
        GameObject roadCellInstance = Instantiate(RoadPrefab, roadPosition, Quaternion.identity, transform.parent);
        RoadController road = roadCellInstance.GetComponent<RoadController>();
        float speed = IsNitroActive ? NitroScrollSpeed : ScrollSpeed;
        road.Setup(obstaclesAmount, coinsAmount, diamondsAmount, speed * (-1f));
        roads.Add(road);
    }

    private void OnRemoveFromRoadList(RoadController road)
    {
        roads.Remove(road);
    }

    private void OnNitroActivated(bool isActive)
    {
        IsNitroActive = isActive;
        float speed = (IsNitroActive ? NitroScrollSpeed : ScrollSpeed) * (-1f);
        foreach (RoadController road in roads)
        {
            road.SetScrollSpeed(speed);
        }
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
