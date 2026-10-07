using DataIngestor.Contracts;

namespace Notifications.Application;

public sealed record ReadingNotification(string Room, MetricReadingPayload Payload, DateTime IngestedAtUtc);
