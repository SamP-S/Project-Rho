using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// The maze can be treated as an entity itself
// All the most important data relevant to it can be stored with it
// All functions surrounding it can be kept here aswell
public class Maze : MonoBehaviour
{
    // Not sure of unity's flag system
    // but it works for you so not going to change it
    [Flags]
    public enum Wall {
        LEFT = 1,   // 0001
        RIGHT = 2,  // 0010
        UP = 4,     // 0100
        DOWN = 8,   // 1000
    }

    // Defined all 15 possible wall setups in a cell
    // done according to wall enum above
    // keep they'll be used later
    public enum CellTypes {
        // 4 connections
        CROSSROAD_4 = 0b0000,
        // 3 connections
        TJUNC_3_N = 0b0010,
        TJUNC_3_E = 0b0100,
        TJUNC_3_S = 0b0001,
        TJUNC_3_W = 0b1000,
        // 2 connections
        CORRIDOR_2_H = 0b0011,
        CORRIDOR_2_V = 0b1100,
        PERP_2_NE = 0b1001,
        PERP_2_SE = 0b1010,
        PERP_2_SW = 0b0110,
        PERP_2_NW = 0b0101,
        // 1 connections
        SINGLE_1_N = 0b1101,
        SINGLE_1_E = 0b1011,
        SINGLE_1_S = 0b1110,
        SINGLE_1_W = 0b0111,
        // 0 connections
        CLOSED_0 = 0b1111,
    }

    // MazeCells are entities entirely owned by the maze
    // This allows for more data than just the walls to be kept in each cell
    // This is useful as you we're already putting whether a cell has been visited in the wall enums which is not related
    // tldr: much more readable code
    public struct MazeCell
    {
        public Wall wall;
        public bool isVisited;
    }

    // Gets wall binary combination from position delta
    // Will not work unless abs(delta) = 1 per axis is
    public static Wall GetSharedWallFromDelta(int dx, int dy) {
        Wall wall = 0;
        if (dx == -1)
            wall |= Wall.RIGHT;
        if (dx == 1)
            wall |= Wall.LEFT;
        // might have to be switched as unsure of cell array orientation
        if (dy == 1)
            wall |= Wall.DOWN;
        if (dy == -1)
            wall |= Wall.UP;
        return wall;
    }

    // Gets wall binary of shared wall between p0 and p1
    // Gets p0's wall shared with p1
    public static Wall GetSharedWall(Vector2Int p0, Vector2Int p1) {
        return GetSharedWallFromDelta(p0.x - p1.x, p0.y - p1.y);
    }

    // Pass single wall value as param
    // Will return opposite of that wall type
    // Else 0
    public static Wall GetOppositeWall(Wall wall) {
        switch (wall) {
            case Wall.RIGHT:    return Wall.LEFT;
            case Wall.LEFT:     return Wall.RIGHT;
            case Wall.UP:       return Wall.DOWN;
            case Wall.DOWN:     return Wall.UP;
            // ensure default is safe
            default:            return 0;
        }
    }

    // left public for easy debugging
    [Range(5, 20)]
    public int width = 0;
    [Range(5, 20)]
    public int height = 0;
    [Range(0, 2000)]
    public int seed = 0;
    [Range(5, 10)]
    public int cellSize = 10;

    private int _width = 0;
    private int _height = 0;
    private int _seed = 0;
    private int _cellSize = 10;

    [SerializeField]
    public MazeCell[,] cellArray;

    public Vector2Int entrancePosition;
    public Vector2Int exitPosition;

    public List<Wall> arr;

    [SerializeField]
    public GameObject wallPrefab = null;
    [SerializeField]
    public GameObject floorPrefab = null;
    [SerializeField]
    public GameObject entranceFloorPrefab = null;
    [SerializeField]
    public GameObject exitFloorPrefab = null;
    
    private void Start() {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        // calls maze generation function
        GenerateMaze();
        
        //Debug.Log("created graphics");
        Debug.Log(stopwatch.Elapsed);
    }

