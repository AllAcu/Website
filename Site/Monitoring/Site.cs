using Its.Log.Monitoring;

namespace AllAcu.Monitoring
{
    public class Site : IMonitoringTest
    {
        public string Health()
        {
            return "Healthy!";
        }
    }
}
