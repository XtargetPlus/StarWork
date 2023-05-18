using BLRequests.Repositories.Profile;
using BLRequests.Repositories.Profile.Concrete;
using BLRequests.ServicesLayer.Chats;
using BLRequests.ServicesLayer.Chats.Concrete;
using BLRequests.ServicesLayer.Friends;
using BLRequests.ServicesLayer.Friends.Concrete;

namespace StarWork3.Configures
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IFriendsService, FriendsService>();
            services.AddTransient<IChatService, ChatService>();
            return services;
        }
    }
}
