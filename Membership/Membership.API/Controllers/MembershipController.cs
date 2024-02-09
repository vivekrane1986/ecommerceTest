using Microsoft.AspNetCore.Mvc;

namespace Membership.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MembershipController : ControllerBase
    {
        public MembershipController()
        {
           
        }

        [HttpGet(Name = "GetMembership")]
        [Route("{customerId}")]
        public Task<string> Get(string customerId)
        {
            return Task.FromResult("Yes");
        }
    }
}
