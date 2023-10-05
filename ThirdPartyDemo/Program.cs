using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var client = new HttpClient();
var discover = client.GetDiscoveryDocumentAsync("http://localhost:5260")
    .Result;
if (discover.IsError)
{
    Console.WriteLine(discover.Error);
    return;
}

var tokenRequest = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discover.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api"
});

if (tokenRequest.IsError)
{
    Console.WriteLine(tokenRequest.Error); 
    return;
}

Console.WriteLine(tokenRequest.Json);

var client2 = new HttpClient();
client2.SetBearerToken(tokenRequest.AccessToken);

var response = await client2.GetAsync("http://localhost:5159/weatherforecast");
if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JArray.Parse(content));
}
else
    Console.WriteLine(response.StatusCode);

Console.ReadKey();