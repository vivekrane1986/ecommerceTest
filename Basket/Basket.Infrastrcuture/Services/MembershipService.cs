using Basket.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Basket.Infrastrcuture.Services;

public class MembershipService : IMembershipService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public MembershipService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }


    public async Task<int> GetMembershipDiscountAsync(string customerId)
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientConsts.MembershipDataHttpClient);

        var response = await httpClient.GetAsync($"{_config["Membership:Endpoint"]}{customerId}");

        if(response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception($"Error Calling Membership API statuscode {response.StatusCode}.");
        }

        return Convert.ToInt32(await response.Content.ReadAsStringAsync());
    }
}
