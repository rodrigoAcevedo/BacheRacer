using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RoadController : MonoBehaviour
{
    [SerializeField] private int Width = 7;
    [SerializeField] private int Height = 9;
    [SerializeField] private float CellDimension = .5f;
    [SerializeField] private Transform Origin;
    [SerializeField] private List<GameObject> BumpPrefabs;
    [SerializeField] private Renderer RoadRenderer;
    [SerializeField] public float RoadDimension = 5f;

    
    private RoadGrid RoadGrid;
    private Rigidbody2D RB;

    private (float, float) CameraBounds;

    public void Setup(int obstaclesAmount, float scrollSpeed)
    {
        RoadGrid = new RoadGrid(Width, Height, obstaclesAmount);
        RB = GetComponent<Rigidbody2D>();
        
        CellType[][] grid = RoadGrid.Grid;

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

        RB.velocity = new Vector2(0, scrollSpeed);
        CameraBounds = CameraUtilities.CalculateCameraBoundaries();
    }

    private void InstantiateAtRoad(int x, int y)
    {
        var originPosition = Origin.localPosition;
        Vector3 position = new Vector3
        {
            x = (x * CellDimension) + originPosition.x,
            y = (y * CellDimension) + originPosition.y,
        };
        int index = Random.Range(0, BumpPrefabs.Count);
        GameObject bumpPrefab = Instantiate(BumpPrefabs[index], transform);
        bumpPrefab.transform.localPosition = position;
    }
    
    // TODO: Can this be converted in a method on the CameraUtilities?
    private void CheckForOutOfBoundaries()
    {
        float topYPosition = gameObject.transform.position.y + (RoadDimension / 2);
        if (topYPosition < CameraBounds.Item1)
        {
            Destroy(gameObject);
        }
    }
    
    void FixedUpdate()
    {
        CheckForOutOfBoundaries();
    }
}
