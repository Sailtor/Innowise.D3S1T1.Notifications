using DataIngestor.Contracts;

namespace Notifications.Application;

public interface IThresholdRuleEvaluator
{
    IReadOnlyList<ThresholdAlert> Evaluate(MetricReadingMessage reading, DateTime ingestedAtUtc, ThresholdPolicy policy);
}
