using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Maze_Renderer : MonoBehaviour
{
    [SerializeField]
    public int m = 10;

    [SerializeField]
    public int n = 10;
    // Start is called before the first frame update

    [SerializeField]
    public int number_of_levels = 5;
    
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

    public List<Transform> grid = new List<Transform>();
    public void Draw_Maze(Wall[,] maze,int m,int n)
    {
        var floor = Instantiate(Floorprefab, transform);
        floor.localScale = new Vector3(10,1,10);
        grid.Add(floor);

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
                    grid.Add(top_wall);
                }
                if (cell.HasFlag(Wall.LEFT))
                {
                    if(i == 0 && j == 0)
                    {
                        var leftWall = Instantiate(entrancePrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size, 0, -size/2);
                        leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                        leftWall.eulerAngles = new Vector3(0, 0, 0);
                        grid.Add(leftWall);
                    }
                    else
                    {
                        var leftWall = Instantiate(wallPrefab, transform) as Transform;
                        leftWall.position = position + new Vector3(-size / 2, 0, 0);
                        leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                        leftWall.eulerAngles = new Vector3(0, 90, 0);
                        grid.Add(leftWall);
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
                            grid.Add(rightWall);
                        }
                        else
                        {
                            var rightWall = Instantiate(wallPrefab, transform) as Transform;
                            rightWall.position = position + new Vector3(+size / 2, 0, 0);
                            rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                            rightWall.eulerAngles = new Vector3(0, 90, 0);
                            grid.Add(rightWall);
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
                        grid.Add(bottomWall);
                    }
                }
            }
        }        

    }
    public bool compare(string folder, int m, int n, int index, int offset)
    {
        bool result = true;
        Debug.Log("Entered Compare: Maze_" + index);
        int j = index-1;
        for(int i = index; i > offset+1; i--)
        {
            string path = folder + @"\Maze_" + index + ".txt";
            string prev_path = folder + @"\Maze_" + j + ".txt";
            
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            System.IO.StreamReader prev_file = new System.IO.StreamReader(prev_path);
            int pointer = 0;
            List<bool> match = new List<bool>();
            while(pointer <= (m*n))
            {
                pointer = pointer + 1;
                string file_line = file.ReadLine();
                string prev_file_line = prev_file.ReadLine();

                //If Both the Files [ie, the mazes] are equal then return false;
                if(string.Equals(file_line,prev_file_line) == true)
                {
                    match.Add(false);
                }
                else
                {
                    match.Add(true);
                }
            }
            int t = 0;
            int f = 0;
            for (int k = 0; k < match.Count; k++)
            {
                if(match[k] == true)
                {
                    t = t+1;
                }
                else
                {
                    f = f+1; 
                }
            }
            //If the Mazes are less than 50% the same then they are diferent else they are very similar
            if(f>t)
            {
                file.Close();
                prev_file.Close();
                Debug.Log("Maze_" + index + " and Maze_" + j + " are very Similar");
                result = false;
                return result;
            }
            else
            {
                file.Close();
                prev_file.Close();
                Debug.Log("Maze_" + index + " and Maze_" + j + " are Different");
                result = true;
            }
            j = j - 1;
        }
        Debug.Log("Exited Compare");
        return result;
    }
    public void delete_maze()
    {
        var clones = GameObject.FindGameObjectsWithTag ("Finish");    
        foreach (var clone in clones) {
                Destroy(clone);
            }
    }
    public void generate_mazes()
    {
        int folder_index = 1;
        int offset = 0;
        //Put 5 mazes in each Folder starting from 5 to 30
        for(int i = 0; i <= 35; i++)
        {
            string folder = @"D:\Maze Game Project\Project-Rho\Assets\Mazes\Level_Set_" + folder_index;
            if(!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            for (int j = 1; j <= number_of_levels; j++)
            {
                int file_index = (i*5) + j;
                string path = folder + @"\Maze_" + file_index + ".txt";
                Convert.Array_to_Txt(Maze_Generator.Create_Maze(m,n),path,m,n);
                int x = 0;
                bool result = compare(folder,m,n,file_index,offset);
                while(result == false)
                {   
                    x = x+1;
                    Debug.Log("Maze_" + file_index + ": False - " + x);
                    Convert.Array_to_Txt(Maze_Generator.Create_Maze(m,n),path,m,n);
                    result = compare(folder,m,n,file_index,offset);
                }
                Debug.Log("Maze_" + file_index + "Generated Sucessfully");
            }
            m += 1;
            n += 1;
            folder_index += 1;
            offset += 5;
        }

    }    
    void Start()
    {     
        //generate_mazes();
        //Only use the above code to create Mazes of dimensions from 5 to 35 and store it in 31 folders  
        //Only call the create function to generate 5 levels for a maze of a given dimension in a folder
    }
}
