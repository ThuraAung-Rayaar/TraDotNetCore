// See https://aka.ms/new-console-template for more information
using DotNetTraining.TRA.ConsoleApp3;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Refit;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;

var serviceCollection = new ServiceCollection()
            
            .AddSingleton(provider =>
            {
                var client = provider.GetRequiredService<HttpClient>();
                return new ClientHttp2gExample("https://jsonplaceholder.typicode.com/posts", client);
            })
            .AddSingleton(provider =>
            {

                var client = new RestClient();
                return new ClientRest2Example("https://jsonplaceholder.typicode.com/posts", client);
            }
            )
           .AddRefitClient<IRootModelApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com")).Services
            .AddSingleton<RefitRootExample>()
            .AddHttpClient()
            ;


var serviceProvider = serviceCollection.BuildServiceProvider();
var refitClient = serviceProvider.GetService<RefitRootExample>();
var REstClient = serviceProvider.GetService<ClientRest2Example>();
Console.ReadLine();
await refitClient.GetAsync();
Console.ReadLine();

//ClientHttpExample client = service.ge

/*Console.WriteLine("Hello, World!");

HttpClient client = new HttpClient();
var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");

if (!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);

else {


   string jsonStr = await response.Content.ReadAsStringAsync();
    Console.WriteLine(jsonStr);


}*/
//ClientRestExample clientHttpExample = new ClientRestExample();
//await clientHttpExample.ReadOneAsync(2);
//for(int i = 0; i < 10; i++) Console.Write(":-:-:__:-");
//await clientHttpExample.DeleteAsync(2);



//Console.WriteLine("Wait for API Swagger To Load before Enter:"); Console.ReadLine();

//RefitExample refitExample = new RefitExample();
////await refitExample.CreateBlogAsync("update2");
////await refitExample.DeleteBlogAsync(25);
//await refitExample.GetBlogAsync(100);
//Console.ReadLine();