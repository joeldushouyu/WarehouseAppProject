using System;
using Flurl.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShellDemo.Models;
using Newtonsoft.Json;
string baseUrl = "http://127.0.0.1:5000";
/*
var product = new Product { Description = "Apples", Price = 23.5 };

Product newProduct = await $"{baseUrl}/products"
    .PostJsonAsync(product)
    .ReceiveJson<Product>();

List<Product> products = await $"{baseUrl}/products"
    .GetJsonAsync<List<Product>>();

foreach (var p in products) {
    Console.WriteLine($"{p.Id} - {p.Description}");
}

Console.WriteLine($"{newProduct.Id} is the new ID number.");

// Try adding a product with an invalid price

try {
    newProduct = await $"{baseUrl}/products"
        .PostJsonAsync(new Product { Description = "Test", Price = -3 })
        .ReceiveJson<Product>();
} catch (FlurlHttpException ex) {
    var apiError = await ex.GetResponseJsonAsync<APIError>();
    Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {apiError.Error}");
}
*/
try
{
    //var x = await $"{baseUrl}/islogin".PostJsonAsync(new User { AccountName = "Jodel", Password = "200367" });
    var res = await $"{baseUrl}/islogin".PostJsonAsync(new User { AccountName = "Jodel", Password = "200367" });
    //Console.WriteLine(x.StatusCode);
}
catch(Exception e)
{
    if(e.GetBaseException().GetType() == typeof(FlurlHttpException))
    {
        Console.WriteLine("");
    }
}


