using StarWork3.Hubs;

namespace StarWork3.Configures
{
    public static class ApplicationConfigurations
    {
        public static IEndpointRouteBuilder UserCustomsHubs(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<UserHub>("/connection");
            return builder;
        }
    }
}
