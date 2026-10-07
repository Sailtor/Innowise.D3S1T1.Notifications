namespace Notifications.Infrastructure.Options;

public class ThresholdOptions
{
    public const string SectionName = "Thresholds";

    public double Co2Threshold { get; init; } = 1000;

    public double Pm25Threshold { get; init; } = 35;

    public bool MotionAlertEnabled { get; init; } = true;
}
