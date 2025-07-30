

namespace Utility
{
    public static class TestTargetSystem
    {
        public static string GetTargetIp()
        {
            var ip = NUnit.Framework.TestContext.Parameters.Get("TargetSystemIP");
            return !string.IsNullOrEmpty(ip) ? ip : "10.0.0.2"; //IP value which is in config file
        }
    }
}
