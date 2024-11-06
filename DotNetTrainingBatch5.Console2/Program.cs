// See https://aka.ms/new-console-template for more information
using DotNetBatch5.Database.Models;
using System;
using Newtonsoft.Json;
Console.WriteLine("Hello, World!");
AppDbContext appDb = new AppDbContext();


Human nnn = new Human("Mg Mg",19,146.2);

Console.WriteLine( nnn.ToJson());
string jsonH = nnn.ToJson();
Console.WriteLine(jsonH.TOClass<Human>().name);

int[] aa = new int[] { 1,2,3,4,5,6,7,8,9,10};
int[] bb = new int[aa.Length-1];
int index = 5-1;
Array.Copy(aa, 0, bb, 0, index);
Array.Copy(aa, index+1, bb, index , 10-index-1);
foreach (int num in bb)
{
    Console.Write(num + " ");
}


public class Human
{   

    public string name { get; set; }
    public int age { get; set; }
    public double Height_cm { get; set; }

   public Human(string name, int age, double height_cm)
    {
        this.name = name;
        this.age = age;
        Height_cm = height_cm;
    }   
}

public static class Dev_Extention 
{

    public static string ToJson(this object obj) { 
    
    return JsonConvert.SerializeObject(obj,Formatting.Indented);
    
    }

    public static T TOClass<T>(this object obj)
    {
        return JsonConvert.DeserializeObject<T>(obj.ToString()!)!;
        // return JsonConvert.SerializeObject(obj, Formatting.Indented);

    }


    
}


