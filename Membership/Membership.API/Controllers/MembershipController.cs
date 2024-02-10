using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Membership.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
    public class MembershipController : ControllerBase
    {
        public MembershipController()
        {

        }

        [HttpGet("{customerId}", Name = "GetMembershipDiscount")]        
        public Task<int> Get(string customerId)
        {   
            return Task.FromResult(20);
        }
    }
}
