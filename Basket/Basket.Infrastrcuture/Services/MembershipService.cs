using Basket.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Basket.Infrastrcuture.Services;

public class MembershipService : IMembershipService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public MembershipService(HttpClient httpClient,IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }


    public async Task<int> GetMembershipDiscountAsync(string customerId)
    {
        var response = await _httpClient.GetAsync($"{_config["Membership.Endpoint"]}{customerId}");
        return Convert.ToInt32(await response.Content.ReadAsStringAsync());
    }
}
