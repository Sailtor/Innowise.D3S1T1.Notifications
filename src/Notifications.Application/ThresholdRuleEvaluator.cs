using DataIngestor.Contracts;

namespace Notifications.Application;

public sealed class ThresholdRuleEvaluator : IThresholdRuleEvaluator
{
    public IReadOnlyList<ThresholdAlert> Evaluate(MetricReadingMessage reading, DateTime ingestedAtUtc, ThresholdPolicy policy)
    {
        return reading.Payload switch
        {
            AirQualityReadingPayload airQuality => EvaluateAirQuality(reading.Room, airQuality, ingestedAtUtc, policy),
            MotionReadingPayload motion => EvaluateMotion(reading.Room, motion, ingestedAtUtc, policy),
            _ => [],
        };
    }

    private static List<ThresholdAlert> EvaluateAirQuality(
        string room,
        AirQualityReadingPayload payload,
        DateTime ingestedAtUtc,
        ThresholdPolicy policy)
    {
        List<ThresholdAlert> alerts = [];

        if (payload.Co2 > policy.Co2Threshold)
        {
            alerts.Add(new ThresholdAlert(
                room,
                "AirQuality",
                "AirQuality.Co2.ExceedsThreshold",
                payload.Co2,
                policy.Co2Threshold,
                ingestedAtUtc));
        }

        if (payload.Pm25 > policy.Pm25Threshold)
        {
            alerts.Add(new ThresholdAlert(
                room,
                "AirQuality",
                "AirQuality.Pm25.ExceedsThreshold",
                payload.Pm25,
                policy.Pm25Threshold,
                ingestedAtUtc));
        }

        return alerts;
    }

    private static List<ThresholdAlert> EvaluateMotion(
        string room,
        MotionReadingPayload payload,
        DateTime ingestedAtUtc,
        ThresholdPolicy policy)
    {
        if (!policy.MotionAlertEnabled || !payload.IsDetected)
        {
            return [];
        }

        return [new ThresholdAlert(room, "Motion", "Motion.Detected", 1.0, 0.0, ingestedAtUtc)];
    }
}
