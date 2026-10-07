using Microsoft.AspNetCore.SignalR;

namespace Notifications.Presentation;

public sealed class MetricsHub : Hub
{
    public Task JoinRoom(string room) => Groups.AddToGroupAsync(Context.ConnectionId, room);

    public Task LeaveRoom(string room) => Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
}
