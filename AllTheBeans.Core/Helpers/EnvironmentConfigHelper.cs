using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheBeans.Core.Helpers
{
    // 
    public static class EnvironmentConfigHelper
    {
        public static string GetDbConnectionString()
        {
            return Environment.GetEnvironmentVariable("ALL-THE-BEANS-CONNSTRING", EnvironmentVariableTarget.Machine) ?? Environment.GetEnvironmentVariable("ALL-THE-BEANS-CONNSTRING");
        }
    }
}
