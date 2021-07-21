using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum Wall
{
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4
}

public static class Maze_Generator 
{
    //Dimensions of Maze is 'm' -> Length of Maze (|) , 'n' -> Breadth of Maze (_)
    public static Wall[,] Create_Maze(int m, int n)
    {
        Wall [,] maze = new Wall[m,n];
        Wall inital_state = Wall.UP | Wall.DOWN | Wall.LEFT | Wall.RIGHT;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                maze[i,j] = inital_state;
            }
        }
        return maze;
    }    

}
