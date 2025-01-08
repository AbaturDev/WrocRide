using Microsoft.AspNetCore.SignalR;
using WrocRide.Entities;

namespace WrocRide.Helpers
{
    public class NotificationHub : Hub
    {
        public async Task NotifyUser(string userId, string content)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", content);
        }
    }
}
