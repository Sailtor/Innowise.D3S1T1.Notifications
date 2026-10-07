using DataIngestor.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notifications.Application;

namespace Notifications.Infrastructure.Messaging;

public class MetricReadingsBatchConsumer(
    IReadingNotificationPublisher publisher,
    ILogger<MetricReadingsBatchConsumer> logger) : IConsumer<MetricReadingsBatch>
{
    public async Task Consume(ConsumeContext<MetricReadingsBatch> context)
    {
        MetricReadingsBatch batch = context.Message;

        logger.LogInformation(
            "Received batch of {Count} metric readings ingested at {IngestedAtUtc}",
            batch.Readings.Count,
            batch.IngestedAtUtc);

        foreach (MetricReadingMessage reading in batch.Readings)
        {
            ReadingNotification notification = new(reading.Room, reading.Payload, batch.IngestedAtUtc);
            await publisher.PublishReadingAsync(notification, context.CancellationToken);
        }
    }
}
