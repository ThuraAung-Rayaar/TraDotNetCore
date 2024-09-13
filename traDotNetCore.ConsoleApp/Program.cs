// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Data.SqlClient;

Console.WriteLine("Hello, World!");
int a = 50;
Console.WriteLine(Math.Pow(a, 121));
Console.WriteLine("TEsting IN process");
//Console.ReadKey();



string connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sa@123sa@123;";
Console.WriteLine("Connecting String: " + connectionString);
SqlConnection connection = new SqlConnection(connectionString);
Console.WriteLine("Opening sql Connection");
connection.Open();
Console.WriteLine("Sql Connection OPened");

string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
SqlCommand cmd = new SqlCommand(query, connection);

//SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//DataTable dt = new DataTable();
//adapter.Fill(dt);
SqlDataReader reader = cmd.ExecuteReader();

while (reader.Read()) {

    Console.WriteLine(reader["BlogId"]);
    Console.WriteLine(reader["BlogTitle"]);
    Console.WriteLine(reader["BlogAuthor"]);
    Console.WriteLine(reader["BlogContent"]);
    Console.WriteLine(reader["DeleteFlag"]);


}


/*foreach (DataRow vr in dt.Rows)
{
    Console.WriteLine(vr["BlogId"]);
    Console.WriteLine(vr["BlogTitle"]);
    Console.WriteLine(vr["BlogAuthor"]);
    Console.WriteLine(vr["BlogContent"]);
    Console.WriteLine(vr["DeleteFlag"]);
}*/


Console.WriteLine("Closing sql Connection");
connection.Close();
Console.WriteLine("Sql Connection closed");

Console.ReadKey();


/*foreach (DataRow vr in dt.Rows)
{
    Console.WriteLine(vr["BlogId"]);
    Console.WriteLine(vr["BlogTitle"]);
    Console.WriteLine(vr["BlogAuthor"]);
    Console.WriteLine(vr["BlogContent"]);
    Console.WriteLine(vr["DeleteFlag"]);
}*/