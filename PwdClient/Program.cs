using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var client = new HttpClient();

var discover = await client.GetDiscoveryDocumentAsync("http://localhost:5260");
if (discover.IsError)
{
    Console.WriteLine(discover.Json);
    return;
}

var tokenRequest = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
{
    Address = discover.TokenEndpoint,
    ClientId = "pwdclient",
    ClientSecret = "secret",
    Scope = "api",
    UserName = "jack",
    Password = "123456"
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
    var responseStr = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JArray.Parse(responseStr));
}
else Console.WriteLine(response.StatusCode);

Console.ReadKey();
