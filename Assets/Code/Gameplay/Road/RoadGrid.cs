using UnityEngine;
using Random = System.Random;

public enum CellType
{
    None,
    Bump
}

public class RoadGrid
{
    private Random randomGenerator = new Random();

    private int width;
    private int height;
    
    private CellType[][] grid;

    public CellType[][] Grid => grid;
    
    public RoadGrid(int width, int height, int obstaclesAmount)
    {
        this.width = width;
        this.height = height;
        grid = new CellType[width][];

        // First we set up the base grid.
        for (int i = 0; i < width; i++)
        {
            grid[i] = new CellType[height];
            for (int j = 0; j < height; j++)
            {
                grid[i][j] = CellType.None;
            }
        }
        
        // Then we add randomly the obstacles in the grid based on the obstaclesAmount value.
        for (int i = 0; i < obstaclesAmount; i++)
        {
            ConvertIntoBump();
        }
    }

    private void ConvertIntoBump()
    {
        int posX = randomGenerator.Next(0, width);
        int posY = randomGenerator.Next(0, height);
        if (grid[posX][posY] == CellType.None)
        {
            grid[posX][posY] = CellType.Bump;
        }
        else
        {
            ConvertIntoBump();
        }
    }

    public void DebugPrintGrid()
    {
        Debug.Log(grid);
    }
    
}
