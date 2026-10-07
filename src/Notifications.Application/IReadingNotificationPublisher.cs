namespace Notifications.Application;

public interface IReadingNotificationPublisher
{
    Task PublishReadingAsync(ReadingNotification notification, CancellationToken cancellationToken);
}
