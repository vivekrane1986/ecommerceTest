namespace Basket.Domain.Services;

public interface IMembershipService
{
    Task<int> GetMembershipDiscountAsync(string customerId);
}
