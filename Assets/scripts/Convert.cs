using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Convert : UnauthorizedAccessException
{
    
    public static void display_txt_array(string[,,] maze_structure ,int m, int n)
    {
        string [] label = new string[4]{"UP", "DOWN", "LEFT", "RIGHT"};
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int cell_no = (i*10) + j;
                Debug.Log("Cell - " + cell_no);
                for (int k = 0; k < 4; k++)
                {
                    
                    Debug.Log("\t" + label[k] + " : " + maze_structure[i,j,k]);
                }
            }
        }
    }
    public static void Array_to_Txt(Wall[,] maze,string path, int m, int n)
    {
        List<string> array = new List<string>();
        string[,,] maze_structure = new string[m,n,4];
        string t = "t";
        string f = "f";
        string all_lines = m + " " + n + "\n";
        for (int i = 0; i < m; i++)
        {
            string first_line = "";
            for (int j = 0; j < n; j++)
            {
                int cell_no = (i*10) + j;
                Wall cell = maze[i,j];
                string content = "";
                // 1 - UP, 2 - DOWN, 3 - LEFT, 4 - RIGHT
                if(cell.HasFlag(Wall.UP) == true)
                {
                    maze_structure[i,j,0] = t;
                }
                else if(cell.HasFlag(Wall.UP) == false)
                {
                    maze_structure[i,j,0] = f;
                }

                if(cell.HasFlag(Wall.DOWN) == true)
                {
                    maze_structure[i,j,1] = t;
                }
                else if(cell.HasFlag(Wall.DOWN) == false)
                {
                    maze_structure[i,j,1] = f;
                }

                if(cell.HasFlag(Wall.LEFT) == true)
                {
                    if(i == 0 && j == 0)
                    {
                        maze_structure[i,j,2] = f;
                    }
                    else
                    {
                        maze_structure[i,j,2] = t;
                    }
                }
                else if(cell.HasFlag(Wall.LEFT) == false)
                {
                    maze_structure[i,j,2] = f;
                }

                if(cell.HasFlag(Wall.RIGHT) == true)
                {
                    if(i == m-1 && j == n-1)
                    {
                        maze_structure[i,j,3] = f;
                    }
                    else
                    {
                        maze_structure[i,j,3] = t;
                    }
                }
                else if(cell.HasFlag(Wall.RIGHT) == false)
                {
                    maze_structure[i,j,3] = f;
                }

                content = maze_structure[i,j,0] + maze_structure[i,j,1] + maze_structure[i,j,2] + maze_structure[i,j,3] + "\n";
                first_line = first_line + content;
            }
            all_lines = all_lines + first_line;
        }
        if(!File.Exists(path))
        {
            File.WriteAllText(path,all_lines);
        }
        else
        {
            //File.Create(path);
            File.Delete(path);
            File.WriteAllText(path,all_lines);
        }
    }
    public static Wall[,] Txt_to_Array(string path)
    {
        if(!File.Exists(path))
        {
            Debug.Log("Error File Does Not Exists");
            return null;
        }
        else
        {
            using (StreamReader ReaderObject = new StreamReader(path))
            {
                string line;
                List<string> lines = new List<string>();
                // ReaderObject reads a single line, stores it in Line string variable and then displays it on console
                while((line = ReaderObject.ReadLine()) != null)
                { 
                    lines.Add(line);
                }
                //Then Maze Dimensions are a single digit value
                int m = 0;
                int n = 0;
                if(lines[0][1] == ' ')
                {
                    m = int.Parse(lines[0][0].ToString());
                    n = int.Parse(lines[0][2].ToString());
                }
                else
                {
                    m = int.Parse(lines[0][0].ToString() + lines[0][1].ToString());
                    n = int.Parse(lines[0][3].ToString() + lines[0][4].ToString());
                }
                Wall [,] maze = new Wall[m,n];
                int index = 0;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Wall cell = new Wall();
                        index += 1;
                        cell = Wall.UP | Wall.DOWN | Wall.LEFT | Wall.RIGHT;
                        //if Wall.Up is false
                        if(lines[index][0] == 'f')
                        {
                            cell &= ~Wall.UP;
                        }
                        //if Wall.DOWN is false
                        if(lines[index][1] == 'f')
                        {
                            cell &= ~Wall.DOWN;
                        }
                        //if Wall.LEFT is false
                        if(lines[index][2] == 'f')
                        {
                            if(index != 1)
                            {
                                cell &= ~Wall.LEFT;
                            }
                        }
                        //if Wall.RIGHT is false
                        if(lines[index][3] == 'f')
                        {
                            if(index != 100)
                            {
                                cell &= ~Wall.RIGHT;
                            }
                        }
                        maze[i,j] = cell;
                    }
                }
                //Debug.Log(maze.Length);
                return maze;
            }
        }
    }

    public static int[] get_dimensions(string path)
    {
        if(!File.Exists(path))
        {
            Debug.Log("Error File Does Not Exists");
            return null;
        }
        else
        {
            using (StreamReader ReaderObject = new StreamReader(path))
            {
                string line;
                List<string> lines = new List<string>();
                // ReaderObject reads a single line, stores it in Line string variable and then displays it on console
                while((line = ReaderObject.ReadLine()) != null)
                { 
                    lines.Add(line);
                }
                //Then Maze Dimensions are a single digit value
                int m = 0;
                int n = 0;
                if(lines[0][1] == ' ')
                {
                    m = int.Parse(lines[0][0].ToString());
                    n = int.Parse(lines[0][2].ToString());
                    Debug.Log("N: " + n);
                }
                else
                {
                    m = int.Parse(lines[0][0].ToString() + lines[0][1].ToString());
                    n = int.Parse(lines[0][3].ToString() + lines[0][4].ToString());
                }
                int[] dim = new int[]{m,n};
                return dim;
            }
        }
    }
}
