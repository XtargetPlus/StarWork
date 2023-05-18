using BLRequests.Dtos.Profile;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace StarWork3.SubFunctions
{
    public static class ChangeUserClaims
    {
        public static async Task ChangeBaseUserClaimsAsync(this HttpContext context, MyProfileInfoDto profileInfo)
        {
            if (context.User.Identity is ClaimsIdentity identity)
            {
                var email = identity.FindFirst(ClaimTypes.Email);
                if (email is not null && profileInfo.Email is not null && profileInfo.Email != email.Value)
                {
                    identity.RemoveClaim(email);
                    identity.AddClaim(new Claim(ClaimTypes.Email, profileInfo.Email));
                }
                var phone = identity.FindFirst(ClaimTypes.MobilePhone);
                if (phone is not null && profileInfo.Phone is not null && profileInfo.Phone != phone.Value)
                {
                    identity.RemoveClaim(phone);
                    identity.AddClaim(new Claim(ClaimTypes.MobilePhone, profileInfo.Phone));
                }
                var nickName = identity.FindFirst("UserName");
                if (nickName is not null && profileInfo.NickName is not null && profileInfo.NickName != nickName.Value)
                {
                    identity.RemoveClaim(nickName);
                    identity.AddClaim(new Claim("UserName", profileInfo.NickName));
                }
                var claimsPrincipal = new ClaimsPrincipal(identity);
                await context.SignInAsync(claimsPrincipal);
            }
        }

        public static async Task ChangeLoginClaimAsync(this HttpContext context, string newLogin)
        {
            if (context.User.Identity is ClaimsIdentity identity)
            {
                var login = identity.FindFirst(ClaimTypes.Name);
                if (login is not null && newLogin != login.Value)
                {
                    identity.RemoveClaim(login);
                    identity.AddClaim(new Claim(ClaimTypes.Name, newLogin));
                }
                var claimsPrincipal = new ClaimsPrincipal(identity);
                await context.SignInAsync(claimsPrincipal);
            }
        }

        public static async Task ChangePasswordClaimAsync(this HttpContext context, string newPassword)
        {
            if (context.User.Identity is ClaimsIdentity identity)
            {
                var password = identity.FindFirst("Password");
                if (password is not null && newPassword != password.Value)
                {
                    identity.RemoveClaim(password);
                    identity.AddClaim(new Claim("Password", newPassword));
                }
                var claimsPrincipal = new ClaimsPrincipal(identity);
                await context.SignInAsync(claimsPrincipal);
            }
        }
    }
}
