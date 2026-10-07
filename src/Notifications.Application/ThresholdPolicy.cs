namespace Notifications.Application;

public sealed record ThresholdPolicy(double Co2Threshold, double Pm25Threshold, bool MotionAlertEnabled);
