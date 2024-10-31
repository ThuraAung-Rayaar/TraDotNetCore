// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Data.SqlClient;
using traDotNetCore.ConsoleApp;

Console.WriteLine("Hello, World!");
int a = 50;
Console.WriteLine(Math.Pow(a, 121));
Console.WriteLine("TEsting IN process");
Console.ReadKey();

AdoDotNetExample2 ado = new AdoDotNetExample2();
ado.Read();

//ado.CreateBlog();
DapperExample2 ddp = new DapperExample2();
//ddp.DeleteItem();
//ado.EditBlog();
//ado.ReadOne();

ddp.ReadOnly();
ddp.Update("Kohee", "ploutsiu", "Vyyyyyyyu gucsg uigoud ud");
ddp.ReadBlog();
Console.ReadKey();

//EfCoreExample efCore = new EfCoreExample();
//efCore.Read();
//efCore.Create("Bla Bla", "Bla Bla BLA", "Bla Bla BLA Bla Bla BLA");
//efCore.Delete(13);

/*DapperExample dap = new DapperExample();
dap.Read();
dap.Delete(3);
dap.ReadOnly();*/
//dap.Update("POpo", "Nolar","Name Change to Nolar");
//dap.Create("Moli", "Nolan", "He is a moli Nolen");

//adoDotnetExample example = new adoDotnetExample();
//example.readTable();
//example.DeleteData();
//example.readTable();
//example.Update();
//example.InsertData();
//example.ReadOnly();
