// See https://aka.ms/new-console-template for more information
using DotNetBatch5.Database.Models;
using System;
using Newtonsoft.Json;
Console.WriteLine("Hello, World!");



/*Human nnn = new Human("Mg Mg",19,146.2);

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
*/



var GID = Guid.NewGuid();
Console.WriteLine(GID);

var GID2 = Guid.NewGuid();
Console.WriteLine(GID2);

string formatGID = GID.ToString().Replace('-', '\0').ToUpper();
formatGID = formatGID.Substring(0, 4) + formatGID.Substring(30, 2);
Console.WriteLine(formatGID);

string formatGID2 = GID2.ToString().Replace('-', '\0').ToUpper();
formatGID2 = formatGID2.Substring(0, 4) + formatGID2.Substring(30, 2);
Console.WriteLine(formatGID2);

var UID = Ulid.NewUlid().ToString();
Console.WriteLine(UID);
var formatUID = UID.Substring(2, 2) + UID.Substring(UID.Length / 2, 2) + UID.Substring(UID.Length-3,2);
Console.WriteLine(formatUID);

var UID2 = Ulid.NewUlid().ToString();
Console.WriteLine(UID2);
var formatUID2 = UID2.Substring(2, 2)+ UID2.Substring(UID.Length/2, 2) + UID2.Substring(UID2.Length - 3, 2);
Console.WriteLine(formatUID2);






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


