using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using Microsoft.AspNetCore.Http;

namespace WebApp.Helpers
{
    public class UserNameProvider : IUserNameProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserNameProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUserName  => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "-";

    }
}