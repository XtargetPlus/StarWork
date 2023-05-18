using BLRequests.Dtos.Profile;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace StarWork3.SubFunctions
{
    public static class ChangeUserCookies
    {
        public static void ChangeCookiesAsync(this HttpResponse response, HttpRequest request, MyProfileInfoDto profileInfo)
        {
            if (profileInfo.Age != 0)
            {
                if (request.Cookies.TryGetValue("age", out string? age))
                {
                    if (age is not null && int.Parse(age) != profileInfo.Age)
                    {
                        response.Cookies.Delete("age");
                        response.Cookies.Append("age", profileInfo.Age.ToString());
                    }
                }
            }
        }
    }
}
