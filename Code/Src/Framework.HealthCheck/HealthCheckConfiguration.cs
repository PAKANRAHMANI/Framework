namespace Framework.HealthCheck;

/// <summary>
/// 
/// </summary>
/// <param name="LivenessPort"></param>
/// <param name="LivenessDelay">per second</param>
/// <param name="ReadinessPort"></param>
public class HealthCheckConfiguration
{
    public int ReadinessPort { get; set; }
    public int LivenessPort { get; set; }
    public int LivenessDelay { get; set; }
}