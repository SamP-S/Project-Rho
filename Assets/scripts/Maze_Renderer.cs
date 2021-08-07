using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Maze_Renderer : MonoBehaviour
{
    [SerializeField]
    public  int m = 10;

    [SerializeField]
    public int n = 10;
    // Start is called before the first frame update

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform Floorprefab = null;
    
    [SerializeField]
    private Transform wallPrefab = null;
    [SerializeField]
    private Transform entrancePrefab = null;
    [SerializeField]
    private Transform exitPrefab = null;
    public void Draw_Maze(Wall[,] maze)
    {
        var floor = Instantiate(Floorprefab, transform);
        floor.localScale = new Vector3(10,1,10);

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var cell = maze[i,j];
                var position = new Vector3(-m/2+i, 0, -n/2+j);
                
                if(cell.HasFlag(Wall.UP))
                {
                    var top_wall = Instantiate(wallPrefab, transform) as Transform;
                    top_wall.position = position + new Vector3(0, 0, size / 2);
                    top_wall.localScale = new Vector3(size, top_wall.localScale.y, top_wall.localScale.z);
                }
                if (cell.HasFlag(Wall.LEFT))
                {
                    if(i == 0 && j == 0)
                    {
                        var leftWall = Instantiate(entrancePrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size, 0, -size/2);
                        leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                        leftWall.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        var leftWall = Instantiate(wallPrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size / 2, 0, 0);
                        leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                        leftWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (i == m - 1)
                {
                    if (cell.HasFlag(Wall.RIGHT))
                    {
                        if(j == n - 1)
                        {
                            var rightWall = Instantiate(exitPrefab, transform) as Transform;
                            rightWall.position = position + new Vector3(+size / 2, 0, size/2);
                            rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                            rightWall.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            var rightWall = Instantiate(wallPrefab, transform) as Transform;
                            rightWall.position = position + new Vector3(+size / 2, 0, 0);
                            rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                            rightWall.eulerAngles = new Vector3(0, 90, 0);
                        }
                        
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(Wall.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }        

    }
    void Start()
    {
        var maze = Maze_Generator.Create_Maze(m,n);
        Draw_Maze(maze);
        // path_finder.initial_call(maze,m,n);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}