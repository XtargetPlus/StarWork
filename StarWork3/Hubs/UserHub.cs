using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StarWork3.SubFunctions;
using System.Security.Claims;

namespace StarWork3.Hubs
{
    [Authorize]
    public class UserHub : Hub
    {
        private readonly static ConnectionMapping _connections = new();

        public async Task CheckConnections(List<string> users)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("CheckedConnections", _connections.CheckRangeConnection(users));
        }

        public override Task OnConnectedAsync()
        {
            _connections.Add(Context.ConnectionId, Context.User.FindFirstValue("UserName"));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
