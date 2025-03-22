using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FloorCreator : MonoBehaviour
{
    public int roomSize = 10; // Room size in grid units
    public int pathCount = 5; // Number of paths to generate
    public int gridSize = 64; // gridSizexgridSize
    public enum TileType
    {
        Wall = 0,
        StraightVertical = 1,
        StraightHorizontal = 2,
        L1 = 3, // Top to Right
        L2 = 4, // Top to Left
        L3 = 5, // Bottom to Right
        L4 = 6, // Bottom to Left
        J4 = 7, // Quad Junction
        Empty = 8
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    private TileType[,] grid;
    private readonly Dictionary<Direction, int> dirWeightsDefault = new()
    {
        { Direction.Up, 10 },
        { Direction.Down, 10 },
        { Direction.Right, 10 },
        { Direction.Left, 10 }
    };
    private Dictionary<Direction, int> newWeights;

    void Awake()
    {
        newWeights = dirWeightsDefault;
    }

    Dictionary<Direction, int> dirAngles = new Dictionary<Direction, int>
    {
        { Direction.Up, 0 },
        { Direction.Down, 180 },
        { Direction.Right, 90 },
        { Direction.Left, -90 }
    };

    public GameObject straightVerticalPrefab;
    public GameObject straightHorizontalPrefab;
    public GameObject l1Prefab, l2Prefab, l3Prefab, l4Prefab;
    public GameObject WallPrefab, emptyPrefab, floorPrefab;
    public GameObject quadJunctionPrefab;
    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Debug.Log("Generating dungeon " + i + ", " + j);
                InitializeGrid();
                GeneratePaths(gridSize/3);
                CreateRoom();
                RenderGrid(i * gridSize * 21, j * gridSize * 21);
            }
        }

        // Using floor prefab, create a floor for the dungeon
        GameObject floor = Instantiate(floorPrefab, this.transform);
        floor.transform.localScale = new Vector3(50 * gridSize, 1, 50 * gridSize);
        floor.transform.position = new Vector3(gridSize*20, 0, gridSize*20);

        // Create walls around dungeon
        GameObject wallTop = Instantiate(WallPrefab);
        GameObject wallBottom = Instantiate(WallPrefab);
        GameObject wallLeft = Instantiate(WallPrefab);
        GameObject wallRight = Instantiate(WallPrefab);

        wallTop.transform.localScale = new Vector3(10 * gridSize/4, 1, 10); // 5660 0 2560
        wallTop.transform.position = new Vector3(gridSize * 42.5f, 0, gridSize * 20);
        wallTop.transform.rotation = Quaternion.Euler(0, 90, 0);

        wallBottom.transform.localScale = new Vector3(10 * gridSize/4, 1, 10);
        wallBottom.transform.position = new Vector3(-gridSize * 1.6f, 0, gridSize * 20);
        wallBottom.transform.rotation = Quaternion.Euler(0, 90, 0);

        wallLeft.transform.localScale = new Vector3(10, 1, 10 * gridSize/4);
        wallLeft.transform.position = new Vector3(gridSize * 20, 0, gridSize * 42.5f);
        wallLeft.transform.rotation = Quaternion.Euler(0, 90, 0);

        wallRight.transform.localScale = new Vector3(10, 1, 10 * gridSize/4);
        wallRight.transform.position = new Vector3(gridSize * 20, 0, -gridSize * 1.6f);
        wallRight.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void InitializeGrid()
    {
        grid = new TileType[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = TileType.Wall;
            }
        }
    }

    void CreateRoom()
    {
        // Create a room in the center of the grid
        int roomStartX = gridSize / 2 - roomSize / 2;
        int roomStartZ = gridSize / 2 - roomSize / 2;

        for (int i = roomStartX; i < roomStartX + roomSize; i++)
        {
            for (int j = roomStartZ; j < roomStartZ + roomSize; j++)
            {
                grid[i, j] = TileType.Empty;
            }
        }

        // Create a large hall on a random side of the room (hall made of tiletype.empty)
        int hallStartX = gridSize / 2;
        int hallStartZ = gridSize / 2;

        // if random == 0, create hall on left side
        // if random == 1, create hall on right side
        // if random == 2, create hall on top side
        // if random == 3, create hall on bottom side

        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                // Left side
                for (int i = 0; i < roomStartX; i++)
                {
                    grid[i, hallStartZ] = TileType.Empty;
                }
                break;
            case 1:
                // Right side
                for (int i = roomStartX + roomSize; i < gridSize; i++)
                {
                    grid[i, hallStartZ] = TileType.Empty;
                }
                break;
            case 2:
                // Top side
                for (int j = roomStartZ + roomSize; j < gridSize; j++)
                {
                    grid[hallStartX, j] = TileType.Empty;
                }
                break;
            case 3:
                // Bottom side
                for (int j = 0; j < roomStartZ; j++)
                {
                    grid[hallStartX, j] = TileType.Empty;
                }
                break;
        }
    }

    void GenerateSinglePath(int startX, int startZ)
    {
        TileType firstTile;
        Direction currentDirection;

        // Determine if path should be horizontal or vertical based on starting position
        if (startX == 0)
        {
            firstTile = TileType.StraightHorizontal;
            currentDirection = Direction.Right;
        }
        else if (startZ == 0)
        {
            firstTile = TileType.StraightVertical;
            currentDirection = Direction.Up;
        }
        else if (startX == gridSize - 1)
        {
            firstTile = TileType.StraightHorizontal;
            currentDirection = Direction.Left;
        }
        else if (startZ == gridSize - 1)
        {
            firstTile = TileType.StraightVertical;
            currentDirection = Direction.Down;
        }
        else
        {
            Debug.LogError("Invalid starting position for path generation");
            return;
        }
        Direction originalDirection = currentDirection;

        // Collapse the first tile and start path propagation
        CollapseTile(startX, startZ, firstTile, currentDirection, originalDirection);
    }
    void GeneratePaths(int numPaths)
    {
        for (int i = 0; i < numPaths; i++)
        {
            // Randomly choose a starting edge
            bool startFromLeft = Random.value > 0.5f; // 50% chance for each start type

            if (startFromLeft)
            {
                int startZ = Random.Range(2, gridSize-2); // Random z position along the left edge
                int startX = Random.Range(0, 2) == 0 ? 0 : gridSize - 1; // Random x position
                GenerateSinglePath(startX, startZ); // Start horizontally
            }
            else
            {
                int startX = Random.Range(2, gridSize-2); // Random x position along the top edge
                int startZ = Random.Range(0, 2) == 0 ? 0 : gridSize - 1; // Random z position
                GenerateSinglePath(startX, startZ); // Start vertically
            }
        }

        // Render the final paths
        // RenderGrid();
    }

    Queue<Direction> lastMoves = new Queue<Direction>(); // Stores last two moves
    void CollapseTile(int x, int z, TileType type, Direction dir, Direction originalDir = Direction.None)
    {
        grid[x, z] = type;

        if (dir == Direction.None) return; // Stop if no direction

        // Add the new direction to tracking queue

        // Propagate constraints
        PropagateConstraints(x, z, type, dir, originalDir);
    }
        // If last two moves are the same direction, prevent third consecutive turn
    void TrackLastMoves(Direction newDir)
    {
        if (lastMoves.Count >= 2) 
        {
            lastMoves.Dequeue(); // Remove oldest move
        }
        if (newDir == Direction.Left || newDir == Direction.Right) // Only track turns
        {
            lastMoves.Enqueue(newDir);
        }
    }

    void PropagateConstraints(int x, int z, TileType type, Direction dir, Direction originalDir)
    {
        // Check if within bounds
        if (x < 0 || x >= gridSize || z < 0 || z >= gridSize) return;


        // Tiles and directions:
        // UP: Vert, L3, L4
        // DOWN: Vert, L1, L2
        // RIGHT: Horiz, L2, L4
        // LEFT: Horiz, L1, L3

        // Check next tile based on direction

        // Get direction change
        // dir vs originalDir
        // Up vs Up = 0
        // Up vs Down = 180
        // Up vs Right = 90
        // Up vs Left = -90
        int angle = dirAngles[dir] - dirAngles[originalDir];
        Dictionary<Direction, int> newWeights = dirWeightsDefault.ToDictionary(entry => entry.Key, entry => entry.Value);
        switch (angle)
        {
            case 0:
                // Same direction
                // Default weight
                newWeights[Direction.Up] *= 60; // Forward
                break;
            case 90:
                // Right turn
                // Left Weight
                newWeights[Direction.Left] *= 2;
                newWeights[Direction.Right] /= 2;
                break;
            case -90:
                // Left turn
                // Right Weight
                newWeights[Direction.Right] *= 2;
                newWeights[Direction.Left] /= 2;
                break;
            case 180:
                // U-turn
                // Left and Right Weight
                newWeights[Direction.Right] *= 2;
                newWeights[Direction.Left] *= 2;
                break;
        }

        if (dir == Direction.Up)
        {
            if (z + 1 < gridSize && grid[x, z + 1] == TileType.Wall)
            {
                Direction chosenDir = GetWeightedAngle(newWeights); // Select turn direction
                (TileType, Direction) nextTileDir = GetNextTile(dir, chosenDir);

                CollapseTile(x, z + 1, nextTileDir.Item1, nextTileDir.Item2, originalDir);
            }
            else if (z + 1 < gridSize && grid[x, z + 1] != TileType.Wall)
            {
                CollapseTile(x, z + 1, TileType.J4, Direction.None);
            }
        }
        else if (dir == Direction.Down)
        {
            if (z - 1 >= 0) // && grid[x, z - 1] == TileType.Wall
            {
                Direction chosenDir = GetWeightedAngle(newWeights);
                (TileType, Direction) nextTileDir = GetNextTile(dir, chosenDir);

                CollapseTile(x, z - 1, nextTileDir.Item1, nextTileDir.Item2, originalDir);
            }
            // else if (z - 1 >= 0 && grid[x, z - 1] != TileType.Wall)
            // {
            //     CollapseTile(x, z - 1, TileType.J4, Direction.None);
            // }
        }
        else if (dir == Direction.Right)
        {
            if (x + 1 < gridSize) // && grid[x + 1, z] == TileType.Wall
            {
                Direction chosenDir = GetWeightedAngle(newWeights);
                (TileType, Direction) nextTileDir = GetNextTile(dir, chosenDir);

                CollapseTile(x + 1, z, nextTileDir.Item1, nextTileDir.Item2, originalDir);
            }
            // else if (x + 1 < gridSize && grid[x + 1, z] != TileType.Wall)
            // {
            //     CollapseTile(x + 1, z, TileType.J4, Direction.None);
            // }
        }
        else if (dir == Direction.Left)
        {
            if (x - 1 >= 0) // && grid[x - 1, z] == TileType.Wall
            {
                Direction chosenDir = GetWeightedAngle(newWeights);
                (TileType, Direction) nextTileDir = GetNextTile(dir, chosenDir);

                CollapseTile(x - 1, z, nextTileDir.Item1, nextTileDir.Item2, originalDir);
            }
            // else if (x - 1 >= 0 && grid[x - 1, z] != TileType.Wall)
            // {
            //     CollapseTile(x - 1, z, TileType.J4, Direction.None);
            // }
        }
    }

    Direction GetWeightedAngle(Dictionary<Direction, int> newWeights)
    {

        if (lastMoves.Count == 2 && lastMoves.Peek() == lastMoves.Last())
        {
            newWeights[lastMoves.Peek()] = 0; // Prevent third consecutive turn
        }
        // Get the direction based on the weights
        List<Direction> weightedDirs = new List<Direction>();
        List<int> angles = new List<int> { 0, 90, -90 };
        foreach (int angle in angles)
        {
            if (angle == 0)
            {
                for (int i = 0; i < newWeights[Direction.Up]; i++)
                {
                    weightedDirs.Add(Direction.Up);
                }
            }
            else if (angle == 90)
            {
                for (int i = 0; i < newWeights[Direction.Right]; i++)
                {
                    weightedDirs.Add(Direction.Right);
                }
            }
            else if (angle == -90)
            {
                for (int i = 0; i < newWeights[Direction.Left]; i++)
                {
                    weightedDirs.Add(Direction.Left);
                }
            }
        }
        Direction chosenDir = weightedDirs[Random.Range(0, weightedDirs.Count)];
        TrackLastMoves(chosenDir);
        return chosenDir;
    }
    (TileType, Direction) GetNextTile(Direction currentDirection, Direction newDirection)
    {
        // if (currentDirection == Direction.Up)
        // and newDirection == Direction.Up
        // return TileType.StraightVertical

        switch (currentDirection)
        {
            case Direction.Up:
                switch (newDirection)
                {
                    case Direction.Up:
                        return (TileType.StraightVertical, Direction.Up);
                    case Direction.Left:
                        return (TileType.L4, Direction.Left);
                    case Direction.Right:
                        return (TileType.L3, Direction.Right);
                }
                break;
            case Direction.Down:
                switch (newDirection)
                {
                    case Direction.Up:
                        return (TileType.StraightVertical, Direction.Down);
                    case Direction.Left:
                        return (TileType.L1, Direction.Right);
                    case Direction.Right:
                        return (TileType.L2, Direction.Left);
                }
                break;
            case Direction.Left:
                switch (newDirection)
                {
                    case Direction.Up:
                        return (TileType.StraightHorizontal, Direction.Left);
                    case Direction.Left:
                        return (TileType.L3, Direction.Down);
                    case Direction.Right:
                        return (TileType.L1, Direction.Up);
                }
                break;
            case Direction.Right:
                switch (newDirection)
                {
                    case Direction.Up:
                        return (TileType.StraightHorizontal, Direction.Right);
                    case Direction.Left:
                        return (TileType.L2, Direction.Up);
                    case Direction.Right:
                        return (TileType.L4, Direction.Down);
                }
                break;
        }
        return (TileType.Wall, Direction.None);
    }

    void RenderGrid(int startX, int startZ)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(startX + x * 20, 0, startZ + z * 20);
                switch (grid[x, z])
                {
                    case TileType.StraightVertical:
                        Instantiate(straightVerticalPrefab, position, Quaternion.Euler(0, 90, 0), this.transform);
                        break;
                    case TileType.StraightHorizontal:
                        Instantiate(straightHorizontalPrefab, position, Quaternion.identity, this.transform);
                        break;
                    case TileType.L1:
                        Instantiate(l1Prefab, position, Quaternion.identity, this.transform);
                        break;
                    case TileType.L2:
                        Instantiate(l2Prefab, position, Quaternion.Euler(0, -90, 0), this.transform);
                        break;
                    case TileType.L3:
                        Instantiate(l3Prefab, position, Quaternion.Euler(0, 90, 0), this.transform);
                        break;
                    case TileType.L4:
                        Instantiate(l4Prefab, position, Quaternion.Euler(0, 180, 0), this.transform);
                        break;
                    case TileType.Wall:
                        Instantiate(WallPrefab, position, Quaternion.identity, this.transform);
                        break;
                    case TileType.J4:
                        Instantiate(quadJunctionPrefab, position, Quaternion.identity, this.transform);
                        break;
                    case TileType.Empty:
                        Instantiate(emptyPrefab, position, Quaternion.identity, this.transform);
                        break;
                }
            }
        }
    }

    // void OnDrawGizmos()
    // {
    //     if (grid == null) return;

    //     for (int x = 0; x < gridSize; x++)
    //     {
    //         for (int z = 0; z < gridSize; z++)
    //         {
    //             Vector3 position = new Vector3(x * 20, 2, z * 20); // Slightly above grid

    //             // Draw a label at each grid position
    //             UnityEditor.Handles.Label(position, grid[x, z].ToString() + " (" + x + ", " + z + ")");
    //         }
    //     }
    // }


    //     void generateMobTypes()
    //     {
    //         // Generate mob types
    //         // List of all mobs for floor 1
    //         // All mobs are level 1-5
    //         // Janitor Mobs clean up corpses. They are level 2
    //         string[] mobs = {
    //             "Goblin", // Small, sneaky, loves stealing items mid-fight, weak but swarms in groups.
    //             "Pigeon", // Disease-ridden, dive-bombs from above, screeches loudly to alert others.
    //             "Rat", // Janitor rats in tiny uniforms, wielding broken mops, spreads filth.
    //             "Llama", // Aggressive, territorial, spits weak acid and occasionally bites.
    //             "Penguin" // Fast, slides on its belly, pecks ankles with shocking force.
    //         };

    //         string[] bosses = {
    //             "Goblin Chief", // A cunning goblin covered in **stolen adventurer gear**, fights dirty.
    //             "Pigeon Lady", // A **deranged old woman** covered in pigeons, throws bread to **buff allies**.
    //             "Rats Nest", // A pulsating **mass of fused rats**, attacks with a **whipping rat tail**.
    //             "Llama Prince", // A royal llama covered in **bone armor**, spits acid in **spray patterns**.
    //             "Emperor Penguin" // 8 feet tall, wears **a tattered golden crown**, commands **an army of ice penguins**.
    //         };

    //         // Choose 4 random mobs
    //         List<string> mobTypes = mobs.OrderBy(x => Random.value).Take(4).ToList();
    //         Debug.Log("Mob Types: " + string.Join(", ", mobTypes));
    //     }
}
