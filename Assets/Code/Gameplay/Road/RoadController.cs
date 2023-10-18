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
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameObject DiamondPrefab;
    [SerializeField] private Renderer RoadRenderer;
    [SerializeField] public float RoadDimension = 5f;

    
    private RoadGrid RoadGrid;
    private Rigidbody2D RB;
    private RoadFactory RoadFactory;

    private (float, float) CameraBounds;

    public void Setup(int obstaclesAmount, int coinsAmount, int diamondsAmount, float scrollSpeed, RoadFactory factory)
    {
        RoadGrid = new RoadGrid(Width, Height, obstaclesAmount, coinsAmount, diamondsAmount);
        RB = GetComponent<Rigidbody2D>();
        RoadFactory = factory;
        
        CellType[][] grid = RoadGrid.Grid;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (grid[i][j] != CellType.None)
                {
                    InstantiateAtRoad(grid[i][j], i, j);
                }
            }
        }

        SetScrollSpeed(scrollSpeed);
        CameraBounds = CameraUtilities.CalculateCameraBoundaries();
    }
    
    public void SetScrollSpeed(float speed)
    {
        if(RB != null)
        {
            RB.velocity = new Vector2(0, speed);
        }
    }

    private void InstantiateAtRoad(CellType type, int x, int y)
    {
        var originPosition = Origin.localPosition;
        Vector3 position = new Vector3
        {
            x = (x * CellDimension) + originPosition.x,
            y = (y * CellDimension) + originPosition.y,
        };

        RoadItem item = RoadFactory.CreateNewItem((int)type, transform);
        item.transform.localPosition = position;
    }
    
    // TODO: Can this be converted in a method on the CameraUtilities?
    private void CheckForOutOfBoundaries()
    {
        float topYPosition = gameObject.transform.position.y + (RoadDimension / 2);
        if (topYPosition < CameraBounds.Item1)
        {
            Events.OnRemoveFromRoadList.Dispatch(this);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        CheckForOutOfBoundaries();
    }
}
