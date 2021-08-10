using System;
using System.IO;
using Unity;
using UnityEngine;
using System.Collections;
public class Testing : MonoBehaviour
{
   public static void Main()
   {
      string path = @"D:\Maze Game Project\Project-Rho\Assets\Mazes\Test.txt";
      string content = "Just Created this Test.txt File";
      if(!File.Exists(path))
      {
         File.WriteAllText(path,"Login Log \n\n");
      }
      
   }
}
