using Newtonsoft.Json;
using RestSharp;

namespace DotNetTraining.TRA.ConsoleApp3;

public class ClientRestExample
{
    readonly string _path = "https://jsonplaceholder.typicode.com/posts";
    readonly RestClient _client = new RestClient();

    public async Task ReadAllAsync()
    {
        RestRequest request = new RestRequest(_path, Method.Get);
        var response = await _client.GetAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task ReadOneAsync(int id)
    {
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Get);
        var response = await _client.GetAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task UpdateAsync(int id)
    {
        rootModel rootObj = new rootModel()
        {
            userId = 2,
            body = "kdjdjdjcd",
            title = "popopo"
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Patch);
        request.AddJsonBody(jsonStr);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task CreateAsync()
    {
        rootModel rootObj = new rootModel()
        {
            id = 1,
            userId = 2,
            body = "kdjdjdjcd",
            title = "popopo"
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        RestRequest request = new RestRequest(_path, Method.Post);
        request.AddJsonBody(jsonStr);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task DeleteAsync(int id)
    {
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Delete);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content + response.StatusCode);
    }
}


public class ClientRest2Example
{
    readonly string _path;


    readonly RestClient _client;

    public ClientRest2Example(string path,RestClient client)
    {
        _path = path;
        _client = client;
    }

    public async Task ReadAllAsync()
    {
        RestRequest request = new RestRequest(_path, Method.Get);
        var response = await _client.GetAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task ReadOneAsync(int id)
    {
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Get);
        var response = await _client.GetAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task UpdateAsync(int id)
    {
        rootModel rootObj = new rootModel()
        {
            userId = 2,
            body = "kdjdjdjcd",
            title = "popopo"
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Patch);
        request.AddJsonBody(jsonStr);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task CreateAsync()
    {
        rootModel rootObj = new rootModel()
        {
            id = 1,
            userId = 2,
            body = "kdjdjdjcd",
            title = "popopo"
        };
        string jsonStr = JsonConvert.SerializeObject(rootObj);
        RestRequest request = new RestRequest(_path, Method.Post);
        request.AddJsonBody(jsonStr);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content);
    }

    public async Task DeleteAsync(int id)
    {
        RestRequest request = new RestRequest(_path + $"/{id}", Method.Delete);
        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine(response.StatusCode);
        else
            Console.WriteLine(response.Content + response.StatusCode);
    }
}

