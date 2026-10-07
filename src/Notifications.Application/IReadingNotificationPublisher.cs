namespace Notifications.Application;

public interface IReadingNotificationPublisher
{
    Task PublishReadingAsync(ReadingNotification notification, CancellationToken cancellationToken);

    Task PublishAlertAsync(ThresholdAlert alert, CancellationToken cancellationToken);
}
