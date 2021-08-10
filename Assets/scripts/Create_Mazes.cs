using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Mazes : MonoBehaviour
{
    [SerializeField]
    public int m = 5;
    [SerializeField]
    public int n = 5;
    [SerializeField]
    public int number_of_levels = 5;
    // Start is called before the first frame update
    // void Start()
    // {
    //     for (int k = 1; k <= number_of_levels; k++)
    //     {
    //         string path = @"D:\Maze Game Project\Project-Rho\Assets\Mazes\Maze_" + k + ".txt";
    //         var maze = Maze_Generator.Create_Maze(m,n);
    //         Convert.Array_to_Txt(maze,pathm,n);
    //     }   
    // }
}
