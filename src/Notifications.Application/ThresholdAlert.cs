namespace Notifications.Application;

public sealed record ThresholdAlert(
    string Room,
    string ReadingType,
    string Rule,
    double Value,
    double Threshold,
    DateTime IngestedAtUtc);
