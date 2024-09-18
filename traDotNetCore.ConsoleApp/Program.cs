// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Data.SqlClient;
using traDotNetCore.ConsoleApp;

Console.WriteLine("Hello, World!");
int a = 50;
Console.WriteLine(Math.Pow(a, 121));
Console.WriteLine("TEsting IN process");
//Console.ReadKey();


adoDotnetExample example = new adoDotnetExample();
example.readTable();
example.DeleteData();
example.readTable();
//xample.Update();
//example.InsertData();
//example.ReadOnly();
