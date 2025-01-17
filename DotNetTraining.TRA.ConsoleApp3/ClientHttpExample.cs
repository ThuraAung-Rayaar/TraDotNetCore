using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DotNetTraining.TRA.ConsoleApp3;

public class ClientHttpExample
{
    readonly string path = "https://jsonplaceholder.typicode.com/posts";
    



    public async Task ReadAllAsync()
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(path);

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else
        {


            string jsonStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr);


        }
    }

    public async Task ReadOneAsync(int id)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(path + $"/{id}");

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else
        {


            string jsonStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr);


        }
    }

    public async Task UpdateAsync(int id)
    {
        HttpClient client = new HttpClient();

        rootModel rootObj = new rootModel() { 
        userId=2,
        body="kdjdjdjcd",
        title = "popopo"        
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        StringContent content = new StringContent(jsonStr, Encoding.UTF8, Application.Json);
        var response = await client.PatchAsync(path + $"/{id}", content);
       // var response = await client.PutAsync(path + $"/{id}", content);

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else
        {


            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);


        }
    }

    public async Task CreateAsync()
    {
        HttpClient client = new HttpClient();

        rootModel rootObj = new rootModel()
        {   id = 1,
            userId = 2,
            body = "kdjdjdjcd",
            title = "popopo"
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        StringContent content = new StringContent(jsonStr, Encoding.UTF8, Application.Json);
        var response = await client.PostAsync(path , content);

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else
        {


            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);


        }
    }
    public async Task DeleteAsync(int id)
    {
        HttpClient client = new HttpClient();
        var response = await client.DeleteAsync(path + $"/{id}");

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else
        {


            string jsonStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr+response.StatusCode);


        }
    }

}


public class ClientHttp2gExample {

    private readonly string _baseUrl;
    private readonly HttpClient _client;

    public ClientHttp2gExample(string baseUrl, HttpClient client)
    {
        _baseUrl = baseUrl;
        _client = client;
    }

    public async Task ReadAllBlogs()
    {
      //  HttpClient client = new HttpClient();
        var response = await _client.GetAsync(_baseUrl);

        if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

        else                
        {


            string jsonStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr);


        }
    }


}




public class rootModel
{
    public int userId { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
}

