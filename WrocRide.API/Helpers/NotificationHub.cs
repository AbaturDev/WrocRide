using Microsoft.AspNetCore.SignalR;

namespace WrocRide.API.Helpers
{
    public class NotificationHub : Hub
    {
        public async Task NotifyUser(string userId, string content)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", content);
        }
    }
}