    private void Update() {
        // if maze parameters changed, regenerate maze
        if (width != _width || height != _height || seed != _seed || cellSize != _cellSize)
        {
            GenerateMaze();
            _width = width;
            _height = height;
            _seed = seed;
            _cellSize = cellSize;
        }
    }

    // maze generation function a.k.a ApplyRecursiveBacktracker
    private bool GenerateMaze() {
        // temporary
        cellArray = new MazeCell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                cellArray[i, j].wall = Wall.LEFT | Wall.RIGHT | Wall.UP | Wall.DOWN;
            }
        }
        // Debug.Log(Wall.LEFT | Wall.RIGHT | Wall.UP | Wall.DOWN);
        System.Random rng = new System.Random(seed);

        // i do love this stack system, v good use of data structure
        Stack<Vector2Int> positionStack = new Stack<Vector2Int>();
        // adds a start position to the stack
        entrancePosition = new Vector2Int(rng.Next(0, width - 1), rng.Next(0, height - 1));

        //Debug.Log(entrancePosition);
        // ensure to set isVisited
        cellArray[entrancePosition.x, entrancePosition.y].isVisited = true;
        // add to stack to start DFS
        positionStack.Push(entrancePosition);

        while (positionStack.Count > 0) {
            // be careful with pop functions they usually remove the top element
            Vector2Int currentPos = positionStack.Pop();
            List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentPos);
            
            // checks if their is a neighbour
            if (neighbours.Count > 0) {
                positionStack.Push(currentPos);

                int randIndex = rng.Next(0, neighbours.Count - 1);
                Vector2Int randNeighbourPos = neighbours[randIndex];
                cellArray[currentPos.x, currentPos.y].wall &= ~GetSharedWall(currentPos, randNeighbourPos);
                cellArray[randNeighbourPos.x, randNeighbourPos.y].wall &= ~GetSharedWall(randNeighbourPos, currentPos);
                cellArray[randNeighbourPos.x, randNeighbourPos.y].isVisited = true;

                positionStack.Push(randNeighbourPos);
            }
        }
        
        // set exit = entrance
        exitPosition = entrancePosition;
        // get the minimum maximum distance (i.e. entrance in the middle, exit in corner)
        // set as minimum distance threshold between entrance and exit
        float minDist = Mathf.Sqrt(Mathf.Floor(width / 2) * Mathf.Floor(width / 2) + Mathf.Floor(height / 2) * Mathf.Floor(height / 2));
        // continually get new exit positions until atleast minimum distance from entrance
        while(Vector2.Distance(entrancePosition, exitPosition) <  minDist)
        {
            exitPosition = new Vector2Int(rng.Next(0, width - 1), rng.Next(0, height - 1));
        }
        InstanceGraphics();
        return true;
    }

    // Uses if statements to get the neighbouring cells from a given position
    // Checks bounds and allows to limit to only unvisited cells using bool param
    public List<Vector2Int> GetNeightbours(Vector2Int p, bool getUnvisited = false)
    {
        List<Vector2Int> neighbourList = new List<Vector2Int>();
        // Up
        //Debug.Log(p);
        if (p.y < height - 1) {
            if (!cellArray[p.x, p.y + 1].isVisited || !getUnvisited) {
                neighbourList.Add(new Vector2Int(p.x, p.y + 1));
            }
        }

        // Right
        if (p.x < width - 1) {
            if (!cellArray[p.x + 1, p.y].isVisited || !getUnvisited) {
                neighbourList.Add(new Vector2Int(p.x + 1, p.y));
            }
        }

        // Down
        if (p.y > 0) {
            if (!cellArray[p.x, p.y - 1].isVisited || !getUnvisited) {
                neighbourList.Add(new Vector2Int(p.x, p.y - 1));
            }
        }

        // Left
        if (p.x > 0) {
            if (!cellArray[p.x - 1, p.y].isVisited || !getUnvisited) {
                neighbourList.Add(new Vector2Int(p.x - 1, p.y));
            }
        }
        return neighbourList;
    }

    // Reuses GetNeightbours function but sets bool true so only unvisited
    private List<Vector2Int> GetUnvisitedNeighbours(Vector2Int p)
        {
        return GetNeightbours(p, true);
    }

    // https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
    public void ClearChildren() {
        //Debug.Log(transform.childCount);
        int i = 0;
        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];
        //Find all child obj and store to that array
        foreach (Transform child in transform) {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        //Now destroy them
        foreach (GameObject child in allChildren) {
            DestroyImmediate(child.gameObject);
        }
        //Debug.Log(transform.childCount);
    }

    private bool InstanceGraphics() 
    {
        ClearChildren();
        for (int i = 0; i < width; i++)
        {
            arr.Add(cellArray[i, 0].wall);
            for (int j = 0; j < height; j++)
            {
                MazeCell cell = cellArray[i, j];
                GameObject floor;
                Vector2Int cellPos = new Vector2Int(i, j);
                if (cellPos == entrancePosition) 
                    floor = Instantiate(entranceFloorPrefab);
                else if (cellPos == exitPosition)
                    floor = Instantiate(exitFloorPrefab);
                else
                    floor = Instantiate(floorPrefab);
                
                Vector3 floorPos = new Vector3(i * cellSize, 0, j * cellSize);
                Vector3 floorScl = new Vector3(cellSize / 10, 1, cellSize / 10);
                floor.transform.position = floorPos;
                floor.transform.localScale = floorScl;
                floor.transform.parent = this.transform;

                // Left
                if ((cell.wall & Wall.LEFT) == Wall.LEFT) {
                    //Debug.Log("LEFT WALL");
                    GameObject leftWall = Instantiate(wallPrefab);
                    leftWall.name = "WallLeftX" + i.ToString() + "Y" + j.ToString();
                    Vector3 leftWallPos = new Vector3(i * cellSize - cellSize / 2, cellSize / 2, j * cellSize);
                    // leftWall.transform.Rotate(new Vector3(0, 90, 0));
                    leftWall.transform.position = leftWallPos;
                    leftWall.transform.parent = this.transform;
                }
                // Right
                if ((cell.wall & Wall.RIGHT) == Wall.RIGHT) {
                    //Debug.Log("RIGHT WALL");
                    GameObject rightWall = Instantiate(wallPrefab);
                    rightWall.name = "WallRightX" + i.ToString() + "Y" + j.ToString();
                    Vector3 rightWallPos = new Vector3(i * cellSize + cellSize / 2, cellSize / 2, j * cellSize);
                    rightWall.transform.position = rightWallPos;
                    rightWall.transform.Rotate(new Vector3(0, 180, 0));
                    rightWall.transform.parent = this.transform;
                }
                // Up
                if ((cell.wall & Wall.UP) == Wall.UP) {
                    //Debug.Log("UP WALL");
                    GameObject upWall = Instantiate(wallPrefab);
                    upWall.name = "WallUpX" + i.ToString() + "Y" + j.ToString();
                    Vector3 upWallPos = new Vector3(i * cellSize, cellSize / 2, j * cellSize + cellSize / 2);
                    upWall.transform.position = upWallPos;
                    upWall.transform.Rotate(new Vector3(0, 270, 0));
                    upWall.transform.parent = this.transform;
                }
                // Down
                if ((cell.wall & Wall.DOWN) == Wall.DOWN) {
                    //Debug.Log("DOWN WALL");
                    GameObject downWall = Instantiate(wallPrefab);
                    downWall.name = "WallDownX" + i.ToString() + "Y" + j.ToString();
                    Vector3 downWallPos = new Vector3(i * cellSize, cellSize / 2, j * cellSize - cellSize / 2);
                    downWall.transform.position = downWallPos;
                    downWall.transform.Rotate(new Vector3(0, 90, 0));
                    downWall.transform.parent = this.transform;
                }
            }
        }
        return true;
    }
}