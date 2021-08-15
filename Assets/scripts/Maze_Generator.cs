using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum Wall
{
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000
    VISITED = 128
}

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbour
{
    public Position Position;
    public Wall SharedWall;
}

public static class Maze_Generator 
{

    public static Wall GetOppositeWall(Wall wall)
    {
        switch (wall)
        {
            case Wall.RIGHT: return Wall.LEFT;
            case Wall.LEFT: return Wall.RIGHT;
            case Wall.UP: return Wall.DOWN;
            case Wall.DOWN: return Wall.UP;
            default: return Wall.LEFT;
        }
    }

    public static Boolean Recursive_path_finder(Wall current_cell ,Wall next_cell, int m, int n)
    {
        if(current_cell.HasFlag(Wall.LEFT) == next_cell.HasFlag(Wall.RIGHT) == false)
        {
            return true;
        }
        else if(current_cell.HasFlag(Wall.RIGHT) == next_cell.HasFlag(Wall.LEFT) == false)
        {
            return true;
        }
        else if(current_cell.HasFlag(Wall.UP) == next_cell.HasFlag(Wall.DOWN) == false)
        {
            return true;
        }
        else if(current_cell.HasFlag(Wall.DOWN) == next_cell.HasFlag(Wall.UP) == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // public static Wall[,] set_entrance_exit(Wall[,] maze, int m, int n)
    // {
    //     var start_cell = maze[0,0];
    //     Stack<bool> path = new Stack<bool>();
    //     for (int i = 0; i < m; i++)
    //     {
    //         for (int j = 0; j < n; j++)
    //         {
                
    //         }
    //     }
    //     return maze;
    // }
    public static Wall[,] ApplyRecursiveBacktracker(Wall[,] maze, int m, int n)
    {
        // here we make changes
        var rng = new System.Random(/*seed*/);
        var positionStack = new Stack<Position>();
        var position = new Position { X = rng.Next(0, m), Y = rng.Next(0, n) };

        maze[position.X, position.Y] |= Wall.VISITED;  // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, m, n);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall);
                maze[nPosition.X, nPosition.Y] |= Wall.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }

    public static List<Neighbour> GetUnvisitedNeighbours(Position p, Wall[,] maze, int m, int n)
    {
        var list = new List<Neighbour>();

        if (p.X > 0) // left
        {
            if (!maze[p.X - 1, p.Y].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = Wall.LEFT
                });
            }
        }

        if (p.Y > 0) // DOWN
        {
            if (!maze[p.X, p.Y - 1].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    SharedWall = Wall.DOWN
                });
            }
        }

        if (p.Y < n - 1) // UP
        {
            if (!maze[p.X, p.Y + 1].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    SharedWall = Wall.UP
                });
            }
        }

        if (p.X < m - 1) // RIGHT
        {
            if (!maze[p.X + 1, p.Y].HasFlag(Wall.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = Wall.RIGHT
                });
            }
        }

        return list;
    }
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
        return ApplyRecursiveBacktracker(maze,m,n);
    }    

}
