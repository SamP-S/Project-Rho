using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class path_finder : MonoBehaviour
{
    [SerializeField]
    public static int m = 10;

    [SerializeField]
    public static int n = 10;

    [SerializeField]
    public static Stack<int[]> locations = new Stack<int[]>(); 
    public static bool validate_move(Wall[,] maze, int [] move,String move_name,int i, int j, int m, int n)
    {
        Wall current_cell = maze[i,j];
        int x = move[0];
        int y = move[1];
        Debug.Log("x: " + x.ToString() + " Y: " + y.ToString());
        if(i>=0 && i<m && j>=0 && j<n)
        {
            if(move_name == "UP")
            {
                Wall next_cell = maze[i+1,j];
                if(current_cell.HasFlag(Wall.UP) == true || next_cell.HasFlag(Wall.DOWN) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if(move_name == "DOWN")
            {
                Wall next_cell = maze[i-1,j];
                if(current_cell.HasFlag(Wall.DOWN) == true || next_cell.HasFlag(Wall.UP) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if(move_name == "left")
            {
                Wall next_cell = maze[i,j-1];
                if(current_cell.HasFlag(Wall.LEFT) == true || next_cell.HasFlag(Wall.RIGHT) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if(move_name == "right")
            {
                Wall next_cell = maze[i,j+1];
                if(current_cell.HasFlag(Wall.RIGHT) == true || next_cell.HasFlag(Wall.LEFT) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
        {
            return false;
        }
        }
        else
        {
            return false;
        }
    }
    public static int[] get_move(Wall[,] maze, int i, int j, int m, int n)
    {
        int [] up = new int [2] {i+1,j};
        int [] down = new int [2] {i-1,j};
        int [] left = new int [2] {i,j-1};
        int [] right = new int [2] {i,j+1};
        List<int[]> possible_moves = new List<int[]>();
        
        if((i>=0 || i<=m-1) &&(j>=0 || j<=n-1))
        {
            //bottom left corner; start state (0,0) can move 'UP' and 'RIGHT'
            if(i == 0 && j == 0)
            {         
                Debug.Log("1 called");
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
            //top left corner; can go right and down
            if(i == m-1 && j == 0)
            {       
                Debug.Log("2 called");   
                if(validate_move(maze,down,"down",i,j,m,n) == true)
                {
                    possible_moves.Add(down);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
            //top right corner; reached exit
            //bottom right corner; can go left and up
            if(i == 0 && j == n-1)
            {     
                Debug.Log("3 called");
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,left,"left",i,j,m,n) == true)
                {
                    possible_moves.Add(left);
                }
            }

            //bottom edge; can go left,right,up
            if(i == 0 && j<=n-1 && j>0)
            {      
                Debug.Log("4 called");
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,left,"left",i,j,m,n) == true)
                {
                    possible_moves.Add(left);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
            //top edge; can go left,right,down
            if(i == m-1 && j<=n-1 && j>0)
            {    
                Debug.Log("5 called");
                if(validate_move(maze,down,"down",i,j,m,n) == true)
                {
                    possible_moves.Add(down);
                }
                if(validate_move(maze,left,"left",i,j,m,n) == true)
                {
                    possible_moves.Add(left);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
            //left edge; can go up,right,down
            if(j == 0 && i<=m-1 && i>0)
            {  
                Debug.Log("6 called");    
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,down,"down",i,j,m,n) == true)
                {
                    possible_moves.Add(down);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
            //right edge; can go up,left,down
            if(j == n-1 && i<=m-1 && i>0)
            {        
                Debug.Log("7 called");   
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,left,"left",i,j,m,n) == true)
                {
                    possible_moves.Add(left);
                }
                if(validate_move(maze,down,"down",i,j,m,n) == true)
                {
                    possible_moves.Add(down);
                }
            }
            //somewhere in the middle
            else if(i>0 && i<m && j>0 && j<n)
            {   
                Debug.Log("8 called");         
                if(validate_move(maze,up,"up",i,j,m,n) == true)
                {
                    possible_moves.Add(up);
                }
                if(validate_move(maze,left,"left",i,j,m,n) == true)
                {
                    possible_moves.Add(left);
                }
                if(validate_move(maze,down,"down",i,j,m,n) == true)
                {
                    possible_moves.Add(down);
                }
                if(validate_move(maze,right,"right",i,j,m,n) == true)
                {
                    possible_moves.Add(right);
                }
            }
        }
        if(possible_moves.Count == 0)
        {
            int[] null_val = new int [2] {100000,0};
            return null_val;
        }
        else
        {
            System.Random rd = new System.Random();
            int len = possible_moves.Count;
            int ch = rd.Next(0,len-1);
            return possible_moves[ch];
        }
    }

    public static void recursive_stack(Wall[,] maze, int m, int n)
    {
        Stack<int[]> positions = new Stack<int[]>();

        int[] final_cell = new int [2] {m-1,n-1};
        int i = 0;
        int j = 0;
        int[] start_cell = new int[] {0,0};
        positions.Push(start_cell);
        while(positions.Count > 0)
        {
            int[] top  = positions.Peek();
            if(top != final_cell)
            {
                int[] move = get_move(maze,i,j,m,n);
                Debug.Log("i: " + i + "j: " + j);
                if(move[0] == 100000)
                {
                    int[] x = positions.Pop();
                    i = x[0];
                    j = x[1];
                    continue;
                }
                else
                {
                    positions.Push(move);
                    i = move[0];
                    j = move[1];
                    continue;
                }
            }
            else
            {
                Debug.Log("Path Found");
                break;
            }
        }
       
    }
    public static bool draw_path(Wall[,] maze, int m, int n, int i, int j)
    {
        while(locations.Count > 1)
        {
            //last cell then break the loop
            if(locations.Peek()[0] == m-1 && locations.Peek()[1] == n-1)
            {
                locations.Clear();
                return true;
            }
            //if reached dead end pop 
            else if(locations.Peek()[0] == 100000 && locations.Peek()[1] == 0)
            {
                locations.Pop();
                int[] next_move = get_move(maze,i,j,m,n);
                locations.Push(next_move);
            }
            //else get next cell 
            else
            {
                int[] next_move = get_move(maze,i,j,m,n);
                locations.Push(next_move);
            }
        }
        return false;
    }
        
    //Calls the above function till path found or stack empty
    public static void initial_call(Wall[,] maze, int m, int n)
    {
        //first element in stack
        int[] first = new int[2]{-1,-1};
        locations.Push(first);
        for (int j = 0; j < m; j++)
        {
            if(draw_path(maze,m,n,j,0) == false)
            {
                Debug.Log("Path Not found for: " + j + "0");                
            }
            else
            {
                Debug.Log("Path found!!! for: " + j + "0");
                locations.Push(first);
                break;
            }
        }
        Debug.Log("Checkpoint-1");

    }

    // public static void func(Wall[,] maze, int m, int n)
    // {        
    //     for (int i = 0; i < m; i++)
    //     {
    //         for (int j = 0; j < n; j++)
    //         {
    //             if(recursive_stack(maze,i,j,m,n) == true)
    //             {
    //                 Console.WriteLine("Path Found");
    //                 break;
    //             }
    //         }
    //     }

    // }

    // private void Start()
    // {
    //     var maze = Maze_Generator.Create_Maze(m,n);
    //     func(maze,m,n);

    // }
}
