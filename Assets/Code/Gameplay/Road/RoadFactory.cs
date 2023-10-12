using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
public class RoadFactory : MonoBehaviour
{
    [SerializeField] private int Width = 7;
    [SerializeField] private int Height = 9;
    [SerializeField] private int ObstaclesAmount = 3;
    [SerializeField] private float CellDimension = .5f;
    [SerializeField] private Transform Origin;
    [SerializeField] private List<GameObject> BumpPrefabs;
    
    private RoadGrid roadGrid;
    // Start is called before the first frame update
    void Start()
    {
        BuildRoad();
    }

    public void BuildRoad()
    {
        roadGrid = new RoadGrid(Width, Height, ObstaclesAmount);
        CellType[][] grid = roadGrid.Grid;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (grid[i][j] == CellType.Bump)
                {
                    InstantiateAtRoad(i, j);
                }
            }
        }
    }

    private void InstantiateAtRoad(int x, int y)
    {
        var originPosition = Origin.position;
        Vector3 position = new Vector3
        {
            x = (x * CellDimension) + originPosition.x,
            y = (y * CellDimension) + originPosition.y,
        };
        int index = Random.Range(0, BumpPrefabs.Count);
        GameObject bumpPrefab = Instantiate(BumpPrefabs[index], transform, false);
        bumpPrefab.transform.localPosition = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
