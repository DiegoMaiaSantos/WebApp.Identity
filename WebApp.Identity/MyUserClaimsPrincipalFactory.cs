using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace WebApp.Identity
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<MyUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<MyUser> userManager, 
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
                
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MyUser myUser)
        {
            var identity = await base.GenerateClaimsAsync(myUser);
            identity.AddClaim(new Claim("Member", myUser.Member));

            return identity;
        }
    }
}
