using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Floor1 : MonoBehaviour
{
    [System.Serializable]
    public struct Dot
    {
        public int x;
        public int y;

        public Vector2Int Pos => new Vector2Int(x, y);
    }

    public struct Edge
    {
        public Dot a;
        public Dot b;
    }

    [Header("Grid Settings")]
    public int gridSize = 10;
    public int numDots = 10;

    private List<Dot> dots = new();
    private List<Edge> hallways = new();

    // Create a button to regenerate the level
    public bool regenerateOnStart = false;
    // Slider
    [Range(1, 100)]
    public int fullLengthChance = 100;
    [Range(1, 36)]
    public int halfRoomSize = 9;
    public GameObject cubePrefab;
    public GameObject floorPrefab;
    private int[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        Main();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenerateOnStart)
        {
            regenerateOnStart = false;
            Main();
        }
    }

    void Main()
    {
        Generate();
        grid = PreRender();
        Render(grid);
        StaticBatchingUtility.Combine(this.gameObject);
        // MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        // CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        // for (int i = 0; i < meshFilters.Length; i++)
        // {
        //     combine[i].mesh = meshFilters[i].sharedMesh;
        //     combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        //     meshFilters[i].gameObject.SetActive(false);
        // }
        // Mesh mesh = new Mesh();
        // mesh.indexFormat = IndexFormat.UInt32;
        // mesh.CombineMeshes(combine);
        // transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        // transform.gameObject.SetActive(true);

        // Mesh combinedMesh = new Mesh();
        // combinedMesh.indexFormat = IndexFormat.UInt32;
        // combinedMesh.CombineMeshes(combine, true, true);
        
        // WeldVertices(combinedMesh);

        // MeshFilter parentMeshFilter = transform.GetComponent<MeshFilter>();
        // if (parentMeshFilter != null)
        // {
        //     parentMeshFilter.sharedMesh = combinedMesh;
        // }
        // transform.gameObject.SetActive(true);
    }

    void WeldVertices(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uv = mesh.uv;
        int[] triangles = mesh.triangles;

        Dictionary<Vector3, int> vertexMap = new Dictionary<Vector3, int>();
        List<Vector3> weldedVertices = new List<Vector3>();
        List<Vector3> weldedNormals = new List<Vector3>();
        List<Vector2> weldedUVs = new List<Vector2>();
        List<int> weldedTriangles = new List<int>();

        // This maps original vertex index to new welded vertex index
        int[] vertexRemap = new int[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            if (!vertexMap.ContainsKey(vertex))
            {
                int newIndex = weldedVertices.Count;
                vertexMap[vertex] = newIndex;

                weldedVertices.Add(vertex);
                weldedNormals.Add(normals[i]);
                weldedUVs.Add(uv[i]);
            }

            vertexRemap[i] = vertexMap[vertex];
        }

        // Rebuild triangles using remapped indices
        for (int i = 0; i < triangles.Length; i++)
        {
            weldedTriangles.Add(vertexRemap[triangles[i]]);
        }

        Debug.Log($"Welded {vertices.Length} vertices down to {weldedVertices.Count} vertices.");
        
        mesh.Clear();
        mesh.vertices = weldedVertices.ToArray();
        mesh.normals = weldedNormals.ToArray();
        mesh.uv = weldedUVs.ToArray();
        mesh.triangles = weldedTriangles.ToArray();
        mesh.RecalculateBounds();
    }


    public void Generate()
    {
        dots.Clear();
        hallways.Clear();

        // Step 1: Place random dots
        HashSet<Vector2Int> used = new();
        while (dots.Count < numDots)
        {
            int x = Random.Range(1, gridSize-1);
            int y = Random.Range(1, gridSize-1);
            Vector2Int pos = new(x, y);

            if (used.Add(pos))
                dots.Add(new Dot { x = x, y = y });
        }

        // Step 2: Horizontal connections
        var groupedByRow = dots.GroupBy(d => d.y);
        foreach (var row in groupedByRow)
        {
            var rowDots = row.OrderBy(d => d.x).ToList();
            if (rowDots.Count == 1)
            {
                AddRandomConnection(rowDots[0]);
                continue;
            }

            for (int i = 0; i < rowDots.Count - 1; i++)
            {
                if (!AnyBetween(rowDots[i], rowDots[i + 1], true))
                    hallways.Add(new Edge { a = rowDots[i], b = rowDots[i + 1] });
            }
        }

        // Step 3: Vertical connections
        var groupedByCol = dots.GroupBy(d => d.x);
        foreach (var col in groupedByCol)
        {
            var colDots = col.OrderBy(d => d.y).ToList();
            if (colDots.Count == 1)
            {
                AddRandomConnection(colDots[0]);
                continue;
            }

            for (int i = 0; i < colDots.Count - 1; i++)
            {
                if (!AnyBetween(colDots[i], colDots[i + 1], false))
                    hallways.Add(new Edge { a = colDots[i], b = colDots[i + 1] });
            }
        }
        Debug.Log($"Generated {dots.Count} dots and {hallways.Count} hallways.");
    }

    bool AnyBetween(Dot a, Dot b, bool horizontal)
    {
        foreach (var d in dots)
        {
            if (horizontal)
            {
                if (d.y == a.y && d.x > Mathf.Min(a.x, b.x) && d.x < Mathf.Max(a.x, b.x))
                    return true;
            }
            else
            {
                if (d.x == a.x && d.y > Mathf.Min(a.y, b.y) && d.y < Mathf.Max(a.y, b.y))
                    return true;
            }
        }
        return false;
    }

    void AddRandomConnection(Dot d)
    {
        var possible = dots.Where(o =>
            (o.x == d.x || o.y == d.y) &&
            !o.Pos.Equals(d.Pos)).ToList();

        if (possible.Count == 0)
        {
            if (Random.Range(0,100/fullLengthChance) == 0)
            {
                if (Random.Range(0,2) == 0)
                {
                    Dot _target = new Dot { x = gridSize, y = d.y };
                    d.x = 0;
                    hallways.Add(new Edge { a = d, b = _target });
                }
                else
                {
                    Dot _target = new Dot { x = d.x, y = gridSize };
                    d.y = 0;
                    hallways.Add(new Edge { a = d, b = _target });
                }
            }
        return;
        }
        Dot target = possible[Random.Range(0, possible.Count)];
        hallways.Add(new Edge { a = d, b = target });
    }

    public bool IsInHallway(int x, int y)
    {
        foreach (var edge in hallways)
        {
            Dot a = edge.a;
            Dot b = edge.b;

            // Horizontal hallway
            if (a.y == b.y && y == a.y)
            {
                int minX = Mathf.Min(a.x, b.x);
                int maxX = Mathf.Max(a.x, b.x);
                if (x >= minX && x <= maxX)
                    return true;
            }

            // Vertical hallway
            if (a.x == b.x && x == a.x)
            {
                int minY = Mathf.Min(a.y, b.y);
                int maxY = Mathf.Max(a.y, b.y);
                if (y >= minY && y <= maxY)
                    return true;
            }
        }
        return false;
    }

    public bool IsNextToHallway(int x, int y)
    {
        foreach (var edge in hallways)
        {
            Dot a = edge.a;
            Dot b = edge.b;

            // Horizontal hallway
            if (a.y == b.y)
            {
                if (a.y != y)
                {
                    if ((y == a.y - 1 || y == a.y + 1) && x >= Mathf.Min(a.x-1, b.x-1) && x <= Mathf.Max(a.x+1, b.x+1))
                        return true;
                }
            }

            // Vertical hallway
            if (a.x == b.x)
            {
                if (a.x != x)
                {
                    if ((x == a.x - 1 || x == a.x + 1) && y >= Mathf.Min(a.y-1, b.y-1) && y <= Mathf.Max(a.y+1, b.y+1))
                        return true;
                }
            }
        }
        return false;
    }

    public bool BossRoomCreator1(int _x, int _y)
    {
        // Makes an empty room in the middle of the grid

        // At the middle of the grid make a room of 9x9
        for (int x = gridSize / 2 - halfRoomSize; x < gridSize / 2 + halfRoomSize; x++)
        {
            for (int y = gridSize / 2 - halfRoomSize; y < gridSize / 2 + halfRoomSize; y++)
            {
                if (x == _x && y == _y)
                    return true;
            }
        }
        return false;
    }

    public bool BossRoomCreator2(int _x, int _y)
    {
        int min = gridSize / 2 - halfRoomSize;
        int max = gridSize / 2 + halfRoomSize;

        // Expand by 1 in each direction
        int wallMin = min - 1;
        int wallMax = max;

        // Check if in outer ring
        bool isInOuter = _x >= wallMin && _x <= wallMax && _y >= wallMin && _y <= wallMax;
        bool isInInner = _x >= min && _x < max && _y >= min && _y < max;

        return isInOuter && !isInInner;
    }

    public bool BossRoomCreator3(int _x, int _y)
    {
        int min = gridSize / 2 - halfRoomSize;
        int max = gridSize / 2 + halfRoomSize;

        int outerMin = min - 2;
        int outerMax = max + 1;
        int innerMin = min - 1;
        int innerMax = max;

        bool isInOuter = _x >= outerMin && _x <= outerMax && _y >= outerMin && _y <= outerMax;
        bool isInInner = _x >= innerMin && _x <= innerMax && _y >= innerMin && _y <= innerMax;

        return isInOuter && !isInInner;
    }


    int[,] PreRender()
    {
        
        int[,] grid = new int[gridSize, gridSize];   
        
        // Iterate through grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (BossRoomCreator1(x, y))
                {
                    grid[x, y] = 0;
                    continue;
                }
                if (BossRoomCreator2(x, y))
                {
                    grid[x, y] = 1;
                    continue;
                }
                if (BossRoomCreator3(x, y))
                {
                    grid[x, y] = 0;
                    continue;
                }
                if (IsInHallway(x, y))
                {
                    grid[x, y] = 0;
                    continue;
                }
                grid[x, y] = 1;
            }
        }
        return grid;
    }

    void Render(int[,] grid)
    {
        // Destroy all children
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        GameObject floor = Instantiate(floorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        int gridInc = 30;
        floor.transform.localScale = new Vector3(gridSize+gridInc, 1, gridSize+gridInc);
        floor.transform.position = new Vector3((gridSize/2 - 0.5f)*10, 0, (gridSize/2 - 0.5f)*10);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] == 1)
                    CreateBlock(x, y);
                
                // CreateBlock(x, y, false);
            }
        }
    }

    void CreateBlock(int x, int y, bool isWall = true)
    {
        Vector3 pos = new(x*10, 0.5f*10, y*10);
        if (!isWall)
            pos.y = -0.5f*10;

        GameObject block = Instantiate(cubePrefab, pos, Quaternion.identity);
        block.transform.parent = transform;
        block.transform.localScale = new Vector3(10, 10, 10);
        block.isStatic = true;
    }

    private void OnDrawGizmos()
    {
        // Render dots
        foreach (var d in dots)
        {
            Vector3 pos = new Vector3(d.x*10, 0, d.y*10);
            // Draw Red sphere
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pos, 0.1f);
        }

        // Render hallways
        foreach (var h in hallways)
        {
            Vector3 a = new Vector3(h.a.x*10, 0, h.a.y*10);
            Vector3 b = new Vector3(h.b.x*10, 0, h.b.y*10);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(a, b);
        }
    }

}
