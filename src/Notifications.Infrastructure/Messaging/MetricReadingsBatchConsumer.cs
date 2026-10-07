using DataIngestor.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notifications.Application;
using Notifications.Infrastructure.Options;

namespace Notifications.Infrastructure.Messaging;

public class MetricReadingsBatchConsumer(
    IReadingNotificationPublisher publisher,
    IThresholdRuleEvaluator evaluator,
    IOptions<ThresholdOptions> thresholdOptions,
    ILogger<MetricReadingsBatchConsumer> logger) : IConsumer<MetricReadingsBatch>
{
    public async Task Consume(ConsumeContext<MetricReadingsBatch> context)
    {
        MetricReadingsBatch batch = context.Message;

        logger.LogInformation(
            "Received batch of {Count} metric readings ingested at {IngestedAtUtc}",
            batch.Readings.Count,
            batch.IngestedAtUtc);

        ThresholdOptions options = thresholdOptions.Value;
        ThresholdPolicy policy = new(options.Co2Threshold, options.Pm25Threshold, options.MotionAlertEnabled);

        foreach (MetricReadingMessage reading in batch.Readings)
        {
            ReadingNotification notification = new(reading.Room, reading.Payload, batch.IngestedAtUtc);
            await publisher.PublishReadingAsync(notification, context.CancellationToken);

            foreach (ThresholdAlert alert in evaluator.Evaluate(reading, batch.IngestedAtUtc, policy))
            {
                await publisher.PublishAlertAsync(alert, context.CancellationToken);
            }
        }
    }
}
