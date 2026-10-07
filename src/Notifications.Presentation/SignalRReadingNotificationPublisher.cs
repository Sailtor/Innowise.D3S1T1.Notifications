using Microsoft.AspNetCore.SignalR;
using Notifications.Application;

namespace Notifications.Presentation;

public sealed class SignalRReadingNotificationPublisher(IHubContext<MetricsHub> hubContext) : IReadingNotificationPublisher
{
    public Task PublishReadingAsync(ReadingNotification notification, CancellationToken cancellationToken) =>
        hubContext.Clients.Group(notification.Room).SendAsync("ReceiveReading", notification, cancellationToken);
}
